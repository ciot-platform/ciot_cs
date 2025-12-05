using Ciot.Protos.V2;
using Ciot.Sdk.Domain.Interfaces;
using Ciot.Sdk.Errors;
using LanguageExt;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading.Tasks;

namespace Ciot.Sdk.Infra.Interfaces
{
    public class Uart : IfaceBase
    {
        private readonly SerialPort port;
        private readonly IDecoder decoder;
        private readonly UartStatus status;
        private UartCfg cfg;
        private TaskCompletionSource<Either<ErrorBase, byte[]>> decode;

        private byte[] decoded;
        bool eventInitialized = false;

        public Uart(uint id)
        {
            decoder = new DecoderS();
            status = new UartStatus();
            port = new SerialPort();
            Info = new IfaceInfo
            {
                Id = id,
                Type = IfaceType.Uart,
            };
            decoded = new byte[1023];
            decode = new TaskCompletionSource<Either<ErrorBase, byte[]>>();
        }

        public Err Start(UartCfg cfg)
        {
            this.cfg = cfg;
            port.PortName = $"COM{cfg.Num}";
            port.BaudRate = (int)cfg.BaudRate;
            port.DtrEnable = cfg.Dtr;
            port.StopBits = StopBits.One;
            port.ReadTimeout = (int)cfg.ReadTimeout;
            port.WriteTimeout = (int)cfg.WriteTimeout;
            try
            {
                if(eventInitialized == false)
                {
                    port.DataReceived += Port_DataReceived;
                    eventInitialized = true;
                }
                port.Open();
                RaiseEvent(new Event
                {
                    Type = EventType.Started,
                });
                return Err.Ok;
            }
            catch (Exception)
            {
                return Err.Exception;
            }
        }

        public override Err Stop()
        {
            try
            {
                port.Close();
                return Err.Ok;
            }
            catch (Exception)
            {
                return Err.Exception;
            }
        }

        public override Err GetData(MsgData data)
        {
            if (data.TypeCase != MsgData.TypeOneofCase.Uart)
            {
                return Err.InvalidType;
            }

            switch (data.Uart.TypeCase)
            {
                case UartData.TypeOneofCase.Config:
                    data.Uart.Config = cfg;
                    break;
                case UartData.TypeOneofCase.Status:
                    data.Uart.Status = status;
                    break;
                default:
                    return Err.InvalidType;
            }

            return Err.Ok;
        }

        public override Err ProcessData(MsgData data)
        {
            if (data.TypeCase != MsgData.TypeOneofCase.Uart)
            {
                return Err.InvalidType;
            }

            switch (data.Uart.TypeCase)
            {
                case UartData.TypeOneofCase.Config:
                    Start(data.Uart.Config);
                    break;
                case UartData.TypeOneofCase.Request:
                    return Err.NotImplemented;
                default:
                    return Err.InvalidType;
            }

            return Err.Ok;
        }

        public override async Task<Either<ErrorBase, byte[]>> SendData(byte[] data)
        {
            try
            {
                if (!port.IsOpen) port.Open();

                // (Re)cria TaskCompletionSource para esta chamada
                decode = new TaskCompletionSource<Either<ErrorBase, byte[]>>();

                // Determina timeout em ms a partir de cfg.ReadTimeout (fallback para 5000 ms)
                int timeoutMs = 5000;
                try
                {
                    if (this.cfg != null)
                    {
                        timeoutMs = (int)this.cfg.ReadTimeout;
                    }
                    else if (port.ReadTimeout > 0)
                    {
                        timeoutMs = port.ReadTimeout;
                    }
                }
                catch { timeoutMs = 5000; }

                // Agendar timeout: se expirar antes de receber resposta, completar com Timeout
                _ = Task.Run(async () =>
                {
                    await Task.Delay(timeoutMs).ConfigureAwait(false);
                    if (decode.Task.IsCompleted == false)
                    {
                        try
                        {
                            if (port.IsOpen)
                            {
                                port.DiscardInBuffer();
                            }
                        }
                        catch { /* ignore discard exceptions */ }

                        decoder.Reset();
                        decode.TrySetResult(new ErrorException(new TimeoutException($"Uart.SendData timeout after {timeoutMs} ms")));
                    }
                });

                SendBytes(data);
                return await decode.Task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new ErrorException(ex);
            }
        }

        public void SendBytes(byte[] bytes)
        {
            if(port.IsOpen)
            {
                var encoded = decoder.Encode(bytes);
                port.Write(encoded, 0, encoded.Length);
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[port.BytesToRead];
            port.Read(data, 0, data.Length);


            for (int i = 0; i < data.Length; i++)
            {
                if (decoder.Decode(data[i], out decoded))
                {
                    if(decode?.Task.IsCompleted == false)
                    {
                        // usar TrySetResult para evitar InvalidOperationException em corrida
                        decode.TrySetResult(decoded);
                    }
                    var ev = new Event
                    {
                        Type = EventType.Data,
                        Msg = Msg.Parser.ParseFrom(decoded),
                    };
                    RaiseEvent(ev);
                }
            }

            //if (decode.Task.IsCompleted == false)
            //{
            //    try
            //    {
            //        port.DiscardInBuffer();
            //    }
            //    catch { /* ignore discard exceptions */ }
            //    decoder.Reset();
            //    // TrySetResult para evitar exceção de tarefa já concluída
            //    decode.TrySetResult(new ErrorException(new FormatException()));
            //}
        }
    }
}
