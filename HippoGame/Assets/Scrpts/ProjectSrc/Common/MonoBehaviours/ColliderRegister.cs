using Scripts.BaseSystems.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.ProjectSrc.Common
{
    public class ColliderRegister : MonoBehaviour
    {
        [SerializeField]
        [FilterByType(typeof(IBankTypeId<int, Collider>))]
        private Object[] _colliderBankObj;

        private IBankTypeId<int, Collider>[] _colliderBank;
        private IBankTypeId<int, Collider>[] ColliderBank => _colliderBank;

        [SerializeField]
        [Uneditable]
        private Collider[] _colliders; 

        private int[] _registeredCollider;

        private void OnValidate()
        {
            _colliders = GetComponents<Collider>();
        }

        private void Awake()
        {
            var bankList = new List<IBankTypeId<int, Collider>>();

            foreach (var item in _colliderBankObj)
            {
                var bankObj = item.GetComponent<IBankTypeId<int, Collider>>();

                if (bankObj != null)
                    bankList.Add(bankObj); 
            }

            _colliderBank = bankList.ToArray(); 
        }

        private void OnEnable()
        {
            if (_colliders == null) return;

            var registeredColliderInstanceIdList = new List<int>(); 

            foreach (var bank in ColliderBank)
            {
                foreach (var collider in _colliders)
                {
                    registeredColliderInstanceIdList.Add(collider.GetInstanceID());
                    bank.AddItem(collider, registeredColliderInstanceIdList[^1]); 
                }
            }

            _registeredCollider = registeredColliderInstanceIdList.ToArray(); 
        }
        
        private void OnDisable()
        {
            foreach (var bank in ColliderBank)
            {
                foreach (var colliderInstanceId in _registeredCollider)
                    bank.RemoveItem(colliderInstanceId);
            }

            _registeredCollider = null; 
        }
    }
}
