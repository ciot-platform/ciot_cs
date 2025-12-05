using LanguageExt;
using System.Collections.Generic;
using Ciot.Protos.V2;
using Ciot.Sdk.Errors;

namespace Ciot.Sdk.Domain.Repositories
{
    public interface IIfaceRepository
    {
        Either<ErrorBase, Msg> CreateIface(Msg msg);

        Either<ErrorBase, Msg> GetIfaceById(uint id);

        Either<ErrorBase, List<Msg>> GetIfaces();

        Either<ErrorBase, Msg> UpdateIface(Msg msg);

        Either<ErrorBase, Msg> DeleteIface(uint id);
    }
}
