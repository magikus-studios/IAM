using UnityEngine;
using UnityEngine.Events;

namespace IAM {
    public class SlideController : MonoBehaviour
    {
        private enum STATE { CLOSING, CLOSE, OPENING, OPEN }
        private enum VERTICAL_MOVEMENT { NONE, UP, DOWN }
        private float Vertical()
        {
            switch (_verticalMovement)
            {
                case VERTICAL_MOVEMENT.UP: return 1f;
                case VERTICAL_MOVEMENT.DOWN: return -1f;
                case VERTICAL_MOVEMENT.NONE: return 0f;
                default: return 0f;
            }
        }
        private enum HORIZONTAL_MOVEMENT { NONE, LEFT, RIGHT }
        private float Horizontal()
        {
            switch (_horizontalMovement)
            {
                case HORIZONTAL_MOVEMENT.RIGHT: return 1f;
                case HORIZONTAL_MOVEMENT.LEFT: return -1f;
                case HORIZONTAL_MOVEMENT.NONE: return 0f;
                default: return 0f;
            }
        }

        [SerializeField] private float _totalLengthInSeconds;
        [SerializeField] private bool _openOnAwake;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private VERTICAL_MOVEMENT _verticalMovement;
        [SerializeField] private HORIZONTAL_MOVEMENT _horizontalMovement;
        [SerializeField] private bool _useUnscaledTime;
        [Space]
        [SerializeField] private UnityEvent _onStart;
        [SerializeField] private UnityEvent _onOpenStart;
        [SerializeField] private UnityEvent _onCloseStart;
        [SerializeField] private UnityEvent _onEnd;
        [SerializeField] private UnityEvent _onOpenEnd;
        [SerializeField] private UnityEvent _onCloseEnd;

        private Vector2 _openPosition;
        private Vector2 _closePosition;
        private EventController _playOnOpen;
        private EventController _playOnClose;
        private float _progress = 1;
        private STATE _state = STATE.CLOSE;
        private RectTransform rect;
        private float DeltaTime
        {
            get
            {
                if (_useUnscaledTime) { return (Time.unscaledDeltaTime > 0.5f) ? 0.05f : Time.unscaledDeltaTime; }
                else { return (Time.deltaTime > 0.5f) ? 0.05f : Time.deltaTime; }
            }
        }

        private void Awake() 
        {
            rect = gameObject.GetComponent<RectTransform>();

            _openPosition = new Vector2((Screen.width + _offset.x) * Horizontal(), (Screen.height + _offset.y) * Vertical());
            _closePosition = new Vector2(0, 0);

            if (_openOnAwake) { OpenSlide(); }
        }
        private void Update()
        {
            if (_state == STATE.CLOSE || _state == STATE.OPEN) { return; }

            if (_state == STATE.CLOSING) { _progress += DeltaTime * (1/_totalLengthInSeconds); }
            else if(_state == STATE.OPENING) { _progress -= DeltaTime * (1/_totalLengthInSeconds); }

            if (_progress >= 1)
            { 
                _progress = 1;
                if(_playOnClose != null) { _playOnClose.Play(); }
                _playOnClose = null;
                _state = STATE.CLOSE;
                _onEnd?.Invoke();
                _onCloseEnd?.Invoke();
            } 
            else if (_progress <= 0) 
            { 
                _progress = 0;
                if (_playOnOpen != null) { _playOnOpen.Play(); }
                _playOnOpen = null;
                _state = STATE.OPEN;
                _onEnd?.Invoke();
                _onOpenEnd?.Invoke();
            }

            rect.anchoredPosition = Vector2.Lerp(_openPosition, _closePosition, _progress);
        }

        public void OpenSlide() 
        {
            if (_state == STATE.OPEN) { return; }
            _state = STATE.OPENING;
            _onStart?.Invoke();
            _onOpenStart?.Invoke();
        }
        public void CloseSlide() 
        {
            if (_state == STATE.CLOSE) { return; }
            _state = STATE.CLOSING;
            _onStart?.Invoke();
            _onCloseStart?.Invoke();
        }
        public void OpenSlide(EventController eventController) 
        {
            _playOnOpen = eventController;
            OpenSlide();
        }
        public void CloseSlide(EventController eventController) 
        {
            _playOnClose = eventController;
            CloseSlide();
        }
    }
}