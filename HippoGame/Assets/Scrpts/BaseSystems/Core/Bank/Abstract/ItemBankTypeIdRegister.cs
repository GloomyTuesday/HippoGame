using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public abstract class ItemBankTypeIdRegister<T,T2>: MonoBehaviour
    {
        protected void Register(T key, T2 item, IBankTypeId<T, T2>[] iItemBank)
        {
            if (iItemBank == null) return;

            for (int i = 0; i < iItemBank.Length; i++)
                iItemBank[i].AddItem(item, key);
        }

        protected void Register(T[] key, T2 item, IBankTypeId<T, T2>[] iItemBank)
        {
            if (iItemBank == null) return;

            for (int i = 0; i < key.Length; i ++)
            {
                for (int j = 0; j < iItemBank.Length; j++)
                {
                    iItemBank[j].AddItem(item, key[i]);
                }
            }
        }

        protected void Unregister(T key, IBankTypeId<T, T2>[] iItemBank)
        {
            if (iItemBank == null) return;

            for (int i = 0; i < iItemBank.Length; i++)
                iItemBank[i].RemoveItem(key);
        }

        protected void Unregister(T[] key, IBankTypeId<T, T2>[] iItemBank)
        {
            if (iItemBank == null) return;

            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < iItemBank.Length; j++)
                {
                    iItemBank[j].RemoveItem(key[i]);
                }
            }
        }

        protected IBankTypeId<T, T2>[] GetBankArray(Object[] objArray)
        {
            var list = new List<IBankTypeId<T, T2>>();
            
            for (int i = 0; i < objArray.Length; i++)
            {
                var bankObj = objArray[i].GetComponent<IBankTypeId<T, T2>>();

                if (bankObj == null) continue; 

                list.Add(bankObj); 
            }

            return list.ToArray(); 
        }
    }
}
