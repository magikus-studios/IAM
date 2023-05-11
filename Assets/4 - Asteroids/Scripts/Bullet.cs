using UnityEngine;

namespace IAM.Asteroids
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private float _speed = 500f;
		[SerializeField] private float _maxLifeTime = 10f;

		private Rigidbody2D _rigidbody;

		private void Awake() { _rigidbody = GetComponent<Rigidbody2D>(); }
        private void OnCollisionEnter2D(Collision2D collision) { Destroy(gameObject); }

        public void Shoot(Vector2 direction)
		{
			_rigidbody.AddForce(direction * _speed);
			Destroy(gameObject, _maxLifeTime);
		}
	}
}
