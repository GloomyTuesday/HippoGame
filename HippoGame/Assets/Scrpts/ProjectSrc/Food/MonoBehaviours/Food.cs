using Scripts.BaseSystems.Core;
using Scripts.ProjectSrc.Common;
using UnityEngine;

namespace Scripts.ProjectSrc.Food
{
    public class Food : MonoBehaviour, IGrabbable, IEatable
    {
        [SerializeField]
        private GameObject _particlePrefab; 

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IBankTypeId<int, Transform>))]
        private Object _foodActiveObjBankObj;

        private IBankTypeId<int, Transform> _foodActiveObjBank;
        private IBankTypeId<int, Transform> FoodActiveObjBank => _foodActiveObjBank;

        private int _instanceId;

        private Rigidbody _rigidbody; 
        public Rigidbody Rigidbody { get=> _rigidbody; private set => _rigidbody = value; }


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _instanceId = transform.GetInstanceID();
            _foodActiveObjBank = _foodActiveObjBankObj.GetComponent<IBankTypeId<int, Transform>>();
        }

        private void OnEnable()
        {
            FoodActiveObjBank.AddItem(transform, _instanceId);
        }

        private void OnDisable()
        {
            FoodActiveObjBank.RemoveItem(_instanceId); 
        }

        private void OnCollisionEnter(Collision collision)
        {
            
        }

        public void Destroy()
        {
            var obj = Instantiate(_particlePrefab, transform.parent);
            obj.name = "Food particles";
            obj.transform.localPosition = transform.position;
            obj.transform.localRotation = transform.rotation;
            obj.transform.localScale = Vector3.one;

            Destroy(gameObject);
        }
    }
}
