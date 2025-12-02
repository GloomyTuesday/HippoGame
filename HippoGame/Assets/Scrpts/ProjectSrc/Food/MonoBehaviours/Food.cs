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

        [SerializeField]
        [FilterByType(typeof(IBankTypeId<int, Collider>))]
        private Object _hippoColiderBankObj;

        private Rigidbody _rigidbody;

        private IBankTypeId<int, Transform> _foodActiveObjBank;
        private IBankTypeId<int, Transform> FoodActiveObjBank => _foodActiveObjBank;

        private IBankTypeId<int, Collider> _hippoColliderBank;
        private IBankTypeId<int, Collider> HippoColliderBank => _hippoColliderBank;

        private int _instanceId;

        public Rigidbody Rigidbody { get=> _rigidbody; private set => _rigidbody = value; }

        private bool _charge;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _instanceId = transform.GetInstanceID();
            _foodActiveObjBank = _foodActiveObjBankObj.GetComponent<IBankTypeId<int, Transform>>();
            _hippoColliderBank = _hippoColiderBankObj.GetComponent<IBankTypeId<int, Collider>>(); 
        }

        private void OnEnable()
        {
            FoodActiveObjBank.AddItem(transform, _instanceId);
        }

        private void OnDisable()
        {
            FoodActiveObjBank.RemoveItem(_instanceId); 
        }

        public void Charge() => _charge = true;

        private void OnCollisionEnter(Collision collision)
        {
            if (!_charge) return;
            if (HippoColliderBank.KeysHashSet.Contains(collision.collider.GetInstanceID())) return;

            Destroy(); 
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
