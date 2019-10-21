using UnityEngine;

namespace Assets.Scripts.Movement
{
    class MoveController : MonoBehaviour
    {
        // based on:
        // - https://stuartspixelgames.com/2018/06/24/simple-2d-top-down-movement-unity-c/
        // - https://forum.unity.com/threads/click-drag-camera-movement.39513/

        Rigidbody2D body;

        float horizontal;
        float vertical;
        float moveLimiter = 0.7f;

        public float runSpeed = 20.0f;
        public bool enableMovement = true;

        Vector2 dragOrigin = default;

        void Start()
        {
            body = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            // Gives a value between -1 and 1
            // Also handles keyboard (WASD, arrows, ...)
            horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
            vertical = Input.GetAxisRaw("Vertical"); // -1 is down

            // Click&drag for mouse and touchscreen
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.GetMouseButton(0))
            {
                Vector2 mouseCurrentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var distance = mouseCurrentPos - dragOrigin;

                transform.position -= new Vector3(distance.x, distance.y);
            }
            else
            {
                dragOrigin = default;
            }
        }


        void FixedUpdate()
        {
            if (!enableMovement) { return; }

            if (horizontal != 0 && vertical != 0) // Check for diagonal movement
            {
                // limit movement speed diagonally, so you move at 70% speed
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }

            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        }
    }
}
