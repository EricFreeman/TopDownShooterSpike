using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TDS.Futures.Object
{
    public class Pool<TItem> : IEnumerable<TItem> where TItem : class
    {
        private readonly LinkedList<TItem> _itemPool;
        private int _count = 0;

        public Pool()
        {
            _itemPool = new LinkedList<TItem>();
        }

        public void Put(TItem item)
        {
            lock (_itemPool)
            {
                _count++;
                _itemPool.AddLast(item);
            }
        }

        public TItem Get()
        {
            TItem item;
            lock (_itemPool)
            {
                item = _itemPool.First();
            }

            if (item != null)
            {
                _itemPool.RemoveFirst();
                _count--;
            }

            return item;
        }

        public bool Any()
        {
            return _count > 0;
        }

        public void Clear()
        {
            _count = 0;
            _itemPool.Clear();
        }

        public int Count
        {
            get { return _count; }
        } 

        public IEnumerator<TItem> GetEnumerator()
        {
            return _itemPool.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}