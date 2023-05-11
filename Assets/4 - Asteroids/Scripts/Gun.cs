using UnityEngine;
using UnityEngine.Events;

namespace IAM.Asteroids
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private UnityEvent _onShoot;

        public void Shoot() { Shoot(transform.up); }
        public void Shoot(Vector3 direction)
        {
            Bullet bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
            bullet.Shoot(direction);
            _onShoot?.Invoke();
        }
    }
}
