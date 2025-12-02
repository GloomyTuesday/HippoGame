using Cysharp.Threading.Tasks;
using Scripts.BaseSystems.Core;
using Scripts.ProjectSrc.MainCharacter;
using System.Threading;
using UnityEngine;

namespace Scripts.ProjectSrc.Hippo
{
    public class Awareness : MonoBehaviour
    {
        [Space(15)]
        [SerializeField]
        private float _groundedRayLength = 1;
        [SerializeField]
        private Vector3 _groundedRayOriginOffset = Vector3.zero;

        [Space(15)]
        [SerializeField]
        private float _rotationSpeed = 1;

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(TargetBankSrc))]
        private TargetBankSrc _targetBankSrc;

        [Space(15)]
        [Header("For grounded check")]
        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private Rigidbody _rigidbody;

        [SerializeField]
        private float _jumpForce;

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IHippoStateEventsInvoker))]
        private Object _hippoStateEventsInvokerObj;

        [SerializeField]
        [FilterByType(typeof(IHippoStateEventsCallbackHandler))]
        private Object _hippoStateEventsCallbackHandlerObj;

        [SerializeField]
        [FilterByType(typeof(IBankTypeId<int, Collider>))]
        private Object _hippoColliderBankObj;

        [SerializeField]
        [FilterByType(typeof(AwarenessDistanceSrc))]
        private AwarenessDistanceSrc _awarenessDistanceSrc;

        private bool _isWatchingTheFood = false;
        private TriggerIds _currentTriggerId = TriggerIds.Non;

        private CancellationTokenSource _cancellationTokenSource;

        private IHippoStateEventsInvoker _iHippoStateEventsInvoker;
        private IHippoStateEventsInvoker IHippoStateEventsInvoker => _iHippoStateEventsInvoker;

        private IHippoStateEventsCallbackHandler _iHippoStateEventsCallbackHandler;
        private IHippoStateEventsCallbackHandler IHippoStateEventsCallbackHandler => _iHippoStateEventsCallbackHandler;

        private IBankTypeId<int, Collider> _hippoColloiderBank;
        private IBankTypeId<int, Collider> HippoColloiderBank => _hippoColloiderBank;

        private void OnValidate()
        {
            if (_groundedRayLength < 0)
                _groundedRayLength = 0;

            if (_rotationSpeed < 0)
                _rotationSpeed = 0;
        }

        private void Awake()
        {
            _iHippoStateEventsInvoker = _hippoStateEventsInvokerObj.GetComponent<IHippoStateEventsInvoker>();
            _iHippoStateEventsCallbackHandler = _hippoStateEventsCallbackHandlerObj.GetComponent<IHippoStateEventsCallbackHandler>();
            _hippoColloiderBank = _hippoColliderBankObj.GetComponent<IBankTypeId<int, Collider>>();
        }

        private void OnEnable()
        {
            _currentTriggerId = TriggerIds.Non;
            _isWatchingTheFood = false;

            Subscribe();

            var currentTrigger = IHippoStateEventsInvoker.GetCurrentTrigger();

            NewTriggerApplyed(currentTrigger);

        }

        private void OnDisable()
        {
            TryToStop();
            Unsubscribe();
        }

        private void Subscribe()
        {
            IHippoStateEventsCallbackHandler.OnNewTriggerApplyed += NewTriggerApplyed;
        }

        private void Unsubscribe()
        {
            IHippoStateEventsCallbackHandler.OnNewTriggerApplyed -= NewTriggerApplyed;
        }

        private void NewTriggerApplyed(TriggerIds triggerId)
        {
            var previousTriggerd = _currentTriggerId;
            _currentTriggerId = triggerId;

            if (
                 (previousTriggerd != TriggerIds.Idle_OpenMouth) &&
                 (triggerId == TriggerIds.Idle_OpenMouth)
                )
                {
                    if (_cancellationTokenSource != null)
                        _cancellationTokenSource.Cancel();

                    _cancellationTokenSource = new CancellationTokenSource();
                    var token = _cancellationTokenSource.Token;
                    AwarenessProcess(token).Forget();
                return;
                }

            if (
               (previousTriggerd != TriggerIds.Idle) &&
               (triggerId == TriggerIds.Idle)
              )
            {
                if (_cancellationTokenSource != null)
                    _cancellationTokenSource.Cancel();

                _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;
                AwarenessProcess(token).Forget();
                return; 
            }

            if (
               (previousTriggerd == TriggerIds.Idle) &&
               (triggerId == TriggerIds.Idle)
              )
            {
                return;
            }

            if (
               (previousTriggerd == TriggerIds.Idle_OpenMouth) &&
               (triggerId == TriggerIds.Idle_OpenMouth)
              )
            {
                return;
            }

            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();
        }

        private async UniTaskVoid AwarenessProcess(CancellationToken token)
        {
            float distance;

            while (!token.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);

                if (!_targetBankSrc.IsFoodGrabbed || _targetBankSrc.TargetTransform == null )
                {
                    if (!_isWatchingTheFood) continue; 

                    _isWatchingTheFood = false;
                    IHippoStateEventsInvoker.SetTrigger(TriggerIds.Idle);
                    continue;
                }

                distance = Vector3.Distance(transform.position, _targetBankSrc.TargetTransform.position);

                if (distance < _awarenessDistanceSrc.AwarnessDistance)
                {
                    if (!_isWatchingTheFood)
                    {
                        _isWatchingTheFood = true;

                        IHippoStateEventsInvoker.SetTrigger(TriggerIds.Idle_OpenMouth);

                        if (IsGrounded())
                        {
                            _rigidbody.AddForce(_rigidbody.transform.up * _jumpForce, ForceMode.Impulse);
                        }
                    }

                    RotateToTarget(_targetBankSrc.TargetTransform);
                    continue; 
                }


                if(!_isWatchingTheFood) continue;

                _isWatchingTheFood = false;
                IHippoStateEventsInvoker.SetTrigger(TriggerIds.Idle);
                
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
            var colliders = Physics.RaycastAll(transform.position + _groundedRayOriginOffset, -transform.up, _groundedRayLength + 0.1f, _layerMask);

            var hippoColliders = HippoColloiderBank.ItemHashSet;

            foreach (var item in colliders)
            {
                if (!hippoColliders.Contains(item.collider))
                    return true;
            }

            return false;
        }

        private void TryToStop()
        {
            if (_cancellationTokenSource == null) return;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            var origin = transform.position + _groundedRayOriginOffset;
            var end = origin + -transform.up * _groundedRayLength;

            Gizmos.DrawLine(origin, end);
            Gizmos.DrawSphere(end, 0.05f);
        }
    }
}
