using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;

namespace ForecastMonitor.Test.Unit
{
    [TestFixture]
    public abstract class BaseUnitTest
    {
        protected IFixture Fixture;

        [SetUp]
        public void Setup()
        {
            Fixture = new Fixture().Customize(new AutoMoqCustomization());
        }
    }
}
