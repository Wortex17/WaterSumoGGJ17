using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Stomper : BeatReceiver {

	[SerializeField, FormerlySerializedAs("Wave")]
	private GameObject WavePrefab;

    /*
	void Start () {

        // TODO: Get the wave reference by code or drag an drop in inspector?!

        //Start with random Timer
        stompingInterval = Random.Range(minRange, maxRange);
    }
	
	void Update () {

		stompingTimer += Time.deltaTime * timeMultiplicator;

		if (stompingTimer >= stompingInterval)
			Stomp();
	}
    */
    protected override void OnBeat(int beat)
    {
        Stomp();
    }

    public void Stomp()
    {
        Instantiate(WavePrefab, transform.position, Quaternion.identity, this.transform);
    }
}
