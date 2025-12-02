using Scripts.BaseSystems.Core;
using Scripts.InputSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.ProjectSrc.Ui
{
    public class BtnEsc : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _event; 

        [SerializeField]
        [FilterByType(typeof(IInputEventsCallbackIHandler))]
        private Object _inputEventsCallbackIHandlerObj;

        private IInputEventsCallbackIHandler _IInputEventsCallbackIHandler;
        private IInputEventsCallbackIHandler IInputEventsCallbackIHandler => _IInputEventsCallbackIHandler;

        private void Awake()
        {
            _IInputEventsCallbackIHandler = _inputEventsCallbackIHandlerObj.GetComponent<IInputEventsCallbackIHandler>();
        }

        private void OnEnable()
        {
            Subscribe(); 
        }

        private void OnDisable()
        {
            Unsubscribe(); 
        }

        private void Subscribe()
        {
            IInputEventsCallbackIHandler.OnEscUp += EscUp;
        }

        private void Unsubscribe()
        {
            IInputEventsCallbackIHandler.OnEscUp -= EscUp;
        }

        private void EscUp()
        {
            _event?.Invoke(); 
        }
    }
}
