using Ciot.Protos.V2;
using Ciot.Sdk.Errors;
using Ciot.Sdk.Infra.Interfaces;
using LanguageExt;
using System;
using System.Collections.Generic;

namespace Ciot.Sdk.Domain.Interfaces
{
    public interface IIfaceEventManager
    {
        event EventHandler<Event> OnEvent;
        Either<ErrorBase, Unit> Subscribe(IIface iface);
        Either<ErrorBase, Unit> Unsubscribe(IIface iface);
        Either<ErrorBase, List<IfaceEventManagerSubscription>> GetSubscriptions();
    }
}
