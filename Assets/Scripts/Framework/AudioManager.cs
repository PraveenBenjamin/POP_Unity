using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//while techinally not a critical component, it can be thought of as one if developed further. hence framework
namespace POP.Framework
{


    /// <summary>
    /// Manages the Audio playback of the application
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        // Start is called before the first frame update

        public enum AudioClipType
        {
            Red,
            Green,
            Blue,
            Black,
            White,
            BackgroundMusic,
            OnResultsShown
        }




        private AudioSource _audioSourceHandle;

        [SerializeField]
        private AudioClipTypeAudioClipDic _audioClips;


        /// <summary>
        /// Play audio in a loop. Indexed by AudioClipType enum. Indexes populated in editor
        /// </summary>
        public void PlayLooping(AudioClipType type)
        {
            if (_audioClips.ContainsKey(type))
            {
                _audioSourceHandle.clip = _audioClips[type];
                _audioSourceHandle.Play();
                _audioSourceHandle.loop = true;
            }
        }

        /// <summary>
        /// Play audio once. Indexed by AudioClipType enum. Indexes populated in editor
        /// </summary>
        public void PlayOneShot(AudioClipType type)
        {
            if (_audioClips.ContainsKey(type))
                _audioSourceHandle.PlayOneShot(_audioClips[type]);
        }


        /// <summary>
        /// Set master volume, 0-1
        /// </summary>
        /// <param name="nVolume">normalized volume to set</param>
        public void SetVolume(float nVolume)
        {
            _audioSourceHandle.volume = nVolume;
        }


        protected override void InitializeSingleton()
        {
            _audioSourceHandle = GetComponent<AudioSource>();
        }
    }
}
