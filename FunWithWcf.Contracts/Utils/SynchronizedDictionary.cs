using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FunWithWcf.Contracts.Utils
{
    public class SynchronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> table = new Dictionary<TKey, TValue>();
        private readonly object locker = new object();

        public void Add(TKey key, TValue value)
        {
            var hs = new Hashtable();
            lock (locker)
            {
                this.table[key] = value;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return this.table.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return this.table.Keys; }
        }

        public bool Remove(TKey key)
        {
            lock (locker)
            {
                return this.table.Remove(key);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.table.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return this.table.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                return this.table[key];
            }
            set
            {
                lock (locker)
                {
                    this.table[key] = value;
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            lock (locker)
            {
                this.table[item.Key] = item.Value;
            }
        }

        public void Clear()
        {
            lock (locker)
            {
                this.table.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.table.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.table.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.table.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.table.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (locker)
            {
                return this.table.Remove(item);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new TableEnumerator<TKey, TValue>(this.table, this.locker);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// The enumerator does not rely on the dispose mechanism, so it will lock when MoveNext is called the first time. When it reaches the end, it will release the lock.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class TableEnumerator<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        private readonly IDictionary<TKey, TValue> table;
        private readonly object lockerObject;
        private int position = -1;
        private bool lockTaken = false;

        public TableEnumerator(IDictionary<TKey, TValue> table, object lockerObject)
        {
            this.table = table;
            this.lockerObject = lockerObject;
        }
        public KeyValuePair<TKey, TValue> Current
        {
            get
            {
                return this.table.ElementAt(position);
            }
        }

        public void Dispose()
        {
            ReleaseLock();
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            if (!this.lockTaken)
            {
                TakeLock();
            }

            if (++this.position >= this.table.Count)
            {
                ReleaseLock();
                return false;
            }
            return true;
        }

        public void Reset()
        {
            this.position = -1;
            ReleaseLock();
        }

        private void TakeLock()
        {
            Console.WriteLine(Thread.CurrentThread.Name + " is taking lock");
            Monitor.Enter(lockerObject);
            this.lockTaken = true;
        }
        private void ReleaseLock()
        {
            if (this.lockTaken)
            {
                Console.WriteLine(Thread.CurrentThread.Name + " is releasing lock");
                Monitor.Exit(lockerObject);
                this.lockTaken = false;
            }
        }
    }
}
