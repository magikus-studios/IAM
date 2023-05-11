using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace IAM.Asteroids
{
    public class StatsManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreLabel;
        [SerializeField] private TMP_Text _livesLabel;

        [SerializeField] private int _score = 0;
        [SerializeField] private int _lives = 3;

        [SerializeField] private UnityEvent _onOutOfLives;

        public bool IsOutOfLives() { return _lives == 0; }
        public void AddScore(int amount) 
        {
            _score += amount;
            UpdateLabels();
        }
        public void LoseLife() 
        {
            _lives--;
            UpdateLabels();
            if (IsOutOfLives()) { _onOutOfLives?.Invoke(); }
        }

        private void UpdateLabels()
        {
            if (_scoreLabel != null) { _scoreLabel.text = $"Score {_score:00000}"; }
            if (_livesLabel != null) { _livesLabel.text = $"Lives {LivesToSymbols()}"; }
        }

        private string LivesToSymbols() 
        {
            string symbols = "";
            string symbol = "x";
            for (int i = 0; i < _lives; i++) { symbols += symbol; }
            return symbols;
        }
    }
}
