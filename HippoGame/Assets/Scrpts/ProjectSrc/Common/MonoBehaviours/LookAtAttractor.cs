using UnityEngine;

namespace Scripts.ProjectSrc
{
    public class LookAtAttractor : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody;

        [Space(15)]
        [SerializeField]
        private Vector3 _gravityVector = new Vector3(0,-1,0);

        private Vector3 _gravityVectorNormalized ;

        private void OnEnable()
        {
            _gravityVectorNormalized = _gravityVector.normalized;
        }

        private void FixedUpdate()
        {
            if (_rigidbody == null) return;

            _rigidbody.MoveRotation(Quaternion.FromToRotation(_rigidbody.transform.up, -_gravityVectorNormalized) * _rigidbody.rotation);

        }
    }
}
