namespace SqlServerInterop
{
    using System;
    using System.Collections;
    using SmartyStreets;

    public abstract class EnumerableRequest<TLookup, TCandidate> : IEnumerable
        where TCandidate : class, new()
    {
        private readonly IClient<TLookup> _client;
        protected readonly TLookup Lookup;

        protected EnumerableRequest(IClient<TLookup> client, TLookup lookup)
        {
            _client = client;
            Lookup = lookup;
        }

        public IEnumerator GetEnumerator()
        {
            try
            {
                _client.Send(Lookup);
                return Response();
            }
            catch (Exception ex)
            {
                var status = ex.ParseStatus();
                if (status == 0)
                    throw;

                return Response(status);
            }
        }

        protected abstract EnumerableResponse<TCandidate> Response();
        protected abstract EnumerableResponse<TCandidate> Response(int status);
    }
}