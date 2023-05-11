using UnityEngine;

namespace IAM.Pong
{
	public class Paddle : MonoBehaviour
	{
        [SerializeField] protected float Speed = 10f;
        [SerializeField] protected int _rotationAngle = 15;
		public Rigidbody2D _rigidbody { get; protected set; }
        protected Vector2 _direction;
        protected float _rotation = 0;

        protected bool _isPaused = false;
        private Vector2 _velocity;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            if (_isPaused) { return; }
            UpdatePosition();
        }

        public void Play() 
        {
            _rigidbody.velocity = _velocity;
            _rigidbody.isKinematic = false;
            _isPaused = false; 
        }
        public void Pause()
        {
            _velocity = _rigidbody.velocity;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.isKinematic = true;
            _isPaused = true; 
        }

        public void MoveUp() { _direction = Vector2.up; }
        public void MoveDown() { _direction = Vector2.down; }
        public void RotateUp() { _rotation = _rotationAngle; }
        public void RotateDown() { _rotation = -_rotationAngle; }

        private void UpdatePosition()
        {
            transform.localEulerAngles = new Vector3(0, 0, _rotation);
            _rotation = 0;

            if (_direction == Vector2.zero) { return; }
            _rigidbody.AddForce(_direction * Speed);
            _direction = Vector2.zero;
        }
    }
}
