using System;
using UnityEngine;

namespace IAM
{
    [Serializable] public class Sound
    {
        [SerializeField] private AudioClip Clip;
        public string Name;
        [Range(0f, 2f)] public float Volume = 1;
        [Range(0.1f, 3f)] public float Pitch = 1;
        public bool Loop = false;
        public bool CreateNewInstanceOnEachPlay = false;
        
        private AudioSource _source;
        [HideInInspector] public bool SkipNextStop = false;

        public bool IsPlaying { get { return _source.isPlaying; } }
        public float Left { get { return Clip.length - _source.time; } }
        public float Progress { get { return _source.time / Clip.length; } }

        public void Play() { _source.Play(); }
        public void Stop()
        {
            if (SkipNextStop) { SkipNextStop = false; return; }
            _source.Stop();
        }
        public void Pause() { _source.Pause(); }
        public void Unpause() { _source.UnPause(); }

        public void CreateAudioSource(GameObject owner)
        {
            _source = owner.AddComponent<AudioSource>();
            _source.clip = Clip;
            _source.volume = Volume;
            _source.pitch = Pitch;
            _source.loop = Loop;
        }
        public void UpdateSettings(SoundMasterAsset master, SoundLibraryAsset library, float managerVolume) 
        {
            _source.volume = Volume * master.Volume * library.Volume * managerVolume;
            _source.pitch = Pitch * master.Pitch * library.Pitch;
            _source.loop = Loop;
        }
        public AudioSource CopyAudioSource(GameObject owner) 
        {
            GameObject source = new GameObject($"{Name} Audio Source");
            source.transform.parent = owner.transform;
            AudioSource audioSource = source.AddComponent<AudioSource>();
            audioSource.clip = _source.clip;
            audioSource.volume = _source.volume;
            audioSource.pitch = _source.pitch;
            audioSource.loop = _source.loop;
            return audioSource;
        }
    }
}