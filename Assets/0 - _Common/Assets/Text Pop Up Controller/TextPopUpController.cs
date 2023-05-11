using TMPro;
using UnityEngine;

namespace IAM
{
    public class TextPopUpController : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _lifespan;
        [SerializeField] private float _distance;

        private float _timer = 0;
        private float _progress = 0;
        private TMP_Text _component;
        private Vector3 _initialPosition;
        private Vector3 _direction;

        private void Awake()
        {
            _component = gameObject.GetComponent<TMP_Text>();
        }
        private void Start()
        {
            _initialPosition = transform.position;
            _direction = transform.position + (Vector3.up * _distance) - _initialPosition;
        }

        private void Update()
        {
            _timer += Time.deltaTime * _speed;
            if (_timer >= _lifespan) { Destroy(gameObject); }

            _progress = _timer / _lifespan;

            transform.position = _initialPosition + (_direction * _progress);
            if (_component != null)
            {
                _component.color = new Color(_component.color.r, _component.color.g, _component.color.b, 1 - _progress);
            }
        }

        public void SetText(string text) { if (_component != null) { _component.text = text; } }
    }
}