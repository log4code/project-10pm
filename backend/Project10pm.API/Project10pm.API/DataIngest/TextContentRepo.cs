using System.Linq;

namespace Project10pm.API.DataIngest
{
    public class TextContentRepo
    {
        private readonly Dictionary<int, string?> _textContent = new Dictionary<int, string?>();
        private readonly object _lock = new object();
        private int _nextIdentity = 1;

        public int Add(string content)
        {
            ArgumentNullException.ThrowIfNull(content);
            if(string.IsNullOrEmpty(content) 
                || string.IsNullOrWhiteSpace(content)) 
            {
                throw new ArgumentException("content is required.");
            }

            var id = _nextIdentity;
            lock (_lock)
            {
                _textContent.Add(_nextIdentity, content);
                _nextIdentity = _nextIdentity + 1;
            }
            return id;
        }

        /// <summary>
        /// Retrieve all specified records
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Dictionary<int, string?> Get(int page, int pageSize)
        {
            var skipCount = (pageSize * (page-1));
            var records = _textContent.Skip(skipCount).Take(pageSize).ToDictionary(i => i.Key, i => i.Value);
            return records;
        }

        /// <summary>
        /// Retrieve a record with a known id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Find(int id)
        {
            if(false == _textContent.ContainsKey(id))
            {
                throw new Exception("Invalid record id");
            }
            var record =  _textContent[id];
            if(record == null)
            {
                throw new Exception("Invalid record id");
            }
            return record;
        }

        public object Remove(int id)
        {
            var record = Find(id);
            _textContent[id] = null;
            return record;
        }
    }
}
