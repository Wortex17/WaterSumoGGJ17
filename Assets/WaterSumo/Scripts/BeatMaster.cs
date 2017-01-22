using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMaster : MonoBehaviour {
    
    public enum BeatMode
    {
        Constant,
        Random
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
        lastBeatTime = 0f;
        nextBeatTime = delayFirstBeat;
    }


    void OnEnable()
    {
        RecollectBeatReceivers();
    }

    void Start()
    {
        ResetProgress();
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
                case BeatMode.Random:
                    nextBeatTime += randomBeatsWait.GetRandomValue()*randomBeatsFactor;
                    break;
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
                    receiver.ReceiveBeat(beatCount, beatLevelCurve.Evaluate(beatCount));
                } else if (receiver.ExpectedBeat == beatCount+1 && receiver.PreviewBeat > timeToNextBeat)
                {
                    receiver.ReceiveBeat(beatCount+1, beatLevelCurve.Evaluate(beatCount));
                }
            }
        }
    }


    [SerializeField] private float delayFirstBeat = 1f;
    [SerializeField]
    private BeatMode beatMode = BeatMode.Constant;

    [SerializeField]
    private float constantBeatsPerSecond = 1f;
    [MinMaxRange(0.1f, 15f), SerializeField]
    private MinMaxRange randomBeatsWait = new MinMaxRange();
    [SerializeField]
    private float randomBeatsFactor = 1f;

    [SerializeField, Tooltip("How the beatLevel (strength) is at each beat")]
    private AnimationCurve beatLevelCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

    private int beatCount = -1;
    private float recordedTime = 0f;
    private float lastBeatTime = 0f;
    private float nextBeatTime = 0f;

    private List<BeatReceiver> receivers = new List<BeatReceiver>();
}
