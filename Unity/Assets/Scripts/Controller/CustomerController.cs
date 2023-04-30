using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LD53
{
    public class CustomerController : MonoBehaviour
    {
        // Public variables
        public float transitionTime = 1.0f; // Duration of transition in seconds
        public float yOffset = 2.0f; // The amount to move the object up or down (in world space)

        // Private variables
        private bool isTransitioning = false; // Flag to indicate whether the object is currently transitioning
        private Vector3 startPos; // The initial position of the object before transitioning
        private Vector3 targetPos; // The target position of the object during transitioning
        private bool isTargetAboveStart; // Flag to indicate whether the target position is above the initial position

        private void Start()
        {
            // Save the initial position of the object
            startPos = transform.position;

            // Calculate the target position for the object based on its starting position and the yOffset value
            targetPos = startPos + new Vector3(0f, yOffset, 0f);

            // Calculate whether the target position is above or below the initial position
            isTargetAboveStart = targetPos.y > startPos.y;
        }

        private void Update()
        {
            // Check if the condition for transitioning has been met
            if (ConditionForTransition())
            {
                // Start the transition if it's not already in progress and the object is not in transitional state
                if (!isTransitioning)
                {
                    isTransitioning = true;
                    StartCoroutine(DoTransition());
                }
            }
        }

        private bool ConditionForTransition()
        {
            // TODO: Replace with the actual condition for toggling the transition
            return Input.GetKeyDown(KeyCode.Space); // Example condition based on Input
        }

        private System.Collections.IEnumerator DoTransition()
        {
            float elapsedTime = 0.0f;
            Vector3 startPos2 = transform.position; // Save the current position of the object each frame

            // Loop until the transition is complete
            while (elapsedTime < transitionTime)
            {
                // Calculate the new position of the object based on the current time and target position
                Vector3 newPos = Vector3.Lerp(startPos2, targetPos, elapsedTime / transitionTime);

                // Move the object to the new position
                transform.position = newPos;

                // Update the elapsed time
                elapsedTime += Time.deltaTime;

                // Wait until the next frame before continuing the loop
                yield return null;
            }

            // Snap the object to its final position to avoid any floating-point errors
            transform.position = targetPos;

            // Kill the coroutine and reset the flag
            StopCoroutine(DoTransition());
            isTransitioning = false;

            // Swap the target position and the start position
            Vector3 tempPos = targetPos;
            targetPos = startPos;
            startPos = tempPos;
        }
    }

}
