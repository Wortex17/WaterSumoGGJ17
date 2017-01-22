using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using WaterSumo;

public class Stomper : BeatReceiver {

	[SerializeField, FormerlySerializedAs("Wave")]
	private GameObject WavePrefab;

    protected override float GetBeatPreview()
    {
        return rawStompTime/speed;
    }

    protected void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    protected void Update()
    {
        animator.speed = speed;

        if (stompCountdown > 0f)
        {
            stompCountdown -= Time.deltaTime * speed;
            if (stompCountdown <= stompEarly)
            {
                stompCountdown = 0f;
                Stomp();
            }
        }
    }
    
    protected override void OnBeat(int beat, float beatWeight)
    {
        stompCountdown = rawStompTime;
        stompStrength = beatWeight;
        animator.SetTrigger("stomp");
    }

    public void Stomp()
    {
        var waveGO = Instantiate(WavePrefab.gameObject, transform.position, Quaternion.identity, this.transform);
        waveGO.GetComponent<Wave>().OverallStrength = stompStrength;
    }

    [SerializeField]
    private float rawStompTime = 0f;
    [SerializeField]
    private float stompEarly = 0f;
    [SerializeField]
    private float speed = 1f;

    private float stompStrength = 1f;
    private float stompCountdown = 0f;

    private Animator animator = null;
}
