using LD53.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace LD53.UI
{
    [RequireComponent(typeof(Button))]
    public class CloseButton : MonoBehaviour
    {
        [SerializeField] private GameObject _target;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Close);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
        }

        private void Close()
        {
            AudioManager.Instance.Play("back");
            _target.SetActive(false);
        }
    }
}