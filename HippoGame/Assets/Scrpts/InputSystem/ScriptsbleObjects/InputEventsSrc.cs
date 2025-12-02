using UnityEngine;
using System;

namespace Scripts.InputSystem
{
    [CreateAssetMenu(fileName = "InputEvents", menuName = "Scriptable Obj/Input system/Input events")]
    public class InputEventsSrc : ScriptableObject,
        IInputEventsCallbackIHandler,
        IInputEventsCallbackInvoker,
        IInputEventsHandler,
        IInputEventsInvoker
    {
        //  ----------------------------------------    Pointer 0
        #region Pointer

        private Action<Vector2> _onPointerDown;
        event Action<Vector2> IInputEventsCallbackIHandler.OnPointerDown
        {
            add => _onPointerDown += value;
            remove => _onPointerDown -= value;
        }
        void IInputEventsCallbackInvoker.PointerDown(Vector2 position) => _onPointerDown?.Invoke(position);
        
        private Action<Vector2> _onPointerDrag;
        event Action<Vector2> IInputEventsCallbackIHandler.OnPointerDrag
        {
            add => _onPointerDrag += value;
            remove => _onPointerDrag -= value;
        }
        void IInputEventsCallbackInvoker.PointerDrag(Vector2 position) => _onPointerDrag?.Invoke(position);
        
        private Action<Vector2> _onPointerUp;
        event Action<Vector2> IInputEventsCallbackIHandler.OnPointerUp
        {
            add => _onPointerUp += value;
            remove => _onPointerUp -= value;
        }
        void IInputEventsCallbackInvoker.PointerUp(Vector2 position) => _onPointerUp?.Invoke(position);
        
        #endregion


        //  ----------------------------------------    Mouse 
        private Action<Vector2> _onMouseDelta;
        event Action<Vector2> IInputEventsCallbackIHandler.OnMouseDelta
        {
            add => _onMouseDelta += value;
            remove => _onMouseDelta -= value;
        }
        void IInputEventsCallbackInvoker.MouseDelta(Vector2 value) => _onMouseDelta?.Invoke(value);


        private Action<Vector2> _onMouseRightBtnDown;
        event Action<Vector2> IInputEventsCallbackIHandler.OnMouseRightBtnDown
        {
            add => _onMouseRightBtnDown += value;
            remove => _onMouseRightBtnDown -= value;
        }
        void IInputEventsCallbackInvoker.MouseRightBtnDown(Vector2 value) => _onMouseRightBtnDown?.Invoke(value);


        private Action<Vector2> _onMouseRightBtnUp;
        event Action<Vector2> IInputEventsCallbackIHandler.OnMouseRightBtnUp
        {
            add => _onMouseRightBtnUp += value;
            remove => _onMouseRightBtnUp -= value;
        }
        void IInputEventsCallbackInvoker.MouseRightBtnUp(Vector2 value) => _onMouseRightBtnUp?.Invoke(value);

        //  ----------------------------------------    Mouse middle button
        #region Mouse middle button
        private Action<Vector2> _onMouseMiddleBtnDown;
        event Action<Vector2> IInputEventsCallbackIHandler.OnMouseMiddleBtnDown
        {
            add => _onMouseMiddleBtnDown += value;
            remove => _onMouseMiddleBtnDown -= value;
        }
        void IInputEventsCallbackInvoker.MouseMiddleBtnDown(Vector2 position)=> _onMouseMiddleBtnDown?.Invoke(position);


        private Action<Vector2> _onMouseMiddleBtnDrag;
        event Action<Vector2> IInputEventsCallbackIHandler.OnMouseMiddleBtnDrag
        {
            add => _onMouseMiddleBtnDrag += value;
            remove => _onMouseMiddleBtnDrag -= value;
        }
        void IInputEventsCallbackInvoker.MouseMiddleBtnDrag(Vector2 position) => _onMouseMiddleBtnDrag?.Invoke(position);
        

        private Action<Vector2> _onMouseMiddleBtnUp;
        event Action<Vector2> IInputEventsCallbackIHandler.OnMouseMiddleBtnUp
        {
            add => _onMouseMiddleBtnUp += value;
            remove => _onMouseMiddleBtnUp -= value;
        }
        void IInputEventsCallbackInvoker.MouseMiddleBtnUp(Vector2 position) => _onMouseMiddleBtnUp?.Invoke(position);
        
        #endregion


        //  ----------------------------------------    Mouse Y scroll
        #region Mouse Y scroll
        private Action<float> _onMouseScrollY;
        event Action<float> IInputEventsCallbackIHandler.OnMouseScrollY
        {
            add => _onMouseScrollY += value;
            remove => _onMouseScrollY -= value;
        }
        void IInputEventsCallbackInvoker.MouseScrollY(float scrollValue) => _onMouseScrollY?.Invoke(scrollValue);
        public void MouseScrollY(float scrollValue = 0)
        {
            _onMouseScrollY?.Invoke(scrollValue);
        }


        private Action<float> _onMouseScrollYCanceled;
        event Action<float> IInputEventsCallbackIHandler.OnMouseScrollYCanceled
        {
            add => _onMouseScrollYCanceled += value;
            remove => _onMouseScrollYCanceled -= value;
        }
        void IInputEventsCallbackInvoker.MouseScrollYCanceled(float scrollValue) => _onMouseScrollYCanceled?.Invoke(scrollValue);

        #endregion


        //  ----------------------------------------    Keyboard input
        private Action<Vector2> _onWASD;
        event Action<Vector2> IInputEventsCallbackIHandler.OnWASD
        {
            add => _onWASD += value;
            remove => _onWASD -= value; 
        }

        void IInputEventsCallbackInvoker.WASD(Vector2 direction) => _onWASD?.Invoke(direction);


        private Action _onWASDCanceled;
        event Action IInputEventsCallbackIHandler.OnWASDCanceled
        {
            add => _onWASDCanceled += value;
            remove => _onWASDCanceled -= value;
        }

        void IInputEventsCallbackInvoker.WASDCanceled() => _onWASDCanceled?.Invoke();


        private Action _onEscUp;
        event Action IInputEventsCallbackIHandler.OnEscUp
        {
            add => _onEscUp += value;
            remove => _onEscUp -= value;
        }

        void IInputEventsCallbackInvoker.EscUp() => _onEscUp?.Invoke();
    }
}

