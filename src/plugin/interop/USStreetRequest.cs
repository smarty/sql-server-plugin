namespace SmartySqlServerPlugin.Interop
{
    using SmartyStreets;
    using SmartyStreets.USStreetApi;

    public class USStreetRequest : EnumerableRequest<Lookup, Candidate>
    {
        public USStreetRequest(string freeform, string id, string token, string auth) : this(new Lookup(freeform), new StaticCredentials(id, token), auth)
        {
        }

        public USStreetRequest(
            string street,
            string street2,
            string secondary,
            string city,
            string state,
            string zipcode,
            string lastline,
            string addressee,
            string urbanization,
            int maxCandidates,
            string matchStrategy,
            string id, string token, string url) : this(new Lookup
        {
            Street = street,
            Street2 = street2,
            Secondary = secondary,
            City = city,
            State = state,
            ZipCode = zipcode,
            Lastline = lastline,
            Addressee = addressee,
            Urbanization = urbanization,
            MaxCandidates = maxCandidates,
            MatchStrategy = matchStrategy,
        }, new StaticCredentials(id, token), url)
        {
        }

        private USStreetRequest(Lookup lookup, ICredentials credentials, string url) : base(new ClientBuilder(credentials).WithCustomBaseUrl(url).BuildUsStreetApiClient(), lookup)
        {
        }

        protected override EnumerableResponse<Candidate> Response()
        {
            return new USStreetResponse(Lookup.Result);
        }

        protected override EnumerableResponse<Candidate> Response(int status)
        {
            return new USStreetResponse(status);
        }
    }
}