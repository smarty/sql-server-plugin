using Newtonsoft.Json;
using System.IO;

namespace Auth
{
    public class AuthJson
    {
        public string auth_id { get; set; }

        public string auth_token { get; set; }

        public string url { get; set; }

        public AuthJson ReadJsonFromFile(string filename) => JsonConvert.DeserializeObject<AuthJson>(File.ReadAllText(filename));

        public AuthJson ReadJson(string json) => JsonConvert.DeserializeObject<AuthJson>(json);
    }
}