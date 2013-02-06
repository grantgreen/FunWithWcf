using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.Serialization;

namespace FunWithWcf.Contracts.Utils
{
    public class SynchronizedTable : IEnumerable
    {
        private readonly object locker = new object();
        private readonly Hashtable table = new Hashtable();

        public void Add(object key, object value)
        {
            lock (locker)
            {
                this.table.Add(key, value); 
            }
        }

        public void Remove(object key)
        {
            lock (locker)
            {
                this.table.Remove(key); 
            }
        }

        public SynchronizedTable()
        {
            var hs = new Hashtable();
            
        }
        public bool Contains(object key)
        {
            return this.table.Contains(key);
        }

        public int Count
        {
            get { return this.table.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.table.IsReadOnly; }
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class SynchronizedList<T> : IList<T>
    {
        private readonly IList<T> cache = new List<T>();
        private readonly object locker = new object();

        public int IndexOf(T item)
        {
            return this.cache.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            lock (locker)
            {
                this.cache.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (locker)
            {
                this.cache.RemoveAt(index);
            }
        }

        public T this[int index]
        {
            get
            {
                return this.cache[index];
            }
            set
            {
                this.cache[index] = value;
            }
        }

        public void Add(T item)
        {
            Console.WriteLine("Add item: " + item.ToString());
            lock (locker)
            {
                Console.WriteLine("Item: " + item.ToString() + " added");
                this.cache.Add(item);
            }
        }

        public void Clear()
        {
            lock (locker)
            {
                this.cache.Clear();
            }
        }

        public bool Contains(T item)
        {
            return this.cache.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.cache.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.cache.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.cache.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            lock (locker)
            {
                return this.cache.Remove(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnumerator<T>(this, locker);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    /// <summary>
    /// /// The enumerator does not rely on the dispose mechanism, so it will lock when MoveNext is called the first time. When it reaches the end, it will release the lock.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListEnumerator<T> : IEnumerator<T>
    {
        private readonly SynchronizedList<T> cache;
        private readonly object lockerObject;
        private int position = -1;
        private bool lockTaken = false;

        public ListEnumerator(SynchronizedList<T> cache, object lockerObject)
        {
            this.cache = cache;
            this.lockerObject = lockerObject;
        }
        public T Current
        {
            get
            {
                return this.cache[position];
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
            if (++this.position >= this.cache.Count)
            {
                ReleaseLock();
                return false;
            }
            return true;
        }

        public void Reset()
        {
            this.position = -1;
            TakeLock();
        }

        private void TakeLock()
        {
            Monitor.Enter(lockerObject);
            this.lockTaken = true;
        }
        private void ReleaseLock()
        {
            if (this.lockTaken)
            {
                Monitor.Exit(lockerObject);
                this.lockTaken = false;
            }
        }
    }
}
