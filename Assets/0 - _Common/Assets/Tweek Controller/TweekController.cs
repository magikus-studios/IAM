using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IAM {
    public class TweekController : MonoBehaviour
    {
        [SerializeField] private float _origin = 0f;
        [SerializeField] private float _target = 1f;
        [SerializeField] private float _totalLengthInSeconds = 1f;
        [SerializeField] private bool _useUnscaledTime;

        [Space]
        [SerializeField] private UnityEvent<float> _tweekEvent;

        private float _timer = 0f;
        private bool _tweeking = false;
        private Action _onFinish;
        private float DeltaTime 
        {
            get
            {
                if (_useUnscaledTime) { return (Time.unscaledDeltaTime > 0.5f) ? 0.05f : Time.unscaledDeltaTime; }
                else { return (Time.deltaTime > 0.5f) ? 0.05f : Time.deltaTime; }
            }
        }

        public void Play()
        {
            if (_tweeking) { return; }

            _timer = 0f;
            _tweeking = true;
        }
        public void Play(EventController onFinish)
        {
            if (_tweeking) { return; }

            _onFinish = () => { onFinish.Play(); };

            _timer = 0f;
            _tweeking = true;
        }

        private void Update()
        {
            if (!_tweeking) { return; }

            _timer += DeltaTime * (1 / _totalLengthInSeconds);

            if (_timer >= 1f)
            {
                _timer = 1f;
                _tweeking = false;
                _onFinish?.Invoke();
            }

            _tweekEvent?.Invoke(Mathf.Lerp(_origin, _target, _timer));
        }
    }
}
