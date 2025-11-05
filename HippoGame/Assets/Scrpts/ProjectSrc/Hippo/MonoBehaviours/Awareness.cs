using Cysharp.Threading.Tasks;
using Scripts.ProjectSrc.MainCharacter;
using System.Threading;
using UnityEngine;

namespace Scripts.ProjectSrc.Hippo
{
    public class Awareness : MonoBehaviour
    {
        [SerializeField]
        private float _awarenessDistance = 1;

        [SerializeField]
        private float _groundedRayLength = 1;

        [SerializeField]
        private float _rotationSpeed = 1;

        [Space(15)]
        [SerializeField]
        private TargetBankSrc _targetBankSrc;

        [Space(15)]
        [Header("For grounded check")]
        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private Rigidbody _rigidbody;

        [SerializeField]
        private float _jumpForce;

        private CancellationTokenSource _cancellationTokenSource;
        private bool _isFallowingTarget;

        private void OnValidate()
        {
            if (_awarenessDistance < 0)
                _awarenessDistance = 0;

            if (_groundedRayLength < 0)
                _groundedRayLength = 0;

            if (_rotationSpeed < 0)
                _rotationSpeed = 0;
        }

        private void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            LaunchAwareness(token).Forget();
        }

        private void OnDisable()
        {
            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();
        }

        private async UniTaskVoid LaunchAwareness(CancellationToken token)
        {
            float distance;

            while (!token.IsCancellationRequested)
            {
                if (_targetBankSrc.TargetTransform != null && _targetBankSrc.IsFoodGrabbed)
                {
                    distance = Vector3.Distance(transform.position, _targetBankSrc.TargetTransform.position);

                    if (distance < _awarenessDistance)
                    {
                        if (!_isFallowingTarget)
                        {
                            _isFallowingTarget = true;

                            Debug.Log(" Is grounded ?");

                            if (IsGrounded())
                            {
                                Debug.Log("Jump"); 
                                _rigidbody.AddForce(_rigidbody.transform.up * _jumpForce, ForceMode.Impulse);
                            }
                        }
                    }
                    else
                    {
                        _isFallowingTarget = false;
                    }
                }
                else
                {
                    _isFallowingTarget = false;
                }

                if(_isFallowingTarget)
                    RotateToTarget(_targetBankSrc.TargetTransform);

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);
            }
        }

        private void RotateToTarget(Transform target)
        {
            Vector3 direction = target.position - _rigidbody.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime));
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -transform.up , _groundedRayLength + 0.1f, _layerMask);
        }
    }
}
