using UnityEngine;

namespace IAM
{
    public class TextPopUpManager : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private GameObject _defaultPosition;

        private void Awake()
        {
            if (_prefab == null) { _prefab = gameObject; }
            if (_parent == null) { _parent = transform; }
            if (_defaultPosition == null) { _defaultPosition = gameObject; }
        }

        public void SpawnInstance() { GetInstance(); }
        public void SpawnInstance(Vector3 position) { GetInstance(position); }


        public GameObject GetInstance()
        {
            GameObject instance = Instantiate(_prefab, _parent);
            instance.transform.position = _defaultPosition.transform.position;
            return instance;
        }
        public GameObject GetInstance(Vector3 position)
        {
            GameObject instance = Instantiate(_prefab, _parent);
            instance.transform.position = position;
            return instance;
        }
    }
}