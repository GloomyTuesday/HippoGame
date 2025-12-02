using Scripts.BaseSystems.Core;
using UnityEngine;

namespace Scripts.ProjectSrc.Food
{
    public class FoodTransformHolder : MonoBehaviour
    {
        [SerializeField]
        [FilterByType(typeof(IBankTypeId<int, Transform>))]
        private Object _foodTransformHolderBankObj;

        private IBankTypeId<int, Transform> _foodTransformHolderBank;
        private IBankTypeId<int, Transform> FoodTransformHolderBank => _foodTransformHolderBank;

        private int _instanceId; 

        private void Awake()
        {
            _foodTransformHolderBank = _foodTransformHolderBankObj.GetComponent<IBankTypeId<int, Transform>>();
            _instanceId = transform.GetInstanceID();
        }

        private void OnEnable()
        {
            FoodTransformHolderBank.AddItem(transform, _instanceId); 
        }

        private void OnDisable()
        {
            FoodTransformHolderBank.RemoveItem(_instanceId); 
        }
    }
}
