using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace POP.Framework
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        // Start is called before the first frame update

        public enum AudioClipType
        {
            BackgroundMusic
        }




        private AudioSource _audioSourceHandle;

        [SerializeField]
        private AudioClipTypeAudioClipDic _audioClips;

        public void PlayLooping(AudioClipType type)
        {
            if (_audioClips.ContainsKey(type))
            {
                _audioSourceHandle.clip = _audioClips[type];
                _audioSourceHandle.Play();
            }
        }

        public void PlayOneShot(AudioClipType type)
        {
            if (_audioClips.ContainsKey(type))
                _audioSourceHandle.PlayOneShot(_audioClips[type]);
        }


        public void SetVolume(float nVolume)
        {
            _audioSourceHandle.volume = nVolume;
        }


        protected override void InitializeSingleton()
        {
            _audioSourceHandle = GetComponent<AudioSource>();
        }

        protected override void OnDestroySingleton()
        {
            //throw new System.NotImplementedException();
        }
    }
}
