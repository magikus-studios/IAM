using System.Collections.Generic;
using UnityEngine;

namespace IAM.Snake
{
	public class Snake : MonoBehaviour
	{
        [Header("Settings")]
        [SerializeField] private int _initialSize = 2;
        [SerializeField] private Transform _bodyPrefab;
        [SerializeField] private Transform _tailPrefab;

        private Vector2Int _direction = Vector2Int.right;
        private Vector2Int _nextMoveDirection = Vector2Int.right;
        private List<Transform> _bodyList = new List<Transform>();
        private Vector3 _startPosition;

        public void MoveUp() { SetNextMoveDirection(Vector2Int.up); }
        public void MoveDown() { SetNextMoveDirection(Vector2Int.down); }
        public void MoveLeft() { SetNextMoveDirection(Vector2Int.left); }
        public void MoveRight() { SetNextMoveDirection(Vector2Int.right); }
        public void Move()
        {
            _direction = _nextMoveDirection;
            SetPosition(_direction);
        }
        public void Grow()
        {
            Transform bodyPart = Instantiate(_bodyPrefab);
            bodyPart.transform.parent = transform.parent;
            bodyPart.position = _bodyList[_bodyList.Count - 1].position;
            _bodyList.Insert(_bodyList.Count - 1, bodyPart);
        }
        public void Restart()
        {
            int size = Mathf.Max(_bodyList.Count, _initialSize);
            for (int i = 1; i < _bodyList.Count; i++) { Destroy(_bodyList[i].gameObject); }
            transform.position = _startPosition;
            _bodyList.Clear();
            _bodyList.Add(transform);
            GrowTail();
            for (int i = 1; i < size - 1; i++) { Grow(); }

            _direction = Vector2Int.right;
            SetNextMoveDirection(Vector2Int.right);
            SetPosition(_direction);
        }

        public bool CheckSelfCollision() 
        {
            bool result = false;
            for (int i = 1; i < _bodyList.Count; i++)
            {
                if (transform.position == _bodyList[i].position) { result = true; break; }
            }
            return result; 
        }
        public List<Vector2Int> GetSnakePositions() 
        {
            List<Vector2Int> positionList = new List<Vector2Int>();
            positionList.Add(transform.position.ToVector2Int());
            foreach (Transform bodyPart in _bodyList) { positionList.Add(bodyPart.position.ToVector2Int()); }
            return positionList;
        }

        private void GrowTail()
        {
            Transform tailPart = Instantiate(_tailPrefab);
            tailPart.transform.parent = transform.parent;
            tailPart.position = _bodyList[_bodyList.Count - 1].position;
            _bodyList.Add(tailPart);
        }
        private void SetNextMoveDirection(Vector2Int direction) 
        {
            if (_direction == -direction) { return; }
            _nextMoveDirection = direction;
            transform.rotation = Quaternion.AngleAxis(GetRotationAngle(), Vector3.forward);
        }
        private void SetPosition(Vector2Int direction)
        {
            for (int i = _bodyList.Count - 1; i > 0; i--) { _bodyList[i].position = _bodyList[i - 1].position; }
            transform.position = new Vector3(Mathf.Round(transform.position.x) + direction.x, Mathf.Round(transform.position.y) + direction.y, 0f);
        }
        public float GetRotationAngle() { return Mathf.Atan2(_nextMoveDirection.y, _nextMoveDirection.x) * Mathf.Rad2Deg; }
    }
}
