using System.Collections.Generic;
using UnityEngine;

namespace audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public List<AudioClip> masterList = new List<AudioClip>();
        public AudioSource musicAS;
        AudioSource aS;

        public List<Vector4> clipQueue = new List<Vector4>(); // Position at xyz, and w is audio clip index

        public void Update()
        {
            if (clipQueue.Count != 0)
            {
                transform.position = new Vector3(clipQueue[0].x, clipQueue[0].y, clipQueue[0].z);
                aS.PlayOneShot(masterList[(int)clipQueue[0].w]);
                clipQueue.RemoveAt(0);
            }
        }
        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
            aS = GetComponent<AudioSource>();
            musicAS = GetComponentInChildren<AudioSource>();
        }
        public void PlaySound2D(int index)
        {
            aS.PlayOneShot(masterList[index]);
        }
        public void PlaySound3D(int index, Vector3 position)//This object will play all the sounds. it can play one sound per frame at any position. The sound index is stored as the w value of a vector4 and the xyz holds the position. All audioclips have an assigned index from a master list.
        {
            clipQueue.Add(new Vector4(position.x, position.y, position.z, index));
        }
        public void ToggleMusic(bool state)
        {
            musicAS.enabled = state;
        }
        public void ToggleMusic()
        {
            musicAS.enabled = !musicAS.enabled;
        }
        public void AdjustMusicVolume(float newVolume)
        {
            musicAS.volume = newVolume;
        }
        public void AdjustSoundFXVolume(float newVolume)
        {
            aS.volume = newVolume;
        }
    }
}

