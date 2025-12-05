using Ciot.Protos.V2;
using Ciot.Sdk.Errors;
using LanguageExt;
using System;
using System.Threading.Tasks;

namespace Ciot.Sdk.Domain.Interfaces
{
    public abstract class IfaceBase : IIface
    {
        private readonly ISerializer _serializer;

        public IfaceInfo Info { get; protected set; }

        public event EventHandler<Event> OnEvent;

        public IfaceBase()
        {
            _serializer = Infra.Interfaces.SerializerPb.Instance;
        }

        public IfaceBase(ISerializer serializer)
        {
            _serializer = serializer;
        }


        public virtual Err GetData(MsgData data)
        {
            throw new NotImplementedException();
        }

        public virtual Err ProcessData(MsgData data)
        {
            throw new NotImplementedException();
        }

        public virtual Task<Either<ErrorBase, byte[]>> SendData(byte[] data)
        {
            throw new NotImplementedException();
        }

        public virtual Err Stop()
        {
            throw new NotImplementedException();
        }

        public async Task<Either<ErrorBase, Msg>> SendMsg(Msg msg)
        {
            var data = _serializer.Serialize(msg);
            var result = await SendData(data);
            return result.Match(
                Right: bytes =>
                {
                    var responseMsg = _serializer.Deserialize<Msg>(bytes);
                    return Prelude.Right<ErrorBase, Msg>(responseMsg);
                },
                Left: err => Prelude.Left<ErrorBase, Msg>(err)
            );
        }

        public async Task<Either<ErrorBase, T>> Send<T>(T msg) {
            var data = _serializer.Serialize(msg);
            var result = await SendData(data);
            return result.Match(
                Right: bytes =>
                {
                    var responseMsg = _serializer.Deserialize<T>(bytes);
                    return Prelude.Right<ErrorBase, T>(responseMsg);
                },
                Left: err => Prelude.Left<ErrorBase, T>(err)
            );

        }

        protected void RaiseEvent(Event evt)
        {
            OnEvent?.Invoke(this, evt);
        }
    }
}
