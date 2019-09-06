using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Libplanet.Stun;
using Serilog;

namespace Libplanet.Net
{
    public class IceServer
    {
        public IceServer(
            IEnumerable<string> urls,
            string username = null,
            string credential = null)
            : this(urls.Select(u => new Uri(u)), username, credential)
        {
        }

        public IceServer(
            IEnumerable<Uri> urls,
            string username = null,
            string credential = null)
        {
            Urls = urls;
            Username = username;
            Credential = credential;
        }

        public IEnumerable<Uri> Urls { get; }

        public string Username { get; }

        public string Credential { get; }

        internal static async Task<TurnClient> CreateTurnClient(
            IEnumerable<IceServer> iceServers)
        {
            ILogger logger = Log.ForContext<IceServer>();
            foreach (IceServer server in iceServers)
            {
                foreach (Uri url in server.Urls)
                {
                    if (url.Scheme != "turn")
                    {
                        var msg = $"{nameof(IceServer)} currently only supports turn:// url; " +
                            "{0} is ignored.";
                        logger.Information(msg, url);
                        continue;
                    }

                    try
                    {
                        int port = url.IsDefaultPort ? TurnClient.TurnDefaultPort : url.Port;
                        var turnClient = new TurnClient(
                            url.Host,
                            server.Username,
                            server.Credential,
                            port);

                        // Check connectability
                        logger.Debug("Check if {0} is reachable...", url);
                        await turnClient.GetMappedAddressAsync();

                        logger.Debug("{0} is reachable: {1}.", url, turnClient);
                        return turnClient;
                    }
                    catch (ArgumentException e)
                    {
                        logger.Information(
                            e,
                            "Something went wrong with {0} (user: {1}): {2}",
                            url,
                            server.Username,
                            e
                        );
                        continue;
                    }
                    catch (SocketException)
                    {
                        logger.Information("{0} is unreachable.", url);
                        continue;
                    }
                }
            }

            throw new IceServerException("Can't find suitable server.");
        }
    }
}
