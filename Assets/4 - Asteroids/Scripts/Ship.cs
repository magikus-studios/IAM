using UnityEngine;
using UnityEngine.Events;

namespace IAM.Asteroids 
{
	public class Ship : MonoBehaviour
	{
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _turnSpeed = 1f;
        [SerializeField] private float _inmunityTime = 1f;
        [SerializeField] private GameObject _visuals;

        [SerializeField] private UnityEvent _onThrustOn;
        [SerializeField] private UnityEvent _onThrustOff;
        [SerializeField] private UnityEvent _onTurnOn;
        [SerializeField] private UnityEvent _onTurnOff;
        [SerializeField] private UnityEvent _onCrash;
        [SerializeField] private UnityEvent _onSpawn;
        [SerializeField] private UnityEvent _onInmunityStart;
        [SerializeField] private UnityEvent _onInmunityEnd;

        private Rigidbody2D _rigidbody;
        private bool _inmune = true;
        private bool _thrusting;
        private float _turnDirection;

        private bool _nextThrusting;
        private float _nextTurnDirection;

        private void Awake() { _rigidbody = GetComponent<Rigidbody2D>(); }
        private void FixedUpdate()
        {
            if (_turnDirection != _nextTurnDirection) 
            {
                if(_turnDirection == 0f) { _onTurnOn?.Invoke(); }
                if(_nextTurnDirection == 0f) { _onTurnOff?.Invoke(); }
                _turnDirection = _nextTurnDirection;
            }
            if (_thrusting != _nextThrusting) 
            {
                if (_thrusting) { _onThrustOff?.Invoke(); }
                else { _onThrustOn?.Invoke(); }
                _thrusting = _nextThrusting;
            }

            if (_thrusting)  { _rigidbody.AddForce(transform.up * _speed); }
            if(_turnDirection != 0) { _rigidbody.AddTorque(_turnDirection * _turnSpeed); }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_inmune) { return; }
            if (collision.gameObject.tag != "Obstacle") { return; }
            Die();
            _onCrash?.Invoke();

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0f;

            _visuals.SetActive(false);
        }
        public void Spawn() 
        {
            //transform.position = Vector3.zero;
            _visuals.SetActive(true);
            _onSpawn?.Invoke();
            StartInmunity(_inmunityTime);
        }
        public void Die() 
        {
            TurnOff();
            ThrustOff();
        }

        public void StartInmunity(float timeSpan) 
        {
            StartInmunity();
            Invoke(nameof(EndInmunity), timeSpan);
        }
        public void StartInmunity()
        {
            _inmune = true;
            _onInmunityStart?.Invoke();
        }
        public void EndInmunity()
        {
            _inmune = false;
            _onInmunityEnd?.Invoke();
        }

        public void TurnLeft() { _nextTurnDirection = 1f; }
        public void TurnRight() { _nextTurnDirection = -1f; }
        public void TurnOff() { _nextTurnDirection = 0f; }
        public void ThrustOn() {  _nextThrusting = true; }
        public void ThrustOff() { _nextThrusting = false; }
    }
}
