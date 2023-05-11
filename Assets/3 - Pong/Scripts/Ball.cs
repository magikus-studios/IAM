using UnityEngine;
using UnityEngine.Events;

namespace IAM.Pong
{
	public class Ball : MonoBehaviour
	{
        [SerializeField] private float Speed = 250f;
        [SerializeField] private float _randomBounceAngle = 0.1f;

        [SerializeField] private UnityEvent OnScore;
        [SerializeField] private UnityEvent OnBounce;
        [SerializeField] private UnityEvent OnPaddleHit;
        [SerializeField] private UnityEvent OnRespawn;

        private Rigidbody2D _rigidbody;
        private Vector3 _spawnPosition;
        private Vector2 _velocity;
        
        private void Awake() 
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spawnPosition = gameObject.transform.position;
        }
        
        private void OnCollisionEnter2D(Collision2D collision) 
        {
            ScoreSurface score = collision.gameObject.GetComponent<ScoreSurface>();
            if (score != null) { Score(score); return; }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            AddBounceForce(collision);
        }

        public void Spawn()
        {
            Play();
            _rigidbody.position = _spawnPosition;
            _rigidbody.velocity = Vector2.zero;

            Vector2 force = new Vector2();
            force.x = Random.value < 0.5f ? -1.0f : 1.0f;
            force.y = Random.value < 0.5f ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f, 1.0f);
            _rigidbody.AddForce(force.normalized * Speed);

            OnRespawn?.Invoke();
        }
        public void Spawn(Vector2 position, bool toLeft)
        {
            Play();
            _rigidbody.position = position;
            _rigidbody.velocity = Vector2.zero;

            Vector2 force = new Vector2();
            force.x = (toLeft) ? -1 : 1;
            force.y = Random.value < 0.5f ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f, 1.0f);
            _rigidbody.AddForce(force.normalized * Speed);

            OnRespawn?.Invoke();
        }
        public void Move(Vector2 position)
        {
            _rigidbody.position = position;
        }

        public void Score(ScoreSurface score) 
        {
            Pause();

            OnScore?.Invoke();
            score.OnScore?.Invoke(score.Points);
            score.OnScoreLabel?.Invoke($"+{score.Points}");
        }

        public void Play() 
        {
            _rigidbody.velocity = _velocity;
            _rigidbody.isKinematic = false; 
        }
        public void Pause() 
        {
            _velocity = _rigidbody.velocity;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.isKinematic = true; 
        }

        private void AddForce(Vector2 force) { _rigidbody.AddForce(force, ForceMode2D.Impulse); }       
        private void AddBounceForce(Collision2D collision)
        {
            Paddle paddle = collision.gameObject.GetComponent<Paddle>();
            if (paddle != null) { OnPaddleHit?.Invoke(); }

            BouncySurface bounce = collision.gameObject.GetComponent<BouncySurface>();
            if (bounce == null) { return; }

            Vector2 randomVector = Random.insideUnitCircle * _randomBounceAngle;
            Vector2 force = _rigidbody.velocity + randomVector;

            AddForce(force.normalized * bounce.BounceStreangth);

            if (paddle == null) { OnBounce?.Invoke(); }
        }
    }
}
