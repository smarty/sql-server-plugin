using NUnit.Framework;

namespace SmartySqlServerPluginTests
{
    [TestFixture]
    public class AuthTests
    {
        [Test]
        public void TestValidJson()
        {
            AuthJson authJson = new AuthJson().ReadJsonFromFile("C:\\testfiles\\testauth.json");
            Assert.AreEqual("test" , authJson.auth_id);
            Assert.AreEqual("test", authJson.auth_token);
            Assert.AreEqual("google.com", authJson.url);
        }
        
        [Test]
        public void TestBlankJson() => Assert.AreEqual(null, new AuthJson().ReadJson(""));
        
        [Test]
        public void TestNoFields()
        {
            AuthJson authJson = new AuthJson().ReadJson("{}");
            Assert.IsNull(authJson.auth_id);
            Assert.IsNull(authJson.auth_token);
            Assert.IsNull(authJson.url);
        }
        
        [Test]
        public void TestNotEnoughFields()
        {
            AuthJson authJson = new AuthJson().ReadJson("{auth_id: \"test\"}");
            Assert.AreEqual("test" , authJson.auth_id);
            Assert.IsNull(authJson.auth_token);
            Assert.IsNull(authJson.url);
        }

        [Test]
        public void TestTooManyFields()
        {
            AuthJson authJson = new AuthJson().ReadJson("{auth_id: \"test\", auth_token: \"test2\", url: \"google.com\", auth_extra: \"extra\"}");
            Assert.AreEqual("test" , authJson.auth_id);
            Assert.AreEqual("test2", authJson.auth_token);
            Assert.AreEqual("google.com", authJson.url);
        }
    }
}