using Ciot.Protos.V2;
using Grpc.Core;

namespace Ciot.Sdk.Errors
{
    public class ErrorNotImplemented : ErrorBase
    {
        public ErrorNotImplemented(string message = "Not implemented") : base(StatusCode.Internal, Err.NotImplemented, message)
        {

        }
    }
}
