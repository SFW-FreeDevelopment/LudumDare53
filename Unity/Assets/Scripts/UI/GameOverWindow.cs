using LD53.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD53.UI
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private TextMeshProUGUI _daysPassedText, _totalCashMadeText, _conesServedText;
        
        private void Start()
        {
            var minigameManager = MinigameManager.Instance;
            _daysPassedText.text = $"Days Passed: {minigameManager.DayNumber}";
            _totalCashMadeText.text = $"Total Cash Made : {minigameManager.Cash}";
            _conesServedText.text = $"Cones Served: {minigameManager.ConesServed}";
            
            _mainMenuButton.onClick.AddListener(() =>
            {
                minigameManager.Quit();
            });
        }
    }
}