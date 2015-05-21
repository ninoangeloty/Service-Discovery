using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;

namespace DiscoverableService
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = new Uri(string.Format("net.tcp://localhost:11001/stringservice/{0}/", Guid.NewGuid().ToString()));

            using (var host = new ServiceHost(typeof(StringService), baseAddress))
            {
                host.Opened += (sender, e) =>
                {
                    Console.WriteLine("Service opened at {0}", host.Description.Endpoints.First().ListenUri);
                };

                host.AddServiceEndpoint(typeof(IStringService), new NetTcpBinding(), string.Empty);

                var announcementAddress = new EndpointAddress(ConfigurationManager.AppSettings["announcementEndpointAddress"]);
                var announcementBinding = Activator.CreateInstance(Type.GetType(ConfigurationManager.AppSettings["bindingType"], true, true)) as Binding;
                var announcementEndpoint = new AnnouncementEndpoint(announcementBinding, announcementAddress);
                var discoveryBehavior = new ServiceDiscoveryBehavior();
                discoveryBehavior.AnnouncementEndpoints.Add(announcementEndpoint);
                host.Description.Behaviors.Add(discoveryBehavior);

                host.Open();

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
