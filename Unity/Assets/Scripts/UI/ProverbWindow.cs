using System;
using LD53.Managers;
using LD53.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD53.UI
{
    public class ProverbWindow : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private StringList _stringList;
        
        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void OnEnable()
        {
            _text.text = _stringList.GetRandom();
        }

        private void Close()
        {
            MinigameManager.Instance.CloseProverbWindow();
            gameObject.SetActive(false);
        }
    }
}