using Ciot.Protos.V2;
using Ciot.Sdk.Domain.Interfaces;
using System.Diagnostics;

namespace Ciot.App.WifiConfig
{
    internal class WifiConfig
    {
        IfaceBase _iface;
        uint _wifiApIfaceId;

        public WifiConfig(IfaceBase iface, uint wifiApIfaceId)
        {
            _iface = iface;
            _wifiApIfaceId = wifiApIfaceId;
        }

        public async Task<WifiCfg?> GetConfig()
        {
            int attempts = 5;
            while(attempts > 0)
            {
                Debug.WriteLine($"Tentativa de obter configuração WiFi. Tentativas restantes: {attempts}");
                try
                {
                    var msg = new Msg
                    {
                        Iface = new IfaceInfo
                        {
                            Id = _wifiApIfaceId,
                            Type = IfaceType.Wifi,
                        },
                        Data = new MsgData
                        {
                            GetData = new GetData
                            {
                                Type = DataType.Config,
                            }
                        }
                    };
                    var result = await _iface.SendMsg(msg);
                    if(result.IsLeft)
                    {
                        attempts--;
                    }
                    else
                    {
                        return result.Match(
                            r => r.Data.Wifi.Config,
                            l => null);
                    }
                }
                catch (Exception ex)
                {
                    attempts--;
                    if(attempts == 0)
                    {
                        throw new Exception("Não foi possível obter as configurações do WiFi após múltiplas tentativas.", ex);
                    }
                    // Aguarda um breve período antes de tentar novamente
                    await Task.Delay(500);
                }
            }
            return null;            
        }

        public async Task SendConfig(string ssid, string password)
        {
            var msg = new Msg
            {
                Iface = new IfaceInfo
                {
                    Id = _wifiApIfaceId,
                    Type = IfaceType.Wifi,
                },
                Data = new MsgData
                {
                    Wifi = new WifiData
                    {
                        Config = new WifiCfg
                        {
                            Type = WifiType.Ap,
                            Ssid = ssid,
                            Password = password,
                        }
                    }
                }
            };
            var result = await _iface.SendMsg(msg);
            if(result.IsLeft)
            {
                throw new Exception("Falha ao enviar configurações do wifi");
            }
            else
            {
                await SaveConfig();
            }
        }

        public async Task SaveConfig()
        {
            var msg = new Msg
            {
                Iface = new IfaceInfo
                {
                    Id = 0,
                    Type = IfaceType.Ciot,
                },
                Data = new MsgData
                {
                    Ciot = new Data
                    {
                        Request = new Req
                        {
                            SaveCfg = new IfaceInfo
                            {
                                Id = _wifiApIfaceId,
                                Type = IfaceType.Wifi,
                            }
                        }
                    }
                }
            };
            var result = await _iface.SendMsg(msg);
            if (result.IsLeft)
            {
                throw new Exception("Failed to save WiFi config");
            }
            //await ResetDevice();
        }
    }
}
