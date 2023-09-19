using UnityEngine;

public class Conductor : MonoBehaviour
{
    [SerializeField]
    private float BPM;
    [SerializeField]
    private float offset;
    [SerializeField]
    private AudioSource song;

    private float audioStart;
    private float lengthOfBeat;
    private float lastBeat;
    private float nextBeat;

    public float LengthOfBeat
    {
        get
        {
            return lengthOfBeat;
        }
    }

    public float LastBeat
    {
        get
        {
            return lastBeat;
        }
    }

    public float NextBeat
    {
        get
        {
            return nextBeat;
        }
    }

    private void Start()
    {
        StartSong();
    }

    private void Update()
    {
        if (GetSongPosition() > lastBeat + lengthOfBeat)
        {
            lastBeat += lengthOfBeat;
            nextBeat += lengthOfBeat;
        }
    }

    public void StartSong()
    {
        lastBeat = 0;
        lengthOfBeat = 60 / BPM;
        nextBeat = lengthOfBeat;
        audioStart = (float)AudioSettings.dspTime;
        song.Play();
    }

    public float GetSongPosition()
    {
        return (float)(AudioSettings.dspTime - audioStart - offset);
    }
}
