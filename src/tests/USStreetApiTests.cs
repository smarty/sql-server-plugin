using System;

namespace SmartySqlServerPluginTests
{
    using NUnit.Framework;
    using SmartySqlServerPlugin.Interop;
    using SmartySqlServerPlugin;
    using SmartyStreets.USStreetApi;
    
    [TestFixture]
    public class USStreetApiTests
    {
        private static AuthJson _auth = new AuthJson();

        [SetUp]
        public void TestInitialize()
        {
            _auth = _auth.ReadJsonFromFile("C:\\testfiles\\auth.json");
        }

        [Test]
        public void ValidFreeformAddress()
        {
            EnumerableResponse<Candidate> enumerator = (EnumerableResponse<Candidate>)USStreetApi.SmartyUSStreetVerifyFreeform("3214 N University Ave Provo UT 84604", 2, "", _auth.auth_id, _auth.auth_token, _auth.url).GetEnumerator();
            Assert.AreEqual(200, enumerator.Status);
            Assert.IsTrue(enumerator.MoveNext());
            Candidate candidate = enumerator.Candidate;
            Assert.AreEqual("3214 N University Ave", candidate.DeliveryLine1);
            Assert.AreEqual("Provo UT 84604-4405", candidate.LastLine);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void ValidFreeformAddressNoZip()
        {
            EnumerableResponse<Candidate> enumerator = (EnumerableResponse<Candidate>)USStreetApi.SmartyUSStreetVerifyFreeform("3214 N University Ave Provo UT",2,  "", _auth.auth_id, _auth.auth_token, _auth.url).GetEnumerator();
            Assert.AreEqual(200, enumerator.Status);
            Assert.IsTrue(enumerator.MoveNext());
            Candidate candidate = enumerator.Candidate;
            Assert.AreEqual("3214 N University Ave", candidate.DeliveryLine1);
            Assert.AreEqual("Provo UT 84604-4405", candidate.LastLine);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void ValidFreeformAddressNoCityState()
        {
            EnumerableResponse<Candidate> enumerator = (EnumerableResponse<Candidate>)USStreetApi.SmartyUSStreetVerifyFreeform("3214 N University Ave 84604", 2, "", _auth.auth_id, _auth.auth_token, _auth.url).GetEnumerator();
            Assert.AreEqual(200, enumerator.Status);
            Assert.IsTrue(enumerator.MoveNext());
            Candidate candidate = enumerator.Candidate;
            Assert.AreEqual("3214 N University Ave", candidate.DeliveryLine1);
            Assert.AreEqual("Provo UT 84604-4405", candidate.LastLine);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void ValidFreeformAddressTwoResults()
        {
            EnumerableResponse<Candidate> enumerator = (EnumerableResponse<Candidate>)USStreetApi.SmartyUSStreetVerifyFreeform("1109 9th Phoenix AZ", 2, "", _auth.auth_id, _auth.auth_token, _auth.url).GetEnumerator();
            Assert.AreEqual(200, enumerator.Status);
            Assert.IsTrue(enumerator.MoveNext());
            Candidate candidate = enumerator.Candidate;
            Assert.AreEqual("3646", candidate.Components.Plus4Code);
            Assert.AreEqual("1109 S 9th Ave", candidate.DeliveryLine1);
            Assert.IsTrue(enumerator.MoveNext());
            candidate = enumerator.Candidate;
            Assert.AreEqual("2734", candidate.Components.Plus4Code);
            Assert.AreEqual("1109 N 9th St", candidate.DeliveryLine1);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void InvalidFreeformAddress()
        {
            EnumerableResponse<Candidate> enumerator = (EnumerableResponse<Candidate>)USStreetApi.SmartyUSStreetVerifyFreeform("3214 N University Ave", 2, "", _auth.auth_id, _auth.auth_token, _auth.url).GetEnumerator();
            Assert.AreEqual(200, enumerator.Status);
            Assert.IsTrue(enumerator.MoveNext());
            Candidate candidate = enumerator.Candidate;
            Assert.IsNull(candidate.Components);
            Assert.IsNull(candidate.Metadata);
            Assert.IsNull(candidate.Analysis);
            Assert.IsFalse(enumerator.MoveNext());
        }
        
        [Test]
        public void InvalidFreeformNumCandidates()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => USStreetApi.SmartyUSStreetVerifyFreeform("",0, "", _auth.auth_id, _auth.auth_token, _auth.url).GetEnumerator());
        }

        [Test]
        public void FreeformEmptyAddress()
        {
            EnumerableResponse<Candidate> enumerator = (EnumerableResponse<Candidate>)USStreetApi.SmartyUSStreetVerifyFreeform("", 2, "", _auth.auth_id, _auth.auth_token, _auth.url).GetEnumerator();
            Assert.AreEqual(400, enumerator.Status);
            Assert.IsTrue(enumerator.MoveNext());
            Candidate candidate = enumerator.Candidate;
            Assert.IsNull(candidate.Components);
            Assert.IsNull(candidate.Metadata);
            Assert.IsNull(candidate.Analysis);
            Assert.IsFalse(enumerator.MoveNext());
        }
        
        [Test]
        public void InvalidAuth()
        {
            EnumerableResponse<Candidate> enumerator = (EnumerableResponse<Candidate>)USStreetApi.SmartyUSStreetVerifyFreeform("3214 N University Ave Provo UT 84604", 2, "", "", "", _auth.url).GetEnumerator();
            Assert.AreEqual(401, enumerator.Status);
            Assert.IsTrue(enumerator.MoveNext());
            Candidate candidate = enumerator.Candidate;
            Assert.IsNull(candidate.Components);
            Assert.IsNull(candidate.Metadata);
            Assert.IsNull(candidate.Analysis);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void ValidAddress()
        {
            EnumerableResponse<Candidate> enumerator = (EnumerableResponse<Candidate>)USStreetApi.SmartyUSStreetVerify("3214 N University Ave", "", "", "", "", "84604", "", "", "", 2, "", _auth.auth_id, _auth.auth_token, _auth.url).GetEnumerator();
            Assert.AreEqual(200, enumerator.Status);
            Assert.IsTrue(enumerator.MoveNext());
            Candidate candidate = enumerator.Candidate;
            Assert.IsNotNull(candidate.Components);
            Assert.AreEqual("UT", candidate.Components.State);
            Assert.IsNotNull(candidate.Metadata);
            Assert.AreEqual("Mountain", candidate.Metadata.TimeZone);
            Assert.IsNotNull(candidate.Analysis);
            Assert.AreEqual("Y", candidate.Analysis.Active);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void ValidAddressTwoResults()
        {
            EnumerableResponse<Candidate> enumerator = (EnumerableResponse<Candidate>)USStreetApi.SmartyUSStreetVerify("1109 9th", "", "", "Phoenix", "AZ", "", "", "", "", 2, "", _auth.auth_id, _auth.auth_token, _auth.url).GetEnumerator();
            Assert.AreEqual(200, enumerator.Status);
            Assert.IsTrue(enumerator.MoveNext());
            Candidate candidate = enumerator.Candidate;
            Assert.AreEqual("3646", candidate.Components.Plus4Code);
            Assert.AreEqual("1109 S 9th Ave", candidate.DeliveryLine1);
            Assert.IsTrue(enumerator.MoveNext());
            candidate = enumerator.Candidate;
            Assert.AreEqual("2734", candidate.Components.Plus4Code);
            Assert.AreEqual("1109 N 9th St", candidate.DeliveryLine1);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void InvalidAddress()
        {
            EnumerableResponse<Candidate> enumerator = (EnumerableResponse<Candidate>)USStreetApi.SmartyUSStreetVerify("", "", "", "", "", "", "", "", "", 2, "", _auth.auth_id, _auth.auth_token, _auth.url).GetEnumerator();
            Assert.AreEqual(400, enumerator.Status);
            Assert.IsTrue(enumerator.MoveNext());
            Candidate candidate = enumerator.Candidate;
            Assert.IsNull(candidate.Components);
            Assert.IsNull(candidate.Metadata);
            Assert.IsNull(candidate.Analysis);
            Assert.IsFalse(enumerator.MoveNext());
        }
    }
}