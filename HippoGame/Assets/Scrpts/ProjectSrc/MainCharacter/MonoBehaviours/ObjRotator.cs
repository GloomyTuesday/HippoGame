using Scripts.BaseSystems.Core;
using Scripts.InputSystem;
using UnityEngine;

namespace Scripts.ProjectSrc.MainCharacter
{
    public class ObjRotator : MonoBehaviour
    {
        [SerializeField]
        private float _rotationSpeedDesktop = 1f;

        [Header("Upvector axis: Y")]
        [SerializeField]
        private Transform _upVectorSource;

        [SerializeField]
        private Transform _rotatableAxisX;

        [SerializeField]
        private Transform _rotatableAxisY;

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IInputEventsCallbackIHandler))]
        private Object _inputEventsCallbackIHandlerObj;

        private IInputEventsCallbackIHandler _IInputEventsCallbackIHandler;
        private IInputEventsCallbackIHandler IInputEventsCallbackIHandler => _IInputEventsCallbackIHandler;

        private bool _isReady; 

        private void OnEnable()
        {
            if (!_isReady)
                Init();

            Subscribe();

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable()
        {
            Unsubscribe();
            Cursor.lockState = CursorLockMode.None;
        }

        private void Init()
        {
            if (_upVectorSource == null)
                Debug.LogError("\t _upVectorSource is NULL! ");

            _IInputEventsCallbackIHandler = _inputEventsCallbackIHandlerObj.GetComponent<IInputEventsCallbackIHandler>();
            _isReady = true; 
        }

        private void Subscribe()
        {
            IInputEventsCallbackIHandler.OnMouseDelta += MouseDelta;
        }

        private void Unsubscribe()
        {
            IInputEventsCallbackIHandler.OnMouseDelta -= MouseDelta;
        }

        private void MouseDelta(Vector2 vector)
        {
            RotateDesktop(vector);
        }

        private void RotateDesktop(Vector2 mouseDelta)
        {
            _rotatableAxisY.Rotate(_upVectorSource.up, mouseDelta.x * _rotationSpeedDesktop, Space.World);
            _rotatableAxisX.Rotate(_rotatableAxisY.right, -mouseDelta.y * _rotationSpeedDesktop, Space.World);
        }
    }
}
