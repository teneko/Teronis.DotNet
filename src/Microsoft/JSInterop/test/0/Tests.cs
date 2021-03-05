using System;
using System.Threading.Tasks;
using Teronis.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace Teronis.Microsoft.JSInterop.Test.Server
{
    public class Tests //: IClassFixture<ServerFixture>

    //public ServerFixture Server { get; }
    {
        private readonly ITestOutputHelper output;

        public Tests(ITestOutputHelper output) =>
            this.output = output;

        string error

        [Fact]
        public async Task Should()
        {
            void writeLine(string data) =>
                output.WriteLine(data ?? string.Empty);

            await SimpleProcess.RunAsync("dotnet", "run", workingDirectory: "",  outputReceived: writeLine);

            ;
        }
    }
}
