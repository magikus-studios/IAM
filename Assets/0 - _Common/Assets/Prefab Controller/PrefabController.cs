using UnityEngine;

namespace IAM
{
    public class PrefabController : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private GameObject _defaultPosition;
        [SerializeField] private bool _usePrefabZPosition;

        private GameObject _instance;
        private TextPopUpController _popUpController;

        private void Awake()
        {
            if (_prefab == null) { _prefab = gameObject; }
            if (_parent == null) { _parent = transform; }
            if (_defaultPosition == null) { _defaultPosition = gameObject; }
        }

        public void SpawnInstance() { GetInstance(); }
        public void SpawnInstance(string popUpText) { GetInstance(popUpText); }
        public void SpawnInstance(Vector3 position) { GetInstance(position); }


        public GameObject GetInstance()
        {
            _instance = Instantiate(_prefab, _parent);
            _instance.transform.position = _defaultPosition.transform.position;
            if (_usePrefabZPosition) { _instance.transform.position = new Vector3(_instance.transform.position.x, _instance.transform.position.y, _prefab.transform.position.z); }
            return _instance;
        }
        public GameObject GetInstance(Vector3 position)
        {
            _instance = GetInstance();
            _instance.transform.position = position;
            return _instance;
        }
        public GameObject GetInstance(string popUpText)
        {
            _instance = GetInstance();

            _popUpController = _instance.GetComponent<TextPopUpController>();
            if (_popUpController != null) { _popUpController.SetText(popUpText); }

            return _instance;
        }

    }
}