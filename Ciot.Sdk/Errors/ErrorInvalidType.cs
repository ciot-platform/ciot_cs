using Ciot.Protos.V2;
using Grpc.Core;

namespace Ciot.Sdk.Errors
{
    public class ErrorInvalidType : ErrorBase
    {
        public ErrorInvalidType(string message = "Invalid Type") : base(StatusCode.Internal, Err.InvalidType, message)
        {

        }
    }
}
