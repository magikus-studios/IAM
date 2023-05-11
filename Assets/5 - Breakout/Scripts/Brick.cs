using UnityEngine;
using UnityEngine.Events;

namespace IAM.Breakout
{
	public class Brick : MonoBehaviour
	{
        [field: SerializeField] public int Health { get; private set; }
        [SerializeField] private Sprite unbreakableState;
        [SerializeField] private Sprite[] states;

        [Header("Settings")]
        [SerializeField] private bool _unbreakable;
        [SerializeField] private bool _randomHealth;
        [SerializeField] private bool _randomUnbreakable;
        [SerializeField][Min(0f)] private float _timeToReSpawn;
        [SerializeField][Min(0f)] private float _timeToBreakWhenUnbreakable;
        [SerializeField][Min(0)] private int _hitScore;
        [SerializeField][Min(0)] private int _breakScore;


        [Header("Events")]
        [SerializeField] private UnityEvent _onBrickSpawn;
        [SerializeField] private UnityEvent _onBreakableBrickSpawn;
        [SerializeField] private UnityEvent _onUnbreakableBrickSpawn;
        [SerializeField] private UnityEvent _onBrickHit;
        [SerializeField] private UnityEvent _onBreakableBrickHit;
        [SerializeField] private UnityEvent _onUnbreakableBrickHit;
        [SerializeField] private UnityEvent _onBrickBreak;
        [SerializeField] private UnityEvent<int> _onScoreHit;
        [SerializeField] private UnityEvent<string> _onScoreHitLabel;
        [SerializeField] private UnityEvent<int> _onScoreBreak;
        [SerializeField] private UnityEvent<string> _onScoreBreakLabel;


        private int _health;
        private bool _broken = false;
        private SpriteRenderer _spriteRenderer;
        
        public bool IsUnbreakable { get { return _unbreakable; } }
        public bool IsBroken { get { return _broken; } }

        private void OnValidate() { Health = Mathf.Clamp(Health, 0, states.Length); }
        private void Awake() { _spriteRenderer = GetComponent<SpriteRenderer>(); }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.TryGetComponent<Ball>(out var ball)) { return; }

            if (ball.IsRegularBall) { Hit(1); }
            else if (ball.IsSlowBall) { Hit(2); }
            else if (ball.IsFastBall) { Hit(3); }
        }
        
        public void Spawn() 
        {
            _broken = false;
            if (_randomHealth) { Randomize(); }
            else { _health = Health; }
            _onBrickSpawn?.Invoke();
            
            if (_unbreakable) 
            {
                _onUnbreakableBrickSpawn?.Invoke();
                _spriteRenderer.sprite = unbreakableState;
                if (_timeToBreakWhenUnbreakable != 0f) { Invoke(nameof(Spawn), Random.Range(_timeToBreakWhenUnbreakable * 0.5f, _timeToBreakWhenUnbreakable)); }
                gameObject.SetActive(true);
                return; 
            }
            
            _onBreakableBrickSpawn?.Invoke();
            _spriteRenderer.sprite = states[_health - 1];
            gameObject.SetActive(true);
        }
        private void Hit(int amount)
        {
            _onBrickHit?.Invoke();
            if (_unbreakable) { _onUnbreakableBrickHit?.Invoke(); return; }

            if (amount >= _health) { amount = _health; }
            _health -= amount;
            
            if (_health <= 0) { Break(_hitScore * (amount - 1)); return; }
            
            _onBreakableBrickHit?.Invoke();

            _onScoreHit?.Invoke(_hitScore * amount);
            _onScoreHitLabel?.Invoke($"+{_hitScore * amount}");
            _spriteRenderer.sprite = states[_health - 1];
        }
        private void Break(int scoreBonus)
        {
            _broken = true;
            _onBrickBreak?.Invoke();
            _onScoreBreak?.Invoke(_breakScore + scoreBonus);
            _onScoreBreakLabel?.Invoke($"+{_breakScore + scoreBonus}");
            gameObject.SetActive(false);
            if (_timeToReSpawn != 0f) { Invoke(nameof(Spawn), Random.Range(_timeToReSpawn * 0.5f, _timeToReSpawn)); }
        }
        private void Randomize()
        {
            _unbreakable = false;
            _health = Random.Range((_randomUnbreakable) ? 0 : 1, states.Length + 1);
            if (_health == 0) { _unbreakable = true; }
        }
    }
}
