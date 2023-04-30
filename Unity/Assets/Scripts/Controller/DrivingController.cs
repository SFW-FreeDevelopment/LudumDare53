using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD53
{
    public class DrivingController : MonoBehaviour
    {
        // Public variables
        public float maxSpeed = 2f;
        public float acceleration = 1f;
        public float deceleration = .5f;
        public float reverseSpeed = 200f;
        public float reverseDeceleration = 0f;
        public float rotationSpeed = 1.7f;

        // Private variables
        private Rigidbody2D rb;
        private float currentSpeed = 0f;
        private int verticalInput = 0;
        private int horizontalInput = 0;
        private bool isReversing = false;

        // Start is called before the first frame update
        void Start()
        {
            // Get a reference to the Rigidbody2D component
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            verticalInput = GetVerticalInput();
            horizontalInput = GetHorizontalInput();
        }

        // FixedUpdate is called once per physics frame
        void FixedUpdate()
        {
            //Move car forward, decelerate, brake, or reverse depending on input and return what type of movement was done
            var movement = VerticalMove();

            //Rotate car left or right, slide depending on what type vertical movement was done. Anything with making the car move that is not forward or back.
            HorizontalMovement(movement);

        }

        private MovementCase VerticalMove()
        {
            MovementCase movementCase = new();
            switch (verticalInput)
            {
                case 0:
                    Decelerate();
                    movementCase = MovementCase.Neutral;
                    break;
                case 1:
                    Accelerate();
                    movementCase = MovementCase.Accelerating;
                    break;
                case -1:
                    movementCase = Brake();
                    break;
                default:
                    throw new NotImplementedException();
            }
            DoVerticalMove();
            return movementCase;
        }

        void DoVerticalMove()
        {
            // Move the car based on speed and direction
            Vector2 movement = transform.up * currentSpeed;
            rb.velocity = movement;

        }
        void HorizontalMovement(MovementCase movement)
        {
            RotateCar(movement);
        }

        int GetVerticalInput()
        {
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (verticalInput > 0f)
            {
                isReversing = false;
                return 1;  // W or Up Arrow was pressed
            }
            else if (verticalInput < 0f)
            {
                isReversing = true;
                return -1;  // S or Down Arrow was pressed
            }
            else
            {
                isReversing = false;
                return 0;  // No vertical input
            }
        }


        void RotateCar(MovementCase movement)
        {
            //Only apply rotation if car is moving
            if (rb.velocity.magnitude < 0.05)
            {
                return;
            }
            
            switch (movement)
            {
                case MovementCase.Reverse:
                    rb.rotation -= horizontalInput * rotationSpeed;
                    break;
                case MovementCase.Neutral:
                    rb.rotation -= horizontalInput * rotationSpeed;
                    break;
                case MovementCase.Braking:
                    rb.rotation -= horizontalInput * rotationSpeed;
                    break;
                case MovementCase.Accelerating:
                    rb.rotation -= horizontalInput * rotationSpeed;
                    break;
            }
        }

        int GetHorizontalInput()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            if (horizontalInput > 0f)
            {
                return 1;  // D or Right Arrow was pressed
            }
            else if (horizontalInput < 0f)
            {
                return -1;  // A or Left Arrow was pressed
            }
            else
            {
                return 0;  // No horizontal input
            }
        }

        void Accelerate()
        {
            currentSpeed += acceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        }

        void Decelerate()
        {
            if (currentSpeed > 0f)
            {
                currentSpeed -= acceleration * Time.fixedDeltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
            }

            if (currentSpeed < 0f)
            {
                currentSpeed += acceleration * Time.fixedDeltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed*0.5f, 0f);
            }
        }

        public MovementCase Brake()
        {
            if (rb.velocity.magnitude > 0 && !isReversing)
            {
                Decelerate();
                return MovementCase.Braking;
            }
            else
            {
                Reverse();
                return MovementCase.Reverse;
            }

            throw new NotImplementedException();
        }

        void Reverse()
        {
            currentSpeed -= reverseSpeed * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed*0.5f, 0f);
        }
    }
}

public enum MovementCase
{
    Reverse = -1,
    Neutral = 0,
    Braking = 1,
    Accelerating = 2
}
