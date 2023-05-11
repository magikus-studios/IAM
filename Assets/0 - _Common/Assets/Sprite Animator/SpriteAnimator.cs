using UnityEngine;

namespace IAM
{
    [AddComponentMenu("IAM/Sprite Animator")]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteAnimatorAsset CurrentAnimation;
        [SerializeField] private float PlaybackSpeed;
        [SerializeField] public bool PlayOnAwake = true;
        [SerializeField] public bool PlayOnce = false;

        private SpriteRenderer _renderer;
        private int _animationFrame = 0;
        private float _time = 0f;
        private bool _playing = false;
        private EventController _eventToPlayWhenFinishedLoop;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (PlayOnAwake) { Play(); }
        }

        private void Update()
        {
            if (!_playing || CurrentAnimation == null) { return; }

            _time += Time.deltaTime;
            if (_time >= GetFrameRate()) 
            {
                _time = 0f;
                Step();

                if (PlayOnce) { Pause(); }
            }
        }

        private void Step() 
        {
            if (CurrentAnimation.RandomOrder) { SetRandomFrame(); }
            else { _animationFrame++; }
           
            if (_animationFrame >= CurrentAnimation.Frames.Length) 
            {
                if(_eventToPlayWhenFinishedLoop != null) 
                {
                    _eventToPlayWhenFinishedLoop.Play();
                    _eventToPlayWhenFinishedLoop = null;
                }
                if (CurrentAnimation.IsLoop) { _animationFrame = 0; }
            }
            if (_animationFrame >= 0 && _animationFrame < CurrentAnimation.Frames.Length) 
            {
                _renderer.sprite = CurrentAnimation.Frames[_animationFrame]; 
            }
        }
        private float GetFrameRate() 
        {
            if(CurrentAnimation.PlaybackDistortion != 0) { return CurrentAnimation.FrameRate * PlaybackSpeed + Random.Range(0, CurrentAnimation.PlaybackDistortion); }

            return CurrentAnimation.FrameRate * PlaybackSpeed;
        }

        public void Play() { _playing = true; }
        public void Pause() { _playing = false; }
        public void Restart() { _animationFrame = -1; }
        public void SetFrame(int frame) { _animationFrame = Mathf.Clamp(frame, 0, CurrentAnimation.Frames.Length - 1); }
        public void SetRandomFrame()
        {
            if (CurrentAnimation.AvoidRepeatingLastSprite) { _animationFrame = CurrentAnimation.Frames.RandomIndex(_animationFrame); }
            else { _animationFrame = CurrentAnimation.Frames.RandomIndex(); }
        }
        public void SetAnimation(SpriteAnimatorAsset spriteAnimation) 
        { 
            if (spriteAnimation == null) { return; }
            Restart();
            CurrentAnimation = spriteAnimation;
            Step();
            Play();
        }
        public void OnFinishedLoop(EventController eventController) { _eventToPlayWhenFinishedLoop = eventController; }
    }
}
