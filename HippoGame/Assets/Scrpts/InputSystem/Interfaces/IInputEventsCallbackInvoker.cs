using UnityEngine;

namespace Scripts.InputSystem
{
    public interface IInputEventsCallbackInvoker 
    {

        //  ----------------------------------------    Pointer 
        public void PointerDown(Vector2 position);
        public void PointerDrag(Vector2 position);
        public void PointerUp(Vector2 position);

        //  ----------------------------------------    Mouse 
        public void MouseRightBtnDown(Vector2 position);
        public void MouseRightBtnUp(Vector2 position);

        public void MouseDelta(Vector2 direction);

        //  ----------------------------------------    Mouse middle button
        public void MouseMiddleBtnDown(Vector2 position);
        public void MouseMiddleBtnDrag(Vector2 position);
        public void MouseMiddleBtnUp(Vector2 position);

        //  ----------------------------------------    Mouse Y scroll
        public void MouseScrollY(float scrollDirection);
        public void MouseScrollYCanceled(float scrollDirection);

        //  ----------------------------------------    Keyboard
        public void WASD(Vector2 direction);
        public void WASDCanceled();

        public void EscUp(); 
    }
}
