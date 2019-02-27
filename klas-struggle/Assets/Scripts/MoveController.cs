using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class MoveController : MonoBehaviour
    {
        // based on https://stuartspixelgames.com/2018/06/24/simple-2d-top-down-movement-unity-c/

        Rigidbody2D body;

        float horizontal;
        float vertical;
        float moveLimiter = 0.7f;

        public float runSpeed = 20.0f;
        public bool enableMovement = true;

        void Start()
        {
            body = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { horizontal = -1; }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) { horizontal = 1; }

            if (Input.GetKeyDown(KeyCode.UpArrow)) { vertical = 1; }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) { horizontal = -1; }

            // Gives a value between -1 and 1
            horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
            vertical = Input.GetAxisRaw("Vertical"); // -1 is down
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
