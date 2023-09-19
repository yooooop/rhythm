using UnityEngine;

public abstract class OnBeatBehaviour : MonoBehaviour
{
    protected Conductor conductor;
    private float lastBeatTracked;

    protected virtual void Start()
    {
        conductor = FindObjectOfType<Conductor>();
    }

    protected virtual void Update()
    {
        if (!Mathf.Approximately(lastBeatTracked, conductor.LastBeat))
        {
            lastBeatTracked = conductor.LastBeat;
            OnBeat();
        }
    }

    protected abstract void OnBeat();
}
