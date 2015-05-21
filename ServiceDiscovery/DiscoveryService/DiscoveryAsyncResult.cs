using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Discovery;
using System.Web;

namespace WcfService1
{
    internal sealed class OnOnlineAnnouncementAsyncResult : AsyncResult
    {
        public OnOnlineAnnouncementAsyncResult(AsyncCallback callback, object state)
            : base(callback, state)
        {
            this.Complete(true);
        }

        public static void End(IAsyncResult result)
        {
            AsyncResult.End<OnOnlineAnnouncementAsyncResult>(result);
        }

    }

    sealed class OnOfflineAnnouncementAsyncResult : AsyncResult
    {
        public OnOfflineAnnouncementAsyncResult(AsyncCallback callback, object state)
            : base(callback, state)
        {
            this.Complete(true);
        }

        public static void End(IAsyncResult result)
        {
            AsyncResult.End<OnOfflineAnnouncementAsyncResult>(result);
        }
    }

    sealed class OnFindAsyncResult : AsyncResult
    {
        public OnFindAsyncResult(AsyncCallback callback, object state)
            : base(callback, state)
        {
            this.Complete(true);
        }

        public static void End(IAsyncResult result)
        {
            AsyncResult.End<OnFindAsyncResult>(result);
        }
    }

    sealed class OnResolveAsyncResult : AsyncResult
    {
        EndpointDiscoveryMetadata matchingEndpoint;

        public OnResolveAsyncResult(EndpointDiscoveryMetadata matchingEndpoint, AsyncCallback callback, object state)
            : base(callback, state)
        {
            this.matchingEndpoint = matchingEndpoint;
            this.Complete(true);
        }

        public static EndpointDiscoveryMetadata End(IAsyncResult result)
        {
            OnResolveAsyncResult thisPtr = AsyncResult.End<OnResolveAsyncResult>(result);
            return thisPtr.matchingEndpoint;
        }
    }
}