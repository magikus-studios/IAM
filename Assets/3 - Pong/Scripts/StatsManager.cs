using TMPro;
using UnityEngine;

namespace IAM.Pong
{
    public class StatsManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerScoreLabel;
        [SerializeField] private TMP_Text _computerScoreLabel;

        [SerializeField] public int PlayerScore { get; private set; } = 0;
        [SerializeField] public int ComputerScore { get; private set; } = 0;

        public void AddPlayerScore(int amount)
        {
            PlayerScore += amount;
            UpdateLabels();
        }
        public void AddComputerScore(int amount)
        {
            ComputerScore += amount;
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            if (_playerScoreLabel != null) { _playerScoreLabel.text = $"{PlayerScore:00}"; }
            if (_computerScoreLabel != null) { _computerScoreLabel.text = $"{ComputerScore:00}"; }
        }
    }
}