using System;
using System.Collections.Generic;

namespace Scripts.BaseSystems.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"> Id used by stockpiled type.</typeparam>
    /// <typeparam name="T2"> Type that is stockpiled.</typeparam>
    public interface IBankTypeId<T,T2>
    {
        /// <summary>
        ///     Changed after each Add or Remove item
        /// </summary>
        public int UpdateId { get; }
        public T2[] GetItemArray();
        public T2 GetItem(T index);
        public bool ContainsKey(T key);

        public HashSet<T2> ItemHashSet { get; }
        public HashSet<T> KeysHashSet { get; }
        public Dictionary<T, T2> ItemDictionary { get; }

        /// <summary>
        ///   The parameter is an id of the added object
        /// </summary>
        public event Action<T> OnItemAdded;

        /// <summary>
        /// The parameter is an removed object id.
        /// </summary>
        public event Action<T> OnItemRemoved;

        public void AddItem(T2 newItem, T itemId);
        public void RemoveItem(T itemId);

        public void Clear();
    }
}
