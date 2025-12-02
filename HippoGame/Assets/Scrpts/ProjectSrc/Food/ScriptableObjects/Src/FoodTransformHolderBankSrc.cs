using Scripts.BaseSystems.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.ProjectSrc.Food
{
    [CreateAssetMenu(fileName = "FoodTransformHolderBank", menuName = "Scriptable Obj/Project src/Food transform holder bank")]
    public class FoodTransformHolderBankSrc : BankTypeIdSrc<int, Transform>
    {
        //  In order to make thi bank to be a stockpile for only 1 item
        public override void AddItem(Transform newItem, int itemId)
        {
            if (ItemList.Count > 0)
            {
                var keyList = new List<int>(KeysHashSet);

                for (int i = keyList.Count; i >= 0; i--)
                {
                    RemoveItem(keyList[i]);
                }
            }

            base.AddItem(newItem, itemId);
        }
    }
}

