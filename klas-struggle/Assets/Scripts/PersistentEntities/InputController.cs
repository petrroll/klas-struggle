using System.Linq;
using UnityEngine;

namespace Assets.Scripts.PersistentEntities
{
    public enum InputType { Nothing, Touch, Keyboard, Mouse, Controller}

    class InputController : MonoBehaviour
    {
        public InputType LastUsed { get; private set; }

        public void Start()
        {
            LastUsed = Input.touchSupported ? InputType.Touch : InputType.Keyboard;
        }

        void OnGUI()
        {
            var newInput = IsKeyboard()
                ? InputType.Keyboard
                : IsMouse()
                ? InputType.Mouse
                : IsTouch()
                ? InputType.Touch
                : IsController()
                ? InputType.Controller
                : LastUsed;

            if (newInput != LastUsed)
            {
                Debug.Log($"New input type: {newInput} old {LastUsed}");
                LastUsed = newInput;
            }
        }

        private bool IsKeyboard() => Event.current.isKey;

        private bool IsMouse() => Event.current.isMouse || Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f;

        private bool IsTouch() => Input.touchCount > 0;

        private bool IsController() => Enumerable.Range((int)KeyCode.JoystickButton0, (int)KeyCode.Joystick8Button19 - (int)KeyCode.JoystickButton0).Any(v => Input.GetKey((KeyCode)v));
    }
}
