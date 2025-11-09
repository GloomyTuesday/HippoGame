using Cysharp.Threading.Tasks;
using Scripts.BaseSystems.Core;
using Scripts.InputSystem;
using Scripts.ProjectSrc.Common;
using Scripts.ProjectSrc.Food;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Scripts.ProjectSrc.MainCharacter
{
    public class Grabber : MonoBehaviour
    {
        [SerializeField]
        [Uneditable]
        private Rigidbody _grabbedRigidbody = null;
        private Rigidbody GrabbedRigidbody
        {
            get => _grabbedRigidbody;
            set
            {
                var previousValue = _grabbedRigidbody;
                _grabbedRigidbody = value;

                if(previousValue != _grabbedRigidbody && previousValue != null)
                {
                    previousValue.isKinematic = _isGrabbableKinematic;
                }

                if (value != null)
                {
                    _grabbedRigidbody.linearVelocity = Vector3.zero;
                    _grabbedRigidbody.angularVelocity = Vector3.zero;

                    _isGrabbableKinematic = _grabbedRigidbody.isKinematic;
                    _grabbedRigidbody.isKinematic = true;

                    _targetBankObj.IsFoodGrabbed = true;
                }

                if(value== null)
                {
                    _targetBankObj.IsFoodGrabbed = false;
                }
            }
        }

        private bool _isGrabbableKinematic;

        [Space(15)]
        [SerializeField]
        private float _grabDistance;

        [SerializeField]
        private float _throwForce;

        [Space(15)]
        [SerializeField]
        private Vector3 _grabPoint;

        [SerializeField]
        private Transform _raycastSource;

        [SerializeField]
        private LayerMask _layerMask;

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IInputEventsCallbackIHandler))]
        private Object _inputEventsCallbackIHandlerObj;

        [SerializeField]
        private TargetBankSrc _targetBankObj;

        private IInputEventsCallbackIHandler _IInputEventsCallbackIHandler;
        private IInputEventsCallbackIHandler IInputEventsCallbackIHandler => _IInputEventsCallbackIHandler;

        [Space(15)]
        [Header("Game objects with colliders to ignore")]
        [SerializeField]
        private GameObject[] _gameObjWithCollidersToIgnore;

        [Space(15)]
        [SerializeField]
        [Uneditable]
        private Collider[] _collidersToignore;

        private CancellationTokenSource _cancellationTokenSource; 

        private void OnValidate()
        {
            if (_grabDistance < 0)
                _grabDistance = 0;

            var colliderList = new List<Collider>(); 

            if(_gameObjWithCollidersToIgnore!=null)
            {
                for (int i = 0; i < _gameObjWithCollidersToIgnore.Length; i++)
                {
                    colliderList.AddRange(_gameObjWithCollidersToIgnore[i].GetComponents<Collider>());
                }
            }
            
            _collidersToignore = colliderList.ToArray();
        }

        private void Awake()
        {
            _IInputEventsCallbackIHandler = _inputEventsCallbackIHandlerObj.GetComponent<IInputEventsCallbackIHandler>();
            
        }

        private void OnEnable()
        {
            _targetBankObj.TargetTransform = transform; 
            Subscribe(); 
        }

        private void OnDisable()
        {
            _targetBankObj.TargetTransform = null;
            _targetBankObj.IsFoodGrabbed = false; 
            Unsubscribe(); 
        }

        private void Subscribe()
        {
            IInputEventsCallbackIHandler.OnPointerDown += PointerDown;
            IInputEventsCallbackIHandler.OnPointerUp += PointerUp;

            IInputEventsCallbackIHandler.OnMouseRightBtnUp += MouseRightBtnUp;
        }

        private void Unsubscribe()
        {
            IInputEventsCallbackIHandler.OnPointerDown -= PointerDown;
            IInputEventsCallbackIHandler.OnPointerUp -= PointerUp;

            IInputEventsCallbackIHandler.OnMouseRightBtnUp -= MouseRightBtnUp;
        }

        private void PointerDown(Vector2 vector)
        {
            var raycastResult = RayCast();

            if (!raycastResult.isHit) return;

            GrabbedRigidbody = raycastResult.grabbable.Rigidbody;
            
            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();

            _targetBankObj.IsFoodGrabbed = true; 
           _cancellationTokenSource = new CancellationTokenSource(); 
            var token = _cancellationTokenSource.Token;
            UpdateGrabbedObjPosition(token).Forget(); 

        }

        private void PointerUp(Vector2 vector)
        {
            if (GrabbedRigidbody == null) return;
            _cancellationTokenSource.Cancel();

            GrabbedRigidbody = null; 
        }

        private void MouseRightBtnUp(Vector2 vector)
        {
            if (GrabbedRigidbody == null) return;
            _cancellationTokenSource.Cancel();

            GrabbedRigidbody.isKinematic = false;

            GrabbedRigidbody.AddForce(_raycastSource.forward * _throwForce, ForceMode.Impulse);

            var eatable = GrabbedRigidbody.GetComponent<IEatable>();
            eatable.Charge(); 
            _targetBankObj.ThrowFood(eatable);

            GrabbedRigidbody = null;
        }

        private  (bool isHit, IGrabbable grabbable) RayCast()
        {
            Vector3 origin = _raycastSource.position;
            Vector3 direction = _raycastSource.forward;

            Ray ray = new Ray(origin, direction);

            RaycastHit[] hits = Physics.RaycastAll(ray, _grabDistance, _layerMask, QueryTriggerInteraction.Ignore);

            bool isValid ;

            int count = 0; 

            foreach (var item in hits)
            {
                isValid = true;
                for ( int i = 0; i < _collidersToignore.Length; i ++)
                {
                    if (item.collider == _collidersToignore[i])
                    {
                        isValid = false;
                        break; 
                    }
                }

                if (isValid)
                {
                    var grabbable = item.transform.GetComponent<IGrabbable>();

                    if(grabbable!=null)
                        return (true, grabbable);
                }
                
                count++; 
            }

            return (false, default);
        }

        private async UniTaskVoid UpdateGrabbedObjPosition(CancellationToken token)
        {
            Vector3 grabPointPosition; 

            while (!token.IsCancellationRequested)
            {
                grabPointPosition = _raycastSource.position + _raycastSource.forward + _grabPoint;
                GrabbedRigidbody.MovePosition(grabPointPosition);

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);
            }
        }

        private void OnDrawGizmos()
        {
            if (_raycastSource == null) return;

            Gizmos.color = Color.red;
            var origin = _raycastSource.position;
            var end = origin + _raycastSource.forward * _grabDistance;

            Gizmos.DrawLine(origin, end);
            Gizmos.DrawSphere(end, 0.05f);

            var grabPointPosition = origin + _raycastSource.forward + _grabPoint;
            Gizmos.DrawWireSphere(grabPointPosition, .05f);
        }
    }
}
