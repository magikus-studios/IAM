using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace IAM.Breakout
{
    public class StatsManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreLabel;
        [SerializeField] private TMP_Text _livesLabel;
        [SerializeField] private TMP_Text _levelLabel;
        [SerializeField] private TMP_Text _bricksLabel;

        [SerializeField] private int _score = 0;
        [SerializeField] private int _lives = 3;
        [SerializeField] private int _bricksLeft = 0;
        [SerializeField] private int _levels = 0;

        [SerializeField] private UnityEvent _onOutOfLives;
        [SerializeField] private UnityEvent _onLifeLose;

        public bool IsOutOfLives() { return _lives == 0; }
        public void AddScore(int amount) 
        {
            _score += amount;
            UpdateLabels();
        }
        public void AddLevel()
        {
            _levels++;
            UpdateLabels();
        }
        public void SubBrick()
        {
            _bricksLeft--;
            UpdateLabels();
        }
        public void SetBricks(int amount)
        {
            _bricksLeft = amount;
            UpdateLabels();
        }
        public void LoseLife() 
        {
            _lives--;
            UpdateLabels();
            if (IsOutOfLives()) { _onOutOfLives?.Invoke(); return; }
            _onLifeLose?.Invoke();
        }

        private void UpdateLabels()
        {
            if (_scoreLabel != null) { _scoreLabel.text = $"Score {_score:00000}"; }
            if (_levelLabel != null) { _levelLabel.text = $"Level {_levels:00}"; }
            if (_bricksLabel != null) { _bricksLabel.text = $"{_bricksLeft:00}"; }
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
