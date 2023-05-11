using System;
using UnityEngine;
using UnityEngine.UI;

namespace IAM
{
    [CreateAssetMenu(menuName = "IAM/Sound Master Asset")]
    public class SoundMasterAsset : ScriptableObject
    {
        [SerializeField] [Range(0f, 1f)] private float _volume = 1f;
        [SerializeField] [Range(-3f, 3f)] private float _pitch = 1f;
        [field: SerializeField] public bool IsMute { get; private set; }

        private Action _onChange;

        public float Volume { get { return (IsMute) ? 0f : _volume; } private set { _volume = value; } }
        public float Pitch { get { return _pitch; } private set { _pitch = value; } }

        public void OnChangeSubscribe(Action action) { _onChange += action; }
        public void OnChangeUnsubscribe(Action action) { _onChange -= action; }

        public void Mute() { IsMute = true; _onChange?.Invoke(); }
        public void Unmute() { IsMute = false; _onChange?.Invoke(); }
        public void ToggleMute() { IsMute = !IsMute; _onChange?.Invoke(); }

        public void SetVolume(float amount)
        {
            Volume = Mathf.Clamp01(amount);
            _onChange?.Invoke();
        }
        public void RaiseVolume(float amount) { SetVolume(Volume + Mathf.Abs(amount)); }
        public void LowerVolume(float amount) { SetVolume(Volume - Mathf.Abs(amount)); }
        public void SetVolume(int amount) { SetVolume((float)amount / 100); }
        public void RaiseVolume(int amount) { RaiseVolume((float)amount / 100); }
        public void LowerVolume(int amount) { LowerVolume((float)amount / 100); }

        public void SetPitch(float amount)
        {
            Pitch = Mathf.Clamp(amount, -3f, 3f);
            _onChange?.Invoke();
        }
        public void RaisePitch(float amount) { SetPitch(Pitch + Mathf.Abs(amount)); }
        public void LowerPitch(float amount) { SetPitch(Pitch - Mathf.Abs(amount)); }
        public void SetPitch(int amount) { SetPitch((float)amount / 100); }
        public void RaisePitch(int amount) { RaisePitch((float)amount / 100); }
        public void LowerPitch(int amount) { LowerPitch((float)amount / 100); }

        public void SetVolumeValueToSlider(Slider slider) { slider.value = _volume; }
        public void SetPitchValueToSlider(Slider slider) { slider.value = _pitch; }
        public void SetMuteValueToGameObject(GameObject gameObject) { gameObject.SetActive(IsMute); }
    }
}