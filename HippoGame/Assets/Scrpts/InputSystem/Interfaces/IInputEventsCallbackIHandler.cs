using System;
using UnityEngine;

namespace Scripts.InputSystem
{
    public interface IInputEventsCallbackIHandler 
    {
        //  ----------------------------------------    Pointer 
        public event Action<Vector2> OnPointerDown;
        public event Action<Vector2> OnPointerDrag;
        public event Action<Vector2> OnPointerUp;

        //  ----------------------------------------    Mouse
        public event Action<Vector2> OnMouseRightBtnDown;
        public event Action<Vector2> OnMouseRightBtnUp;

        public event Action<Vector2> OnMouseDelta; 

        //  ----------------------------------------    Mouse middle button
        public event Action<Vector2> OnMouseMiddleBtnDown;
        public event Action<Vector2> OnMouseMiddleBtnDrag;
        public event Action<Vector2> OnMouseMiddleBtnUp;

        //  ----------------------------------------    Mouse Y scroll
        public event Action<float> OnMouseScrollY;
        public event Action<float> OnMouseScrollYCanceled;

        //  ----------------------------------------    Keyboard
        public event Action<Vector2> OnWASD;
        public event Action OnWASDCanceled;

        public event Action OnEscUp;
    }
}
