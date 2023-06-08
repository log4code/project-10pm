namespace Project10pm.API.DataIngest
{
    public class TextContentRepo
    {
        private readonly List<string?> _textContent = new List<string?>();
        private readonly object _lock = new object();

        public int Add(string content)
        {
            ArgumentNullException.ThrowIfNull(content);
            if(string.IsNullOrEmpty(content) 
                || string.IsNullOrWhiteSpace(content)) 
            {
                throw new ArgumentException("content is required.");
            }

            var id = 0;
            lock (_lock)
            {
                _textContent.Add(content);
                id = _textContent.Count;
            }
            return id;
        }

        /// <summary>
        /// Retrieve a record with a known id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Find(int id)
        {
            if(id < 0 ||  id > _textContent.Count)
            {
                throw new Exception("Invalid record id");
            }
            var record =  _textContent[id -1];
            if(record == null)
            {
                throw new Exception("Invalid record id");
            }
            return record;
        }

        public object Remove(int id)
        {
            var record = Find(id);
            _textContent[id-1] = null;
            return record;
        }
    }
}
