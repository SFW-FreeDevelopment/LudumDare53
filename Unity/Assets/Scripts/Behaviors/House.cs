using LD53.Managers;
using UnityEngine;

namespace LD53.Behaviors
{
    public class House : MonoBehaviour
    {
        public bool Active { get; set; }
        public bool JustLeft { get; set; }

        private void OnCollisionEnter2D(Collision2D col)
        {
            Debug.Log($"OnCollisionEnter2D: {Active}, {JustLeft}, {gameObject != MinigameManager.Instance.LastHouse}");
            Debug.Log($"{gameObject.GetInstanceID()} {MinigameManager.Instance.LastHouseGameObjectId}");
            Debug.Log($"{MinigameManager.Instance.LastHouseCoordinates.x},{MinigameManager.Instance.LastHouseCoordinates.y} {transform.position.x}, {transform.position.y}");
            
            if (!Active && !JustLeft
                        && MinigameManager.Instance.LastHouseCoordinates != (transform.position.x, transform.position.y))
            {
                AudioManager.Instance.Play("car-crash");
                return;
            }
            MinigameManager.Instance.DriveUp(this);
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            Debug.Log($"OnCollisionExit2D: {Active}, {JustLeft}, {gameObject != MinigameManager.Instance.LastHouse}");
            if (!Active && JustLeft)
                JustLeft = false;
        }
    }
}