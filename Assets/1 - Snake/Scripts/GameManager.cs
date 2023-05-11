using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

namespace IAM.Snake
{
    public class GameManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _debug;
        [SerializeField] private float _initialSpeed;
        [SerializeField] private float _speedIncrement;

        [Header("Objects")]
        [SerializeField] private StatsManager _stats;
        [SerializeField] private Snake _snake;
        [SerializeField] private Fruit _fruit;
        [SerializeField] private Tilemap _wallTilemap;
        [SerializeField] private Vector2Int _wallTilemapOffset;
        [SerializeField] private Tilemap _groundTilemap;
        [SerializeField] private Vector2Int _groundTilemapOffset;

        [Header("App Events")]
        [SerializeField] private UnityEvent _onPlay;
        [SerializeField] private UnityEvent _onPause;

        [Header("Game Events")]
        [SerializeField] private UnityEvent _onEat;
        [SerializeField] private UnityEvent _onMove;
        [SerializeField] private UnityEvent _onDeath;
        [SerializeField] private UnityEvent _onGameOver;

        private bool _outOfLives { get { return _stats.IsOutOfLives(); } }
        private bool _isPaused = true;
        private float _timer = 0f;
        private float _speed = 1f;
        private List<Vector2Int> _wallPositionList = new List<Vector2Int>();
        private List<Vector2Int> _groundPositionList = new List<Vector2Int>();

       
        private void Awake()
        {
            _wallPositionList = GetPositionListFromTilemap(_wallTilemap, _wallTilemapOffset);
            _groundPositionList = GetPositionListFromTilemap(_groundTilemap, _groundTilemapOffset);    
            _speed = _initialSpeed;
        }
        private void FixedUpdate()
        {
            if (_isPaused) { return; }

            _timer -= Time.fixedDeltaTime;
            if (_timer > 0) { return; }

            Move();
            CheckCollision();

            _timer = 1f / _speed;
        }

        public void Play()
        {
            _isPaused = false;
            if (_debug) { print("Snake - Play"); }
            _onPlay?.Invoke();
        }
        public void Pause()
        {
            _isPaused = true;
            if (_debug) { print("Snake - Pause"); }
            _onPause?.Invoke();
        }

        public void Respawn()
        {
            _snake.Restart();
            Play();
        }

        private void Move()
        {
            _snake.Move();

            if (_debug) { print("Snake - Move"); }
            _onMove?.Invoke();
        }
        private void CheckCollision()
        {
            if (_snake.transform.position == _fruit.transform.position) { Eat(); return; }
            if (_snake.CheckSelfCollision()) 
            {
                if (_debug) { print("Self Colition"); } 
                Die(); 
                return; 
            }
            if (CheckCollisionWithWalls(_snake.transform.position.ToVector2Int())) 
            {
                if (_debug) { print("Colition with Walls"); }
                Die();
                return; 
            }
        }
        private bool CheckCollisionWithWalls(Vector2Int position) 
        {
            foreach (Vector2Int tilePosition in _wallPositionList) { if (position == tilePosition) { return true; } }
            return false;
        }
        
        private void Die() 
        {
            LoseLife();
            if (_outOfLives) { GameOver(); return; }
        
            if (_debug) { print("Snake - Die"); }
            _onDeath?.Invoke();
        }
        private void LoseLife()
        {
            _stats.LoseLife();
        }
        private void GameOver()
        {
            if (_debug) { print("Snake - Game Over"); }
            _onGameOver?.Invoke();
        }
        
        private void Eat()
        {
            if (_debug) { print("Snake - Eat"); }
            AddScore();
            SpawnFruit();
            GrowSnake();
            _speed += _speedIncrement;
            _onEat?.Invoke();
        }
        private void AddScore() 
        {
            _stats.AddScore(_fruit.Points);
        }
        private void SpawnFruit() 
        {
            List<Vector2Int> spawningPositions = GetAvailableSpawningPosition();
            Vector2Int newSpawnPosition = spawningPositions[UnityEngine.Random.Range(0,spawningPositions.Count)];
            _fruit.Spawn(newSpawnPosition);
        }
        private void RespawnFruit()
        {
            List<Vector2Int> spawningPositions = GetAvailableSpawningPosition();
            Vector2Int newSpawnPosition = spawningPositions[UnityEngine.Random.Range(0, spawningPositions.Count)];
            _fruit.Respawn(newSpawnPosition);
        }
        private void GrowSnake() { _snake.Grow(); }

        private List<Vector2Int> GetAvailableSpawningPosition() 
        {
            List<Vector2Int> availablePositions = new List<Vector2Int>();
            List<Vector2Int> snakePositions = _snake.GetSnakePositions();

            bool available;
            foreach (Vector2Int groundPosition in _groundPositionList) 
            {
                available = true;
                foreach (Vector2Int snakePosition in snakePositions)
                {
                    if (groundPosition == snakePosition) { available = false; break; }
                }

                if (available) { availablePositions.Add(groundPosition); }
            }

            return availablePositions;
        }
        private List<Vector2Int> GetPositionListFromTilemap(Tilemap tilemap, Vector2Int offset)
        {
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] tilesArray = tilemap.GetTilesBlock(bounds);

            TileBase tile;
            List<Vector2Int> positionList = new List<Vector2Int>();
            Vector2Int tilePosition;

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    tile = tilesArray[x + y * bounds.size.x];
                    if (tile != null) 
                    {
                        tilePosition = new Vector2Int(x - offset.x + 1, y - offset.y + 1);
                        positionList.Add(tilePosition);                         
                    }
                }
            }
            return positionList;
        }
    }
}