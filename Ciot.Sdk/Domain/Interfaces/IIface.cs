using Ciot.Protos.V2;
using Ciot.Sdk.Errors;
using LanguageExt;
using System;
using System.Threading.Tasks;

namespace Ciot.Sdk.Domain.Interfaces
{
    public interface IIface
    {
        event EventHandler<Event> OnEvent;
        IfaceInfo Info { get; }
        Err Stop();
        Err ProcessData(MsgData data);
        Task<Either<ErrorBase, byte[]>> SendData(byte[] data);
        Err GetData(MsgData data);
    }
}
