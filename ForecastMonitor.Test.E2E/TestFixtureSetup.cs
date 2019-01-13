using ForecastMonitor.Shared;
using NUnit.Framework;
using WireMock.Server;

namespace ForecastMonitor.Test.E2E
{
    [SetUpFixture]
    public class TestFixtureSetup
    {
        private const int PortStub1 = 6061;
        private const int PortStub2 = 6062;

        private FluentMockServer _stub1;
        private FluentMockServer _stub2;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            this._stub1 = FluentMockServer.Start(PortStub1);
            this._stub2 = FluentMockServer.Start(PortStub2);

            this._stub1.ConfigureClientWithFixedData();
            this._stub2.ConfigureClientWithFixedData();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this._stub1.Stop();
            this._stub2.Stop();
        }
    }
}
