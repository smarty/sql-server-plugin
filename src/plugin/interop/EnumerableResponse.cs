namespace SmartySqlServerPlugin.Interop
{
    using System.Collections;
    using System.Collections.Generic;

    public class EnumerableResponse<T> : IEnumerator, IEnumerable
        where T : class, new()
    {
        private readonly IList<T> _candidates;
        private int _index = -1;

        public EnumerableResponse(IList<T> candidates)
        {
            _candidates = candidates ?? new T[] { };
        }

        public EnumerableResponse(int status) : this(null)
        {
            Status = status;
        }

        public object Current => this;

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public void Reset()
        {
            _index = -1;
        }

        public bool MoveNext()
        {
            _index++;
            return _index == 0 && _candidates.Count == 0 ? PopulateEmpty() : this.Populate();
        }

        private bool PopulateEmpty()
        {
            if (Status != Contracts.StatusOkay)
                Index = null;

            return true;
        }

        private bool Populate()
        {
            if (_index >= _candidates.Count)
                return false;

            Index = _index;
            Candidate = _candidates[_index];
            return true;
        }

        public int Status { get; } = Contracts.StatusOkay;
        public int? Index { get; private set; } = -1;
        public T Candidate { get; private set; } = new T();
    }
}