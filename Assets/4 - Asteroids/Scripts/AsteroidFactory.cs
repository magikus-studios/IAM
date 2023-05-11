using UnityEngine;
using UnityEngine.Events;

namespace IAM.Asteroids
{
	public class AsteroidFactory : MonoBehaviour
	{
        [SerializeField] private float _spawnRate = 2f;
        [SerializeField] private int _spawnAmount = 1;
        [SerializeField] private float _spawnDistance = 15f;
        [SerializeField] private float _trajectoryVariance = 15f;
        [SerializeField] private Asteroid _asteroidPrefab;

        [SerializeField] private UnityEvent _onSpawn;

        private bool _isPaused = true;

        public void Play() { _isPaused = false; }
        public void Pause() { _isPaused = true; }

        private void Start()
        {
            InvokeRepeating(nameof(Spawn), _spawnRate, _spawnRate);
        }

        private void Spawn() 
        {
            if (_isPaused) { return; }
            for (int i = 0; i < _spawnAmount; i++) 
            {
                Vector3 spawnDirection = Random.insideUnitCircle.normalized * _spawnDistance;
                Vector3 spawnPosition = transform.position + spawnDirection;

                float variance = Random.Range(-_trajectoryVariance, _trajectoryVariance);
                Quaternion spawnRotation = Quaternion.AngleAxis(variance, Vector3.forward);

                Asteroid asteroid = Instantiate(_asteroidPrefab, spawnPosition, spawnRotation);
                asteroid.transform.parent = transform;
                asteroid.gameObject.SetActive(true);
                asteroid.SetRandomSize();
                asteroid.SetTrajectory(spawnRotation * -spawnDirection);
                _onSpawn?.Invoke();
            }
        }
    }
}
