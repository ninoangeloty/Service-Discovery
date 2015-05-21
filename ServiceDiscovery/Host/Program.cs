using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;
using WcfService1;

namespace ServiceDiscovery01
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(new ManagedProxyDiscoveryService()))
            {
                host.Opened += (sender, e) =>
                {
                    host.Description.Endpoints.All((ep) =>
                    {
                        Console.WriteLine(ep.ListenUri);
                        return true;
                    });
                };

                try
                {
                    // retrieve the announcement, probe endpoint and binding from configuration
                    var announcementEndpointAddress = new EndpointAddress(ConfigurationManager.AppSettings["announcementEndpointAddress"]);
                    var probeEndpointAddress = new EndpointAddress(ConfigurationManager.AppSettings["probeEndpointAddress"]);
                    var binding = Activator.CreateInstance(Type.GetType(ConfigurationManager.AppSettings["bindingType"], true, true)) as Binding;
                    var announcementEndpoint = new AnnouncementEndpoint(binding, announcementEndpointAddress);
                    var probeEndpoint = new DiscoveryEndpoint(binding, probeEndpointAddress);
                    probeEndpoint.IsSystemEndpoint = false;

                    // append the service endpoint for announcement and probe
                    host.AddServiceEndpoint(announcementEndpoint);
                    host.AddServiceEndpoint(probeEndpoint);

                    host.Open();

                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
