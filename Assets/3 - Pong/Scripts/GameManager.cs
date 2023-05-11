using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IAM.Pong
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private StatsManager _stats;
        [SerializeField] private Ball _ball;
        [SerializeField] private Paddle _playerPaddle;
        [SerializeField] private Paddle _computerPaddle;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _computerSpawnPoint;
        [SerializeField] private int _matchScore = 15;
        
        [SerializeField] private UnityEvent _onScore;
        [SerializeField] private UnityEvent _onPlayerWins;
        [SerializeField] private UnityEvent _onComputerWins;
        
        private bool _isPlayerTurn = true;
        private bool _isKickOff = true;

        private void Update()
        {
            if (_isKickOff) { PrepareForKickOff(); }
        }

        public void Play()
        {
            _ball.Play();
            _playerPaddle.Play();
            _computerPaddle.Play();
        }
        public void Pause()
        {
            _ball.Pause();
            _playerPaddle.Pause();
            _computerPaddle.Pause();
        }

        public void PlayerScore() 
        {
            if (_stats.PlayerScore >= _matchScore) 
            {
                Pause();
                _onPlayerWins?.Invoke();
                return;
            }
            _isPlayerTurn = false;
            _onScore?.Invoke();
        }
        public void ComputerScore() 
        {
            if (_stats.ComputerScore >= _matchScore) 
            {
                Pause();
                _onComputerWins?.Invoke();
                return;
            }
            _isPlayerTurn = true;
            _onScore?.Invoke();
        }

        public void PrepareForKickOff() 
        {
            _isKickOff = true;
            if (_isPlayerTurn) { _ball.Move(_playerSpawnPoint.position); }
            else { _ball.Move(_computerSpawnPoint.position); }
        }
        public void SpawnBall()
        {
            _isKickOff = false;
            if (_isPlayerTurn) { _ball.Spawn(_playerSpawnPoint.position, false); }
            else { _ball.Spawn(_computerSpawnPoint.position, true); }   
        }
    }
}