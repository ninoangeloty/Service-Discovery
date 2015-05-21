using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static EndpointAddress FindServiceEndpoint()
        {
            var probeEndpointAddress = new EndpointAddress(ConfigurationManager.AppSettings["probeEndpointAddress"]);
            var probeBinding = Activator.CreateInstance(Type.GetType(ConfigurationManager.AppSettings["bindingType"], true, true)) as Binding;
            var discoveryEndpoint = new DiscoveryEndpoint(probeBinding, probeEndpointAddress);

            EndpointAddress address = null;
            FindResponse result = null;
            using (var discoveryClient = new DiscoveryClient(discoveryEndpoint))
            {
                result = discoveryClient.Find(new FindCriteria(typeof(IStringService)));
            }

            if (result != null && result.Endpoints.Any())
            {
                var endpointMetadata = result.Endpoints.First();
                address = endpointMetadata.Address;
            }
            return address;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Say something...");
            var content = Console.ReadLine();
            while (!string.IsNullOrWhiteSpace(content))
            {
                Console.WriteLine("Finding the service endpoint...");
                var address = FindServiceEndpoint();
                if (address == null)
                {
                    Console.WriteLine("There is no endpoint matches the criteria.");
                }
                else
                {
                    Console.WriteLine("Found the endpoint {0}", address.Uri);

                    var factory = new ChannelFactory<IStringService>(new NetTcpBinding(), address);
                    factory.Opened += (sender, e) =>
                    {
                        Console.WriteLine("Connecting to {0}.", factory.Endpoint.ListenUri);
                    };
                    var proxy = factory.CreateChannel();
                    using (proxy as IDisposable)
                    {
                        Console.WriteLine("ToUpper: {0} => {1}", content, proxy.ToUpper(content));
                    }
                }

                Console.WriteLine("Say something...");
                content = Console.ReadLine();
            }
        }
    }

    [ServiceContract]
    public interface IStringService
    {
        [OperationContract]
        string ToUpper(string content);
    }
}
