using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace IAM.Tetris
{
    public class Board : MonoBehaviour
    {
        public enum STATE { START, PLAY, PAUSE, OVER }

        [SerializeField] private Tetromino[] _tetrominos;
        [SerializeField] private Tilemap _boardTilemap;
        [SerializeField] private Tilemap _ghostTilemap;
        [SerializeField] private Tilemap _previewTilemap;
        [SerializeField] private Tile _ghostTile;
        [field: SerializeField] public Vector2Int BoardSize { get; private set; }        
        [SerializeField] private Transform _spawnPositionObject;
        [SerializeField] private float _stepTime = 1f;
        [SerializeField] private Vector2Int _previewDirection;
        [SerializeField] private int _previewSpacing = 3;
        [SerializeField] private int _scorePerLine = 10;

        [Header("Movement")]
        public UnityEvent OnStep;
        public UnityEvent OnMovePiece;
        public UnityEvent OnRotatePiece;
        public UnityEvent OnLockPiece;
        public UnityEvent OnFallDownPiece;

        [Header("Game")]
        public UnityEvent OnLineCleared;
        public UnityEvent<int> OnScore;
        public UnityEvent<string> OnScorePopUp;
        public UnityEvent OnGameOver;

        private RectInt _boardBounds { get { return new RectInt(new Vector2Int(-BoardSize.x / 2, -BoardSize.y / 2), BoardSize); } }
        private Vector3Int _spawnPosition { get { return _spawnPositionObject.position.ToVector3Int(); } }


        private float _stepTimer;
        private float _lockTimer;

        private List<Piece> _previewQueue;
        private Piece _activePiece;
        private Piece _ghostPiece;
        private bool _ghostPieceEnable = true;
        private int _previewQueueSize = 5;

        private bool _fallDown = false;
        private int _rotationAngle = 0;
        private Vector2Int _movementDirection = Vector2Int.zero;

        private STATE _state = STATE.START;

        private void Awake() { if (_boardTilemap == null) { _boardTilemap = GetComponentInChildren<Tilemap>(); } }
        private void Update()
        {
            if (_state != STATE.PLAY) { return; }

            Step();
        }

        public void StartGame() 
        {
            if (_state != STATE.START) { return; }
            PopulatePreviewQueue();
            SpawnPiece();
            _state = STATE.PLAY;
        }
        public void Play() 
        {
            if (_state != STATE.PAUSE) { return; }
            _state = STATE.PLAY; 
        }
        public void Pause() 
        {
            if (_state != STATE.PLAY) { return; }
            _state = STATE.PAUSE; 
        }
        public void GameOver() 
        {
            if (_state != STATE.PLAY) { return; }
            _state = STATE.OVER;
            OnGameOver?.Invoke();
        }

        public void MoveLeft() { _movementDirection = Vector2Int.left; }
        public void MoveRight() { _movementDirection = Vector2Int.right; }
        public void MoveDown() { _movementDirection = Vector2Int.down; }
        public void RotateLeft() { _rotationAngle = -1; }
        public void RotateRight() { _rotationAngle = 1; }
        public void FallDown() { _fallDown = true; }

        public void SetStepTime(float stepDelay) { _stepTime = stepDelay; }
        public void AddStepTime(float stepDelay) { _stepTime += stepDelay; }
        public void ProdStepTime(float stepDelay) { _stepTime *= stepDelay; }

        public void SetPreviewSize(int size) 
        {
            _previewQueueSize = Mathf.Clamp(size, 0, _previewQueue.Count);
            ClearPreviewQueue();
            DrawPreviwQueue();
        }

        public void EnableGhostPiece() { _ghostPieceEnable = true; DrawGhostPiece(); }
        public void DisableGhostPiece() { _ghostPieceEnable = false; ClearGhostPiece(); }

        public void Step()
        {
            ClearPiece();

            _lockTimer += Time.deltaTime;
            _stepTimer += Time.deltaTime;

            if (_fallDown) { FallDownActivePiece(); }
            else if (_rotationAngle != 0) { RotateActivePiece(_rotationAngle); }
            else if (_movementDirection != Vector2Int.zero) { MoveActivePiece(_movementDirection); }

            if (_stepTimer >= _stepTime) { StepActivePiece(); }

            if (_ghostPieceEnable) { DrawGhostPiece(); }
            SetPiece();
        }

        private void StepActivePiece()
        {
            _stepTimer = 0f;
            TryMoveActivePiece(Vector2Int.down);
            if (_lockTimer >= _stepTime) { LockActivePiece(); }
            else { OnStep?.Invoke(); }
        }
        private void LockActivePiece()
        {
            _lockTimer = 0f;
            _stepTimer = 0f;

            SetPiece();
            CheckLines();
            OnLockPiece?.Invoke();
            SpawnPiece();
        }

        private void SpawnPiece()
        {
            SetActivePieceFromPreviewQueue();
            ClearGhostPiece();

            if (!IsValidPosition(_spawnPosition)) { GameOver(); }

            SetPiece();
        }

        private void SetActivePieceFromPreviewQueue() 
        {
            ClearPreviewQueue();
            _previewQueue.Add(new Piece(_spawnPosition, 0, _tetrominos.RandomItem()));
            _activePiece = _previewQueue[0];
            _previewQueue.RemoveAt(0);
            _activePiece.Position = _spawnPosition;
            DrawPreviwQueue();
        }
        private void PopulatePreviewQueue() 
        {
            _previewQueue = new List<Piece>();
            _previewQueue.Add(new Piece(_spawnPosition, 0, _tetrominos.RandomItem()));
            _previewQueue.Add(new Piece(_spawnPosition, 0, _tetrominos.RandomItem()));
            _previewQueue.Add(new Piece(_spawnPosition, 0, _tetrominos.RandomItem()));
            _previewQueue.Add(new Piece(_spawnPosition, 0, _tetrominos.RandomItem()));
            _previewQueue.Add(new Piece(_spawnPosition, 0, _tetrominos.RandomItem()));
        }
        private void ClearPreviewQueue() 
        {
            if (_previewTilemap == null) { return; }

            foreach (Piece piece in _previewQueue) { piece.ClearFromTilemap(_previewTilemap); }
        }
        private void DrawPreviwQueue() 
        {
            if (_previewTilemap == null) { return; }

            for (int i = 0; i < Mathf.Min(_previewQueue.Count, _previewQueueSize); i++) 
            {
                _previewQueue[i].Position = _previewDirection.ToVector3Int() * i * _previewSpacing;
                _previewQueue[i].SetToTilemap(_previewTilemap);
            }
        }

        private void FallDownActivePiece()
        {
            _fallDown = false;
            while (TryMoveActivePiece(Vector2Int.down)) { continue; }
            OnFallDownPiece?.Invoke();
            LockActivePiece();
        }
        private void MoveActivePiece(Vector2Int direction) 
        {
            _movementDirection = Vector2Int.zero;
            if (TryMoveActivePiece(direction)) { OnMovePiece?.Invoke(); }
        }
        private void RotateActivePiece(int rotationSense)
        {
            _rotationAngle = 0;
            _activePiece.Rotate(rotationSense);
            if (TestWallKicks(rotationSense)) { OnRotatePiece?.Invoke(); return; }

            _activePiece.Rotate(-rotationSense);
        }
        
        private bool TestWallKicks(int rotationSense)
        {
            int wallKickIndex = _activePiece.GetWallKickIndex(rotationSense);
            for (int i = 0; i < _activePiece.WallKicks.GetLength(1); i++) 
            {
                if (TryMoveActivePiece(_activePiece.WallKicks[wallKickIndex, i])) { return true; } 
            }
            return false;
        }
        private bool TryMoveActivePiece(Vector2Int direction)
        {
            Vector3Int newPosition = _activePiece.Position + (Vector3Int)direction;

            if (!IsValidPosition(newPosition)) { return false; }

            _activePiece.Position = newPosition;
            _lockTimer = 0f;

            return true;
        }

        private void CheckLines()
        {
            int row = _boardBounds.yMin;
            int streak = 0;
            while (row < _boardBounds.yMax)
            {
                if (IsLineFull(row)) 
                {
                    ClearLine(row); 
                    streak++; 
                    OnLineCleared?.Invoke(); 
                }
                else { row++; }
            }
            if (streak != 0) 
            {
                OnScore?.Invoke(streak * streak * _scorePerLine);
                OnScorePopUp?.Invoke($"+{streak * streak * _scorePerLine}");
            }
        }
        private bool IsLineFull(int row)
        {
            for (int col = _boardBounds.xMin; col < _boardBounds.xMax; col++) { if (!_boardTilemap.HasTile(new Vector3Int(col, row, 0))) { return false; } }
            return true;
        }
        private void ClearLine(int row)
        {
            for (int col = _boardBounds.xMin; col < _boardBounds.xMax; col++) { _boardTilemap.SetTile(new Vector3Int(col, row, 0), null); }
            while (row < _boardBounds.yMax)
            {
                for (int col = _boardBounds.xMin; col < _boardBounds.xMax; col++) { _boardTilemap.SetTile(new Vector3Int(col, row, 0), _boardTilemap.GetTile(new Vector3Int(col, row + 1, 0))); }
                row++;
            }
        }

        private void DrawGhostPiece() 
        {
            ClearGhostPiece();
            _ghostPiece = _activePiece.Copy();

            Vector3Int position = _activePiece.Position;
            int current = position.y;
            int bottom = -BoardSize.y / 2 - 1;

            for (int row = current; row >= bottom; row--)
            {
                position.y = row;
                if (IsValidPosition(position)) { _ghostPiece.Position = position; } else { break; }
            }

            _ghostPiece.SetToTilemap(_ghostTilemap, _ghostTile);
        }
        private void ClearGhostPiece() 
        {
            if (_ghostPiece == null) { return; } 
            _ghostPiece.ClearFromTilemap(_ghostTilemap); 
        }

        public void ClearPiece() { _activePiece.ClearFromTilemap(_boardTilemap); }
        public void SetPiece() { _activePiece.SetToTilemap(_boardTilemap); }
        public bool IsValidPosition(Vector3Int position)
        {
            Vector3Int tilePosition;
            for (int i = 0; i < _activePiece.Cells.Length; i++)
            {
                tilePosition = _activePiece.Cells[i] + position;
                if (!_boardBounds.Contains((Vector2Int)tilePosition)) { return false; }
                if (_boardTilemap.HasTile(tilePosition)) { return false; }
            }
            return true;
        }
    }
}
