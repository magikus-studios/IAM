using UnityEngine;
using UnityEngine.Events;

namespace IAM.Asteroids
{
	public class Asteroid : MonoBehaviour
	{
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _maxTorwueForce = 30f;
        [SerializeField] private float _minSpeed = 8f;
        [SerializeField] private float _maxSpeed = 12f;
        [SerializeField] private float _minSize = 0.5f;
        [SerializeField] private float _maxSize = 1.5f;
        [SerializeField] private float _splitSize = 0.5f;
        [SerializeField] private float _destroySize = 0.5f;
        [SerializeField] private float _maxLifeTime = 30f;
        [SerializeField] private int _splitDebris = 2;
        [SerializeField] private int _impactPoints = 10;
        [SerializeField] private int _destroyPoints = 100;

        [SerializeField] private UnityEvent<int> _onImpact;
        [SerializeField] private UnityEvent<string> _onImpactLabel;
        [SerializeField] private UnityEvent<int> _onDestroy;
        [SerializeField] private UnityEvent<string> _onDestroyLabel;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private float _size;
        private float _speed;

        private float _sizeRate { get { return 1 - (_size / _maxSize); } }
        private float _speedRate { get { return (_speed / _maxSpeed); } }
        private float _pointMultiplier { get { return (1 + (_sizeRate * _sizeRate) + (_speedRate * _speedRate)) / 3; } }

        private void Awake() 
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
            transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
            transform.localScale = Vector3.one * _size;
            _rigidbody.mass = _size;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag != "Projectile") { return; }
            if ((_size * _splitSize) >= _destroySize) 
            {
                OnImpact();
                for (int i = 0; i < _splitDebris; i++) { CreateSplit(); } 
            } else { OnDestroyed(); }
            Destroy(gameObject);
        }
        private void CreateSplit() 
        {
            Vector2 SpawnPosition = this.transform.position;
            SpawnPosition += Random.insideUnitCircle * _splitSize;
            Asteroid part = Instantiate(this, SpawnPosition, transform.rotation);
            part._size = _size * _splitSize;
            part.SetTrajectory(Random.insideUnitCircle.normalized);
        }
        private void OnImpact() 
        {
            int points = (int)(_impactPoints * _pointMultiplier);
            _onImpact?.Invoke(points);
            _onImpactLabel?.Invoke($"+{points}");
        }
        private void OnDestroyed()
        {
            int points = (int)(_destroyPoints * _pointMultiplier);
            _onDestroy?.Invoke(points);
            _onDestroyLabel?.Invoke($"+{points}");
        }

        public void SetRandomSize() { _size = Random.Range(_minSize, _maxSize); }
        public void SetTrajectory(Vector3 direction) 
        {
            _speed = Random.Range(_minSpeed, _maxSpeed);
            _rigidbody.AddForce(direction * _speed);
            _rigidbody.AddTorque(Random.Range(-_maxTorwueForce, _maxTorwueForce));
            Destroy(gameObject, _maxLifeTime);
        }
    }
}
