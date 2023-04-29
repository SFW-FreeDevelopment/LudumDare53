using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD53
{
    public class DrivingController : MonoBehaviour
    {
        public float speed = 10f;
        public float rotationSpeed = 100f;
        public float boostMultiplier = 2f;
        public float boostDuration = 1f;

        private bool isBoosting = false;
        private float boostEndTime = 0f;

        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(moveHorizontal, moveVertical);

            if (isBoosting && Time.time < boostEndTime)
            {
                movement *= boostMultiplier;
            }
            else
            {
                isBoosting = false;
            }

            rb.AddForce(movement * speed);

            float rotation = -moveHorizontal * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotation);

            if (Input.GetButtonDown("Boost") && !isBoosting)
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
