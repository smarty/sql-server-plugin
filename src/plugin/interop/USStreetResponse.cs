namespace SmartySqlServerPlugin.Interop
{
    using System.Collections.Generic;
    using SmartyStreets.USStreetApi;

    public class USStreetResponse : EnumerableResponse<Candidate>
    {
        public USStreetResponse(IList<Candidate> candidates) : base(candidates)
        {
        }

        public USStreetResponse(int status) : base(status)
        {
        }
    }
}