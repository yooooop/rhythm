using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> sources;

    // Start is called before the first frame update
    public void AddSound(AudioClip sound)
    {
        for (int i = 0; i < sources.Count; i++)
        {
            if (sources[i].isPlaying && sources[i].clip == sound)
            {
                sources[i].Stop();
                sources[i].Play();
                return;
            }
            else if (!sources[i].isPlaying)
            {
                sources[i].clip = sound;
                sources[i].Play();
                return;
            }
        }
        AudioSource newSource = this.gameObject.AddComponent<AudioSource>();
        newSource.clip = sound;
        newSource.volume = 0.5f;
        newSource.Play();
        sources.Add(newSource);
    }
}
