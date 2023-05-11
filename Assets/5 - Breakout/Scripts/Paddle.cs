using UnityEngine;

namespace IAM.Breakout 
{
	public class Paddle : MonoBehaviour
	{
        [SerializeField] private float _speed = 30f;
        [SerializeField] private int _rotationAngle = 30;
        
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private int _rotation;

        private void Awake() { _rigidbody = GetComponent<Rigidbody2D>(); }
        private void FixedUpdate()
        {
            UpdateVelocity();
            UpdateRotation();
        }
        private void UpdateVelocity() 
        {
            if (_direction == Vector2.zero) { return; }
            _rigidbody.AddForce(_direction * _speed);
            _direction = Vector2.zero;
        }
        private void UpdateRotation()
        {
            transform.localEulerAngles = new Vector3(0, 0, _rotationAngle * _rotation);
            _rotation = 0;
        }

        public void RotateLeft() { _rotation = -1; }
        public void RotateRight() { _rotation = 1; }
        public void MoveLeft() { _direction = Vector2.left; }
        public void MoveRight() { _direction = Vector2.right; }
    }
}
