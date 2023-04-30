using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD53
{
    public class DrivingController : MonoBehaviour
    {
        // Public variables
        public float maxSpeed = 10f;
        public float acceleration = 5f;
        public float deceleration = 5f;
        public float reverseSpeed = 2f;
        public float reverseDeceleration = 4f;
        public float rotationSpeed = 100f;


        // Private variables
        private Rigidbody2D rb;
        private float currentSpeed = 0f;
        private float decelerationPercentage = 0.1f;

        // Start is called before the first frame update
        void Start()
        {
            // Get a reference to the Rigidbody2D component
            rb = GetComponent<Rigidbody2D>();
        }

        // FixedUpdate is called once per physics frame
        void FixedUpdate()
        {
            //Move car forward, decelerate, brake, or reverse depending on input
            VerticalMove();

            // Move the car based on speed and direction
            Vector2 movement = transform.up * currentSpeed;
            rb.velocity = movement;

            //Rotate car left or right
            RotateCar();


        }

        void VerticalMove()
        {
            var input = GetVerticalInput();
            switch (input)
            {
                case 0:
                    decelerationPercentage += Time.fixedDeltaTime;
                    Decelerate();
                    break;
                case 1:
                    decelerationPercentage = 0.1f;
                    Accelerate();
                    break;
                case -1:
                    decelerationPercentage = 0.5f;
                    Brake();
                    break;
            }
        }

        int GetVerticalInput()
        {
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (verticalInput > 0f)
            {
                return 1;  // W or Up Arrow was pressed
            }
            else if (verticalInput < 0f)
            {
                return -1;  // S or Down Arrow was pressed
            }
            else
            {
                return 0;  // No vertical input
            }
        }

        void RotateCar()
        {
            var input = GetHorizontalInput();
            transform.Rotate(0f, 0f, -input * rotationSpeed * Time.fixedDeltaTime);
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
            float decelerationAmount = currentSpeed * decelerationPercentage;
            currentSpeed -= decelerationAmount * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        }

        void Brake()
        {
            if (currentSpeed > 0.05f)
            {
                float decelerationAmount = currentSpeed * decelerationPercentage * 1.5f;
                currentSpeed -= decelerationAmount * Time.fixedDeltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
            }
            else
            {
                Reverse();
            }
        }

        void Reverse()
        {
            currentSpeed -= reverseSpeed * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed*0.5f, 0f);
        }
    }
}
