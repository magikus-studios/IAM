using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

namespace IAM
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private SoundMasterAsset MasterVolumeAsset;
        [SerializeField] private SoundLibraryAsset SoundLibraryAsset;
        [SerializeField][Range(0f, 1f)] private float _volume = 1f;

        [Header("Events")]
        public UnityEvent<string> OnPlay;
        public UnityEvent<string> OnPause;
        public UnityEvent<string> OnUnpause;
        public UnityEvent<string> OnStop;

        private GameObject _sources;

        private void Awake()
        {
            if (SoundLibraryAsset == null) { return; }
            if (_sources == null)
            {
                _sources = new GameObject($"{gameObject.name} Audio Sources");
                _sources.transform.parent = transform;
                foreach (Sound sound in SoundLibraryAsset.Library) { sound.CreateAudioSource(_sources); }
            }
            UpdateSounds();
        }
        private void OnEnable() { MasterVolumeAsset.OnChangeSubscribe(UpdateSounds); }
        private void OnDisable() { MasterVolumeAsset.OnChangeUnsubscribe(UpdateSounds); }

        private void UpdateSounds() 
        {
            if (SoundLibraryAsset == null) { return; }
            foreach (Sound sound in SoundLibraryAsset.Library) { sound.UpdateSettings(MasterVolumeAsset, SoundLibraryAsset, _volume); }
        }

        public void SetVolume(float volume) 
        {
            _volume = Mathf.Clamp01(volume);
            UpdateSounds();
        }

        public void Play(Sound sound) 
        {
            if (sound == null) { return; } 
            StartPlaying(sound); 
            OnPlay?.Invoke(sound.Name); 
        }
        public void Pause(Sound sound) 
        {
            if (sound == null) { return; } 
            StopPlaying(sound, true); 
            OnPause?.Invoke(sound.Name); 
        }
        public void Unpause(Sound sound)
        {
            if (sound == null) { return; }
            StartPlaying(sound, true); 
            OnUnpause?.Invoke(sound.Name);
        }
        public void Stop(Sound sound) 
        {
            if (sound == null) { return; } 
            StopPlaying(sound); 
            OnStop?.Invoke(sound.Name);
        }

        public void Play(string sound) { Play(GetSound(sound)); }
        public void Pause(string sound) { Pause(GetSound(sound)); }
        public void Unpause(string sound) { Unpause(GetSound(sound)); }
        public void Stop(string sound) { Stop(GetSound(sound)); }

        public void Play(int index) { Play(GetSound(index)); }
        public void Pause(int index) { Pause(GetSound(index)); }
        public void Unpause(int index) { Unpause(GetSound(index)); }
        public void Stop(int index) { Stop(GetSound(index)); }

        private void StartPlaying(Sound sound, bool unPause = false)
        {
            if (SoundLibraryAsset == null) { return; }
            if (sound == null) { return; }

            if (!unPause)
            {
                if (sound.CreateNewInstanceOnEachPlay)
                {
                    AudioSource newSource = sound.CopyAudioSource(_sources);
                    newSource.Play();
                    Destroy(newSource.gameObject, newSource.clip.length);
                    return;
                }

                sound.Play();
            }
            else { sound.Unpause(); }
        }
        private void StopPlaying(Sound sound, bool pause = false)
        {
            if (SoundLibraryAsset == null) { return; }
            if (sound == null) { return; }

            if (!pause) { sound.Stop(); }
            else { sound.Pause(); }
        }

        private Sound GetSound(string name)
        {
            Sound sound = SoundLibraryAsset.Library.Find(s => s.Name == name);
            if (sound == null) { print("The sound " + name + " is not on this manager list."); }
            return sound;
        }
        private Sound GetSound(int index)
        {
            index = Mathf.Abs(index);
            Sound sound = (index >= SoundLibraryAsset.Library.Count) ? null : SoundLibraryAsset.Library[index];
            if (sound == null) { print("The sound of index " + index + " is not on this manager list."); }
            return sound;
        }
    }
}