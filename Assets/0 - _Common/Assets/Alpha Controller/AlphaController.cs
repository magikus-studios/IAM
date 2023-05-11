using UnityEngine;
using UnityEngine.UI;

public class AlphaController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Graphic _graphic;
    [SerializeField] private float _speed;
    [SerializeField][Range(0f, 1f)] private float _startAlpha = 0f;
    [SerializeField][Range(0f, 1f)] private float _endAlpha = 1f;
    [SerializeField] private bool _playBack;
    [SerializeField] private bool _isLoop;
    [SerializeField] private bool _playOnAwake;
    [SerializeField] private bool _useUnscaledTime = true;

    private float _timer = 0f;
    private bool _playing = false;
    private bool _playingForward = true;

    private void Awake() 
    {
        if(_graphic == null) { _graphic = GetComponent<Graphic>(); }
        if(_sprite == null) { _sprite = GetComponent<SpriteRenderer>(); }
        if (_playOnAwake) { Play(); }
    }

    private void Update()
    {
        if (!_playing) { return; }
        UpdateTimer();
        UpdateColor();
    }

    private void UpdateTimer() 
    {
        if (_playingForward)
        {
            _timer += ((_useUnscaledTime) ? Time.unscaledDeltaTime : Time.deltaTime) * _speed;
            if (_timer >= 1f)
            {
                if (_playBack)
                {
                    _playingForward = false;
                    _timer = 1f;
                }
                else
                {
                    if (!_isLoop) 
                    {
                        Pause();
                        return;
                    }
                    _timer = 0f;
                }
            }
        }
        else
        {
            _timer -= ((_useUnscaledTime) ? Time.unscaledDeltaTime : Time.deltaTime) * _speed;
            if (_timer <= 0f)
            {
                _playingForward = true;
                _timer = 0f;
                if (!_isLoop) { Pause(); }
            }
        }
    }
    private void UpdateColor() 
    {
        if (_graphic != null) { _graphic.color = new Color(_graphic.color.r, _graphic.color.g, _graphic.color.b, Mathf.Lerp(_startAlpha, _endAlpha, _timer)); }
        if (_sprite != null) { _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, Mathf.Lerp(_startAlpha, _endAlpha, _timer)); }
    }

    public void Play() { _playing = true; }
    public void Pause() { _playing = false; }
    public void Restart() 
    {
        _timer = 0f;
        _playingForward = true;
        Play();
    }
    public void Stop()
    {
        Pause();
        _timer = 1f;
        UpdateColor();
        _timer = 0f;
        _playingForward = true;
    }
}
