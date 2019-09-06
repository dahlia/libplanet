using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Libplanet.Net;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Libplanet.Tests.Net
{
    public class IceServerTest
    {
        private const int Timeout = 60 * 1000;

        public IceServerTest(ITestOutputHelper output)
        {
            const string outputTemplate = "{Timestamp:HH:mm:ss}[@Urls][{ThreadId}] - {Message}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithThreadId()
                .WriteTo.TestOutput(output, outputTemplate: outputTemplate)
                .CreateLogger()
                .ForContext<IceServerTest>();
        }

        [FactOnlyTurnAvailable(Timeout = Timeout)]
        public async Task CreateTurnClient()
        {
            var turnUri = new Uri(
                Environment.GetEnvironmentVariable(
                    FactOnlyTurnAvailable.TurnUrlVarName));
            var userInfo = turnUri.UserInfo.Split(':');
            var servers = new List<IceServer>()
            {
                new IceServer(new[] { "turn:turn.does-not-exists.org" }),
                new IceServer(new[] { "stun:stun.l.google.com:19302" }),
            };

            Log.Debug("Attempt #1; servers ({0}): {1}", servers.Count, servers);
            await Assert.ThrowsAsync<IceServerException>(
                async () => { await IceServer.CreateTurnClient(servers); });

            servers.Add(new IceServer(new[] { turnUri }, userInfo[0], userInfo[1]));
            Log.Debug("Attempt #2; servers ({0}): {1}", servers.Count, servers);
            var turnClient = await IceServer.CreateTurnClient(servers);

            Assert.Equal(userInfo[0], turnClient.Username);
            Assert.Equal(userInfo[1], turnClient.Password);
        }
    }
}
