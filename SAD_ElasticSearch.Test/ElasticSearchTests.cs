using NUnit.Framework;
using Moq;
using SAD_ElasticSearch.Core.Interfaces;

namespace SAD_ElasticSearch.Test
{
    [TestFixture]
    public class Tests
    {
        private Mock<IElasticSearch> _elasticSearch;

        [SetUp]
        public void Setup()
        {
            _elasticSearch = new Mock<IElasticSearch>();


            //_elasticSearch.Setup(x => x.CheckClusterHealth<>)
        }

        [Test]
        public void ElasticSearch_ConnectToInstance_ReturnsTrue()
        {
            Assert.Pass();
        }
    }
}