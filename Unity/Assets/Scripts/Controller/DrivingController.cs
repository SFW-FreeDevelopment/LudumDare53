using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD53
{
    public class DrivingController : MonoBehaviour
    {
        public float maxSpeed = 10f;
        public float acceleration = 5f;
        public float rotationSpeed = 100f;
        public float boostMultiplier = 2f;
        public float boostDuration = 1f;
        public float driftMultiplier = 0.1f;

        private bool isBoosting = false;
        private float boostEndTime = 0f;

        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 movement = transform.up * moveVertical;
            movement.Normalize();

            if (isBoosting && Time.time < boostEndTime)
            {
                movement *= boostMultiplier;
            }
            else
            {
                isBoosting = false;
            }

            // Calculate speed based on current velocity.
            float currentSpeed = rb.velocity.magnitude;
            if (currentSpeed < maxSpeed)
            {
                rb.AddForce(movement * acceleration);
            }

            // Calculate drift force based on velocity and rotation.
            float driftAmount = Vector2.Dot(rb.velocity, -transform.right.normalized);
            Vector2 driftForce = transform.right * driftAmount * driftMultiplier;

            rb.AddForce(driftForce);

            float rotation = -Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotation);

            if (Input.GetKeyDown(KeyCode.Space) && !isBoosting)
            {
                isBoosting = true;
                boostEndTime = Time.time + boostDuration;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }
}
