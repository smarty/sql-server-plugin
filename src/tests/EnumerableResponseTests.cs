namespace SmartySqlServerPluginTests
{
    using SmartySqlServerPlugin.Interop;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using SmartyStreets.USStreetApi;
    
    [TestFixture]
    public class EnumerableResponseTests
    {
        [Test]
        public void TestExceptionGivesSingleResponse()
        {
            EnumerableResponse<Candidate> enumerableResponse1 = new EnumerableResponse<Candidate>(500);
            foreach (EnumerableResponse<Candidate> enumerableResponse2 in enumerableResponse1.Cast<EnumerableResponse<Candidate>>())
            {
                Assert.AreEqual(500, enumerableResponse2.Status);
                Assert.IsNull(enumerableResponse2.Index);
                Assert.IsNotNull(enumerableResponse2.Candidate);
            }
            AssertCount(1, enumerableResponse1);
        }

        [Test]
        public void TestNoItemsGivesSingleElementResponse()
        {
            AssertEmptyResponse(new EnumerableResponse<Candidate>(null));
            AssertEmptyResponse(new EnumerableResponse<Candidate>(new List<Candidate>()));
        }

        [Test]
        public void TestSingleItemsGivesSingleElementResponse()
        {
            List<Candidate> candidates = new List<Candidate>()
      {
        new Candidate()
      };
            EnumerableResponse<Candidate> enumerableResponse = new EnumerableResponse<Candidate>(candidates);
            foreach (EnumerableResponse<Candidate> actual in enumerableResponse.Cast<EnumerableResponse<Candidate>>())
                AssertElement(0, candidates[0], actual);
            AssertCount(candidates.Count, enumerableResponse);
        }

        [Test]
        public void TestMultipleItemsGivesMultipleElementResponse()
        {
            List<Candidate> candidates = new List<Candidate>()
      {
        new Candidate(),
        new Candidate(),
        new Candidate()
      };
            EnumerableResponse<Candidate> enumerableResponse = new EnumerableResponse<Candidate>(candidates);
            int index = 0;
            foreach (EnumerableResponse<Candidate> actual in enumerableResponse.Cast<EnumerableResponse<Candidate>>())
                AssertElement(index, candidates[index++], actual);
            AssertCount(candidates.Count, enumerableResponse);
        }

        private static void AssertEmptyResponse(EnumerableResponse<Candidate> response)
        {
            foreach (EnumerableResponse<Candidate> enumerableResponse in response.Cast<EnumerableResponse<Candidate>>())
            {
                Assert.AreEqual(200, enumerableResponse.Status);
                Assert.AreEqual(-1, enumerableResponse.Index);
            }
            AssertCount(1, response);
        }

        private static void AssertCount(int expectedCount, EnumerableResponse<Candidate> response)
        {
            response.Reset();
            Assert.AreEqual(expectedCount, response.Cast<EnumerableResponse<Candidate>>().Count());
        }

        private static void AssertElement(
          int index,
          Candidate expected,
          EnumerableResponse<Candidate> actual)
        {
            Assert.AreEqual(200, actual.Status);
            Assert.AreEqual(index, actual.Index);
            Assert.AreSame(expected, actual.Candidate);
        }
    }
}