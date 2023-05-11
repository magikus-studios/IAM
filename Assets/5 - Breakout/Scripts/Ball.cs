using UnityEngine;
using UnityEngine.Events;

namespace IAM.Breakout
{
	public class Ball : MonoBehaviour
	{
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private float _speed = 50f;
        [SerializeField] private float _regularMultiplier = 1f;
        [SerializeField] private float _fastBallMultiplier = 1f;
        [SerializeField] private float _slowBallMultiplier = 1f;
        [SerializeField] private float _friction = 1f;
        [SerializeField] private float _randomBounceSensibility = 1f;
        [SerializeField] private float _randomBounceStreangth = 10f;
        [SerializeField] private float _fastBallDuration = 1f;
        [SerializeField] private float _slowBallDuration = 1f;
        [SerializeField][Range(0f, 1f)] private float _chanceOfFastBallOnBreak = 0.1f;
        [SerializeField][Range(0f, 1f)] private float _chanceOfSlowBallOnBreak = 0.2f;

        private Rigidbody2D _rigidbody;
        private bool _isPaused = true;
        private float _speedMultiplier = 1f;

        [SerializeField] private UnityEvent _onSpawn;
        [SerializeField] private UnityEvent _onBounce;
        [SerializeField] private UnityEvent _onWallBounce;
        [SerializeField] private UnityEvent _onBrickBounce;
        [SerializeField] private UnityEvent _onUBrickBounce;
        [SerializeField] private UnityEvent _onPaddleBounce;
        [SerializeField] private UnityEvent _onDeadZone;
        [SerializeField] private UnityEvent _onResetBall;
        [SerializeField] private UnityEvent _onRegularBall;
        [SerializeField] private UnityEvent _onFastBall;
        [SerializeField] private UnityEvent _onSlowBall;
        [SerializeField] private UnityEvent _onBrickBreak;

        public bool IsRegularBall { get { return _speedMultiplier == _regularMultiplier; } }
        public bool IsFastBall { get { return _speedMultiplier == _fastBallMultiplier; } }
        public bool IsSlowBall { get { return _speedMultiplier == _slowBallMultiplier; } }

        private void Awake() 
        {
            _rigidbody = GetComponent<Rigidbody2D>(); 
            if(_spawnPosition == null) { _spawnPosition = transform; }
        }
        private void FixedUpdate() 
        {
            if (_isPaused) { return; }
            _rigidbody.velocity = _speed * _speedMultiplier * _rigidbody.velocity.normalized; 
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isPaused) { return; }
            AddFrictionBounce(collision);
            _onBounce?.Invoke();
            if(collision.gameObject.name == "DeadZone") { Die(); _onDeadZone?.Invoke(); return; }
            if(collision.gameObject.name == "Wall") { _onWallBounce?.Invoke(); return; }
            if(collision.gameObject.GetComponent<Paddle>() != null) { _onPaddleBounce?.Invoke(); return; }
            if(collision.gameObject.TryGetComponent(out Brick brick)) 
            {
                if (brick.IsUnbreakable) { _onUBrickBounce?.Invoke(); }
                else { _onBrickBounce?.Invoke(); }
                return; 
            }
        }
        private void OnCollisionExit2D(Collision2D collision) 
        {
            if (_isPaused) { return; }
            AddRandomBounce(); 
        }
        
        private void AddFrictionBounce(Collision2D collision)
        {
            if (!collision.gameObject.TryGetComponent<Paddle>(out var paddle)) { return; }

            _rigidbody.AddForce(paddle.GetComponent<Rigidbody2D>().velocity * _friction);
        }
        private void SetRandomTrajectory() 
        {
            Vector2 force = Vector2.zero;
            force.x = Random.Range(-1f, 1f);
            force.y = 1f;
            _rigidbody.velocity = force.normalized * _speed;
        }
        private void AddRandomBounce() 
        {
            Vector2 force = _rigidbody.velocity;
            if (Mathf.Abs(Vector2.Dot(force, Vector2.up)) >= _randomBounceSensibility && Mathf.Abs(Vector2.Dot(force, Vector2.right)) >= _randomBounceSensibility) { return; }
            force.y += Random.Range(-_randomBounceStreangth, _randomBounceStreangth);
            force.x += Random.Range(-_randomBounceStreangth, _randomBounceStreangth);
            _rigidbody.velocity = force.normalized * _speed;
        }

        public void Spawn()
        {
            _rigidbody.velocity = Vector3.zero;
            transform.position = _spawnPosition.position;
            _isPaused = false;
            SetRandomTrajectory();
            _onSpawn?.Invoke();
        }
        public void Die()
        {
            _rigidbody.velocity = Vector3.zero;
            _isPaused = true;
            SetRegularBall();
        }
        public void SetRegularBall() 
        {
            _speedMultiplier = _regularMultiplier;
            CancelInvoke();
            if (!_isPaused) 
            {
                _onRegularBall?.Invoke();
                return;
            }
            _onResetBall?.Invoke();
        }
        public void SetFastBall()
        {
            _speedMultiplier = _fastBallMultiplier;
            _onFastBall?.Invoke();
         
            if (_fastBallDuration == 0f) { return; }
            CancelInvoke();
            Invoke(nameof(SetRegularBall), _fastBallDuration);
        }
        public void SetSlowBall() 
        {
            _speedMultiplier = _slowBallMultiplier; 
            _onSlowBall?.Invoke();

            if (_slowBallDuration == 0f) { return; }
            CancelInvoke();
            Invoke(nameof(SetRegularBall), _slowBallDuration);
        }

        public void RollPowerUpChance() 
        {
            _onBrickBreak?.Invoke();
            if (!IsRegularBall) { return; }
            float chance = Random.Range(0f, 1f);
            if (chance >= _chanceOfFastBallOnBreak + _chanceOfSlowBallOnBreak) { return; }
            if (chance <= _chanceOfFastBallOnBreak) { SetFastBall(); }
            else { SetSlowBall(); }
        }
    }
}
