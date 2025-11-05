using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.InputSystem
{
    public class InputSystemObserver : MonoBehaviour
    {
        [SerializeField]
        private InputEventsSrc _inputEventsSrc;

        private InputActionsSrc _inputActionsSrc;

        /// <summary>
        ///     Contains information if input of certain type hit an rect transform constraint
        /// </summary>
        public Dictionary<InputTypeId, bool> RectTransformHitDictionary { get; set; } = new Dictionary<InputTypeId, bool>(); 

        private IInputEventsCallbackInvoker _IInputEventsCallbackInvoker;
        private IInputEventsCallbackInvoker IInputEventsCallbackInvoker
        {
            get
            {
                if (_IInputEventsCallbackInvoker == null)
                    _IInputEventsCallbackInvoker = _inputEventsSrc;

                return _IInputEventsCallbackInvoker;
            }
        }

        private IInputEventsHandler _IInputEventsHandler;
        private IInputEventsHandler IInputEventsHandler
        {
            get
            {
                if (_IInputEventsHandler == null)
                    _IInputEventsHandler = _inputEventsSrc;

                return _IInputEventsHandler;
            }
        }

        private bool IsPointerDown { get; set; }
        private bool IsMouseMiddleBtnDown { get; set; }
        private bool IsMouseRightBtnDown { get; set; } 
        private Vector2 MouseDeltaProp { get; set; }
        private Vector2 MousePreviousPosition { get; set; }
        private Vector2 MousePosition { get; set; }
        private Vector2 PointerPosition { get; set; }

        private void OnEnable()
        {
            _inputActionsSrc = new InputActionsSrc();

            _inputActionsSrc.Player.Enable();


            Subscribe();
        }

        private void OnDisable()
        {
            _inputActionsSrc.Player.Disable();

            Unsubscribe();
        }

        private void Subscribe()
        {
            //  ----------------------------------------    Pointer
            _inputActionsSrc.Player.PointerPosition.performed += PointerPositionPerformed;
            _inputActionsSrc.Player.PointerDown.performed += PointerDownPerformed;
            _inputActionsSrc.Player.PointerDown.canceled += PointerUpPerformed;

            //  ----------------------------------------    Mouse
            _inputActionsSrc.Player.MouseRightBtnDown.performed += MouseRightBtnDownPerformed;
            _inputActionsSrc.Player.MouseRightBtnDown.canceled += MouseRightBtnUpPerformed;

            _inputActionsSrc.Player.MouseDelta.performed += MouseDelta;

            //  ----------------------------------------    Mouse middle button
            _inputActionsSrc.Player.MouseMiddleBtnDown.started += MouseMiddleBtnDownPerformed;
            _inputActionsSrc.Player.MouseMiddleBtnDown.canceled += MouseMiddleBtnUpPerformed;

            //  ----------------------------------------    Mouse Y scroll
            _inputActionsSrc.Player.MouseScrollY.performed += MouseScrollYPerformed;
            _inputActionsSrc.Player.MouseScrollY.canceled += MouseScrollYCanceled;

            //  ----------------------------------------    Mouse
            _inputActionsSrc.Player.MouseMove.performed += MouseMovePerformed;

            //  ----------------------------------------    Keyboard
            _inputActionsSrc.Player.WASD.performed += Wasd;
            _inputActionsSrc.Player.WASD.canceled += WasdCanceled;

            _inputActionsSrc.Player.Esc.canceled += EscUp;
        }

        private void Unsubscribe()
        {
            //  ----------------------------------------    Pointer
            _inputActionsSrc.Player.PointerPosition.performed -= PointerPositionPerformed;
            _inputActionsSrc.Player.PointerDown.performed -= PointerDownPerformed;
            _inputActionsSrc.Player.PointerDown.canceled -= PointerUpPerformed;

            //  ----------------------------------------    Mouse
            _inputActionsSrc.Player.MouseRightBtnDown.performed -= MouseRightBtnDownPerformed;
            _inputActionsSrc.Player.MouseRightBtnDown.canceled -= MouseRightBtnUpPerformed;

            _inputActionsSrc.Player.MouseDelta.performed -= MouseDelta;

            //  ----------------------------------------    Mouse middle button
            _inputActionsSrc.Player.MouseMiddleBtnDown.started -= MouseMiddleBtnDownPerformed;
            _inputActionsSrc.Player.MouseMiddleBtnDown.canceled -= MouseMiddleBtnUpPerformed;

            //  ----------------------------------------    Mouse Y scroll
            _inputActionsSrc.Player.MouseScrollY.performed -= MouseScrollYPerformed;
            _inputActionsSrc.Player.MouseScrollY.canceled -= MouseScrollYCanceled;

            //  ----------------------------------------    Mouse
            _inputActionsSrc.Player.MouseMove.performed -= MouseMovePerformed;

            //  ----------------------------------------    Keyboard
            _inputActionsSrc.Player.WASD.performed -= Wasd;
            _inputActionsSrc.Player.WASD.canceled -= WasdCanceled;

            _inputActionsSrc.Player.Esc.canceled -= EscUp;
        }


        private void WasdCanceled(InputAction.CallbackContext context)
        {
            IInputEventsCallbackInvoker.WASDCanceled();
        }

        private void Wasd(InputAction.CallbackContext context)
        {
            IInputEventsCallbackInvoker.WASD(context.ReadValue<Vector2>()); 
        }

        private void EscUp(InputAction.CallbackContext context)
        {
            IInputEventsCallbackInvoker.EscUp(); 
        }

        private void MouseMovePerformed(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();

            if (IsMouseMiddleBtnDown && MousePosition != MousePreviousPosition)
                IInputEventsCallbackInvoker.MouseMiddleBtnDrag(MousePosition);

            MousePreviousPosition = MousePosition;
        }

        //  ----------------------------------------    Pointer
        #region Pointer 
        private void PointerPositionPerformed(InputAction.CallbackContext context)
        {
            PointerPosition = context.ReadValue<Vector2>();
            MouseDeltaProp = PointerPosition;
            IInputEventsCallbackInvoker.PointerDrag(PointerPosition);
        }

        private void PointerDownPerformed(InputAction.CallbackContext context)
        {
            //  In order to prevent double calling for devices with touchscreen Pointer event is called from Touch0DownPerformed
            PointerPosition = _inputActionsSrc.Player.PointerPosition.ReadValue<Vector2>();
            IsPointerDown = true;
            IInputEventsCallbackInvoker.PointerDown(PointerPosition); 
        }

        private void PointerUpPerformed(InputAction.CallbackContext context)
        {
            //  In order to prevent double calling for devices with touchscreen Pointer event is called from Touch0UpPerformed
            IsPointerDown = false;
            IInputEventsCallbackInvoker.PointerUp(PointerPosition);
        }
        #endregion

        //  ----------------------------------------    Mouse middle button
        #region Mouse middle button

        private void MouseRightBtnDownPerformed(InputAction.CallbackContext context)
        {
            IsMouseRightBtnDown = true;
            MousePosition = _inputActionsSrc.Player.PointerPosition.ReadValue<Vector2>();
            IInputEventsCallbackInvoker.MouseRightBtnDown(MousePosition);
        }

        private void MouseRightBtnUpPerformed(InputAction.CallbackContext context)
        {
            IsMouseRightBtnDown = false;
            MousePosition = _inputActionsSrc.Player.PointerPosition.ReadValue<Vector2>();
            IInputEventsCallbackInvoker.MouseRightBtnUp(MousePosition);
        }

        private void MouseMiddleBtnDownPerformed(InputAction.CallbackContext context)
        {
            IsMouseMiddleBtnDown = true;
            MousePosition = _inputActionsSrc.Player.PointerPosition.ReadValue<Vector2>();
            IInputEventsCallbackInvoker.MouseMiddleBtnDown(MousePosition); 
        }

        private void MouseMiddleBtnUpPerformed(InputAction.CallbackContext context)
        {
            IsMouseMiddleBtnDown = false;
            MousePosition = _inputActionsSrc.Player.PointerPosition.ReadValue<Vector2>();
            IInputEventsCallbackInvoker.MouseMiddleBtnUp(MousePosition);
            
        }
        #endregion

        //  ----------------------------------------    Mouse Y scroll
        #region Mouse Y scroll
        private void MouseScrollYPerformed(InputAction.CallbackContext context)
        {
            var mouseScrollValue = context.ReadValue<float>();

            if (mouseScrollValue == 0)
            {
                IInputEventsCallbackInvoker.MouseScrollYCanceled(mouseScrollValue);
            }

            IInputEventsCallbackInvoker.MouseScrollY(mouseScrollValue);
        }

        private void MouseScrollYCanceled(InputAction.CallbackContext context)
        {
            IInputEventsCallbackInvoker.MouseScrollYCanceled(0);
        }
        #endregion

        private void MouseDelta(InputAction.CallbackContext context)
        {
            MouseDeltaProp = context.ReadValue<Vector2>();
            IInputEventsCallbackInvoker.MouseDelta(MouseDeltaProp);
        }

    }
}

