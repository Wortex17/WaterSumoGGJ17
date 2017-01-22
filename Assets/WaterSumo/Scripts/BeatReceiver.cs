using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BeatReceiver : MonoBehaviour
{
    public int ExpectedBeat {  get {  return localBeatCount + 1; } }
    public float PreviewBeat {  get { return GetBeatPreview();  } }

    public void ReceiveBeat(int beat, float beatWeight)
    {
        OnAnyBeat(beat, beatWeight);
        if (beat != localBeatCount)
        {
            OnBeat(beat, beatWeight);
            localBeatCount = beat;
        }
    }

    protected virtual void OnBeat(int beat, float beatWeight)
    {

    }

    protected virtual void OnAnyBeat(int beat, float beatWeight)
    {

    }

    /// <summary>
    /// Return how many seconds before the beat you wnat to get beat events
    /// </summary>
    /// <returns></returns>
    protected virtual float GetBeatPreview()
    {
        return 0f;
    }

    private int localBeatCount = -1;
}
