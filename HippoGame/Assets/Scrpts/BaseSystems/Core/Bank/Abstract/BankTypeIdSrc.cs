using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    /// <summary>
    /// 
    /// T1 - Is an id type
    /// T2 - Item type itself
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class BankTypeIdSrc<T1, T2> : ScriptableObject, IBankTypeId<T1, T2>
    {
        [NonSerialized]
        protected bool _ready;

        [NonSerialized]
        protected int _updateId=0;

        public int UpdateId
        { 
            get => _updateId;
            protected set => _updateId = value;
        }

        protected List<T2> _itemList;
        public List<T2> ItemList
        {
            get
            {
                if (!_ready)
                    Init();

                return _itemList;
            }
        }

        protected HashSet<T2> _itemHashSet;
        public HashSet<T2> ItemHashSet
        {
            get
            {
                if (!_ready)
                    Init();

                return _itemHashSet;
            }
        }

        protected HashSet<T1> _keysHashSet;
        public HashSet<T1> KeysHashSet
        {
            get
            {
                if (!_ready)
                    Init();

                return _keysHashSet;
            }
        }

        protected Dictionary<T1, T2> _itemDictionary;
        public Dictionary<T1, T2> ItemDictionary
        {
            get
            {
                if (!_ready)
                    Init();

                return _itemDictionary;
            }
        }

        public event Action<T1> OnItemAdded;
        public event Action<T1> OnItemRemoved;

        protected virtual void Init()
        {
            _itemDictionary = new Dictionary<T1, T2>();
            _itemList = new List<T2>();
            _itemHashSet = new HashSet<T2>();
            _keysHashSet = new HashSet<T1>();

            _ready = true; 
        }

        public virtual void Clear()
        {
            var idList = new List<T1>(KeysHashSet);

            for(int i = idList.Count-1; i >= 0; i--)
                RemoveItem(idList[i]);

            Init();
        }

        protected virtual void AddWithoutEventInvoke(T2 newItem, T1 itemId)
        {
            ItemDictionary.Add(itemId, newItem);
            ItemList.Add(newItem);
            ItemHashSet.Add(newItem);
            KeysHashSet.Add(itemId);

            UpateTheUpdateId();
        }

        public virtual void AddItem(T2 newItem, T1 itemId)
        {
            if (ItemDictionary.ContainsKey(itemId)) return;
            AddWithoutEventInvoke(newItem, itemId);
            OnItemAdded?.Invoke(itemId);
        }

        public T2 GetItem(T1 index)
        {
            if (!ItemDictionary.ContainsKey(index)) return default; 

            return ItemDictionary[index];
        }

        public bool ContainsKey(T1 key) => KeysHashSet.Contains(key); 

        public T2[] GetItemArray() => ItemList.ToArray();

        protected virtual void RemoveWithoutEventInvoke(T1 itemId)
        {
            var item = ItemDictionary[itemId];
            ItemDictionary.Remove(itemId);
            ItemList.Remove(item);

            ItemHashSet.Remove(item);
            KeysHashSet.Remove(itemId);
            UpateTheUpdateId();
        }

        public virtual void RemoveItem(T1 itemId)
        {
            if (!ItemDictionary.ContainsKey(itemId)) return;

            RemoveWithoutEventInvoke(itemId);
            OnItemRemoved?.Invoke(itemId);
        }

        protected virtual void UpateTheUpdateId()
        {
            var updateId = UpdateId++;
            if (updateId < 0)
                updateId = 0;

            UpdateId = updateId; 
        }
    }
}
