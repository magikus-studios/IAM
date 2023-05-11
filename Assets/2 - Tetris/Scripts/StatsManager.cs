using TMPro;
using UnityEngine;

namespace IAM.Tetris
{
    public class StatsManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreLabel;
        [SerializeField] private TMP_Text _linesLabel;

        [SerializeField] private int _score = 0;
        [SerializeField] private int _lines = 0;

        public void AddScore(int amount)
        {
            _score += amount;
            UpdateLabels();
        }
        public void AddLines(int amount)
        {
            _lines += amount;
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            if (_scoreLabel != null) { _scoreLabel.text = $"Score {_score:00000}"; }
            if (_linesLabel != null) { _linesLabel.text = $"Lines Cleared {_lines:0000}"; }
        }
    }
}
