using System;
using System.Collections;
using System.Collections.Generic;

namespace kobk.csharp.collections
{
    public class DualDictionary<TKey, TValue>
    {

        private Dictionary<TKey, TValue> forward;
        private Dictionary<TValue, TKey> reverse;
        public int Count
        {
            private set;
            get;
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                return forward.Keys;
            }
        }

        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                return forward.Values;
            }
        }

        public DualDictionary()
        {
            forward = new Dictionary<TKey, TValue>();
            reverse = new Dictionary<TValue, TKey>();
        }

        public void Add(TKey key, TValue val)
        {
            //if (key is null && val is null)
            if (ReferenceEquals(key, null) && ReferenceEquals(val, null))
                {
                throw new ArgumentNullException("key & val", "Key or value cannot be null!");
            }
            //else if (key is null)
            else if (ReferenceEquals(key, null))
            {
                throw new ArgumentNullException("key", "Key cannot be null!");
            }
            //else if (val is null)
            else if (ReferenceEquals(val, null))
            {
                throw new ArgumentNullException("val", "value cannot be null!");
            }

            if (forward.ContainsKey(key) || reverse.ContainsKey(val))
                throw new ArgumentException("Key or value already exists in the Dictionary");

            forward.Add(key, val);
            reverse.Add(val, key);
            Count++;
        }

        public void Clear()
        {
            Count = 0;
            forward.Clear();
            reverse.Clear();
        }

        public bool containsKey(TKey key)
        {
            //if (key is null)
            if(ReferenceEquals(key, null))
                throw new ArgumentNullException("key", "Key cannot be null");
            return forward.ContainsKey(key);
        }

        public bool containsVal(TValue val)
        {
            //if (val is null)
            if(ReferenceEquals(val, null))
                throw new ArgumentNullException("val", "Val cannot be null");
            return reverse.ContainsKey(val);
        }

        public bool Remove(TKey key)
        {
            //if (key is null)
            if(ReferenceEquals(key, null))
                throw new ArgumentNullException("key", "Key cannot be null");

            if (forward.ContainsKey(key))
            {
                TValue val = forward[key];
                forward.Remove(key);
                reverse.Remove(val);
                return true;
            }
            return false;
        }

        public bool Remove(TValue val)
        {
            //if (val is null)
            if(ReferenceEquals(val, null))
                throw new ArgumentNullException("val", "Key cannot be null");

            if (reverse.ContainsKey(val))
            {
                TKey key = reverse[val];
                forward.Remove(key);
                reverse.Remove(val);
                return true;
            }
            return false;
        }

        public bool Remove(TKey key, out TValue oval)
        {
            if (TryToGetValue(key, out TValue val))
            {
                //TValue val = forward[key];
                forward.Remove(key);
                reverse.Remove(val);
                oval = val;
                return true;
            }
            oval = default(TValue);
            return false;
        }

        public bool Remove(TValue val, out TKey okey)
        {
            if (TryToGetValue(val, out TKey key))
            {
                //TKey key = reverse[val];
                forward.Remove(key);
                reverse.Remove(val);
                okey = key;
                return true;
            }
            okey = default(TKey);
            return false;
        }

        public bool TryAdd(TKey key, TValue val)
        {
            //if (key is null || val is null)
            if(ReferenceEquals(key, null) || ReferenceEquals(val, null))
            {
                return false;
            }

            if (forward.ContainsKey(key) || reverse.ContainsKey(val))
                return false;
            
            forward.Add(key, val);
            reverse.Add(val, key);
            return true;
        }

        public bool TryToGetValue(TKey key, out TValue oval) {
            //if (key is null)
            if(ReferenceEquals(key, null))
                throw new ArgumentNullException("key", "Key cannot be null");

            if (forward.ContainsKey(key))
            {
                oval = forward[key];
                return true;
            }
            oval = default(TValue);
            return false;
        }

        public bool TryToGetValue(TValue val, out TKey okey) {
            //if (val is null)
            if(ReferenceEquals(val, null))
                throw new ArgumentNullException("key", "Key cannot be null");

            if (reverse.ContainsKey(val))
            {
                okey = reverse[val];
                return true;
            }
            okey = default(TKey);
            return false;
        }

    }
}