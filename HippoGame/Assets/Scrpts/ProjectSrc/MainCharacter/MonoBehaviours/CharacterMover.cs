using Cysharp.Threading.Tasks;
using Scripts.BaseSystems.Core;
using Scripts.InputSystem;
using System.Threading;
using UnityEngine;

namespace Scripts.ProjectSrc.MainCharacter
{
    public class CharacterMover : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody;

        [Space(15)]
        [SerializeField]
        private float _moveSpeed = 1;

        [Space(15)]
        [SerializeField]
        private Transform _directionSourceTransform;

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IInputEventsCallbackIHandler))]
        private Object _inputEventsCallbackIHandlerObj;

        private IInputEventsCallbackIHandler _IInputEventsCallbackIHandler ;
        private IInputEventsCallbackIHandler IInputEventsCallbackIHandler => _IInputEventsCallbackIHandler;

        private bool _isReady;
        private CancellationTokenSource _cancellationTokenSourceMove;
        private Vector2 _moveDirectionInput;
        private bool _isMoveInProgress;

        private void OnValidate()
        {
            if (_moveSpeed < 0)
                _moveSpeed = 0; 
        }

        private void OnEnable()
        {
            if (!_isReady)
                Init();

            Subscribe();
        }

        private void OnDisable()
        {
            _isMoveInProgress = false; 

            if(_cancellationTokenSourceMove!=null)
                _cancellationTokenSourceMove.Cancel();

            Unsubscribe(); 
        }

        private void Init()
        {
            _IInputEventsCallbackIHandler = _inputEventsCallbackIHandlerObj.GetComponent<IInputEventsCallbackIHandler>();
            _isReady = true; 
        }

        private void Subscribe()
        {
            IInputEventsCallbackIHandler.OnWASD += WASD;
            IInputEventsCallbackIHandler.OnWASDCanceled += WASDCanceled;
        }

        private void Unsubscribe()
        {
            IInputEventsCallbackIHandler.OnWASD -= WASD;
            IInputEventsCallbackIHandler.OnWASDCanceled -= WASDCanceled;
        }

        private void WASDCanceled()
        {
            _isMoveInProgress = false;

            if (_cancellationTokenSourceMove != null)
            {
                _cancellationTokenSourceMove.Cancel();
            }
        }

        private void WASD(Vector2 vector)
        {
            if (!_rigidbody) return;

            _moveDirectionInput = vector;

            if (!_isMoveInProgress)
            {
                _cancellationTokenSourceMove = new CancellationTokenSource();
                var token = _cancellationTokenSourceMove.Token;

                Move(token).Forget();
            }
        }

        private async UniTaskVoid Move(CancellationToken token)
        {
            _isMoveInProgress = true;
            Vector3 moveDirection;
            Vector3 currentVelocity = Vector3.zero;

            while (!token.IsCancellationRequested)
            {
                moveDirection = (_directionSourceTransform.forward * _moveDirectionInput.y + _directionSourceTransform.right * _moveDirectionInput.x).normalized;

                Vector3 newPosition = _rigidbody.position + moveDirection * _moveSpeed * Time.fixedDeltaTime;

                _rigidbody.MovePosition(newPosition);

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);
            }
        }

        private void FixedUpdate()
        {
            if (_isMoveInProgress) return;
            if (_rigidbody.linearVelocity.magnitude < .01f) return;

            var rigidBodyLinearVelocity = _rigidbody.linearVelocity;
            var forceToApply = -rigidBodyLinearVelocity.normalized * _moveSpeed * Time.fixedDeltaTime;

            _rigidbody.AddForce(forceToApply, ForceMode.Force);
        }
    }
}
