using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMaster : MonoBehaviour {
    
    public enum BeatMode
    {
        Constant
    }

    public void RecollectBeatReceivers()
    {
        receivers.Clear();
        receivers.AddRange(FindObjectsOfType<BeatReceiver>());
    }

    public void ResetProgress()
    {
        recordedTime = 0f;
        beatCount = -1;
    }


    void OnEnable()
    {
        RecollectBeatReceivers();
    }
	
	// Update is called once per frame
	void Update ()
    {
        RecollectBeatReceivers();

        recordedTime += Time.deltaTime;

        if (recordedTime > nextBeatTime)
        {
            lastBeatTime = nextBeatTime;
            beatCount++;

            switch (beatMode)
            {
                case BeatMode.Constant:
                default:
                    nextBeatTime = lastBeatTime + (constantBeatsPerSecond > 0f ? 1f / constantBeatsPerSecond : 1f);
                    break;
            }
        }

        float timeToNextBeat = nextBeatTime - recordedTime;

        foreach (var receiver in receivers)
        {
            if (receiver != null)
            {
                if (receiver.ExpectedBeat <= beatCount)
                {
                    receiver.ReceiveBeat(beatCount);
                } else if (receiver.ExpectedBeat == beatCount+1 && receiver.PreviewBeat < timeToNextBeat)
                {
                    receiver.ReceiveBeat(beatCount+1);
                }
            }
        }
    }

    [SerializeField]
    private BeatMode beatMode = BeatMode.Constant;

    [SerializeField]
    private float constantBeatsPerSecond = 1f;

    private int beatCount = -1;
    private float recordedTime = 0f;
    private float lastBeatTime = 0f;
    private float nextBeatTime = 0f;

    private List<BeatReceiver> receivers = new List<BeatReceiver>();
}
