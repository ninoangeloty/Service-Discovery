using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Web;

namespace WcfService1
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ManagedProxyDiscoveryService : DiscoveryProxy
    {
        private ConcurrentDictionary<EndpointAddress, EndpointDiscoveryMetadata> _services;

        public ManagedProxyDiscoveryService()
        {
            _services = new ConcurrentDictionary<EndpointAddress, EndpointDiscoveryMetadata>();
        }

        protected override IAsyncResult OnBeginOnlineAnnouncement(DiscoveryMessageSequence messageSequence, EndpointDiscoveryMetadata endpointDiscoveryMetadata, AsyncCallback callback, object state)
        {
            _services.AddOrUpdate(endpointDiscoveryMetadata.Address, endpointDiscoveryMetadata, (key, value) => endpointDiscoveryMetadata);
            return new OnOnlineAnnouncementAsyncResult(callback, state);
        }

        protected override void OnEndOnlineAnnouncement(IAsyncResult result)
        {
            OnOnlineAnnouncementAsyncResult.End(result);
        }

        protected override IAsyncResult OnBeginOfflineAnnouncement(DiscoveryMessageSequence messageSequence, EndpointDiscoveryMetadata endpointDiscoveryMetadata, AsyncCallback callback, object state)
        {
            EndpointDiscoveryMetadata endpoint = null;
            _services.TryRemove(endpointDiscoveryMetadata.Address, out endpoint);
            return new OnOfflineAnnouncementAsyncResult(callback, state);
        }

        protected override void OnEndOfflineAnnouncement(IAsyncResult result)
        {
            OnOfflineAnnouncementAsyncResult.End(result);
        }

        protected override IAsyncResult OnBeginFind(FindRequestContext findRequestContext, AsyncCallback callback, object state)
        {
            _services.Where(s => findRequestContext.Criteria.IsMatch(s.Value))
                     .Select(s => s.Value)
                     .All(meta =>
                     {
                         findRequestContext.AddMatchingEndpoint(meta);
                         return true;
                     });
            return new OnFindAsyncResult(callback, state);
        }

        protected override void OnEndFind(IAsyncResult result)
        {
            OnFindAsyncResult.End(result);
        }

        protected override IAsyncResult OnBeginResolve(ResolveCriteria resolveCriteria, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override EndpointDiscoveryMetadata OnEndResolve(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
    }
}