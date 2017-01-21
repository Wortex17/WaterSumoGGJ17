using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomper : MonoBehaviour {

	[SerializeField]
	private GameObject Wave;

	private float stompingTimer = 0;

    [SerializeField]
    private int minRange = 2;
    [SerializeField]
    private int maxRange = 10;

    private float stompingInterval;

	[SerializeField]
	private float timeMultiplicator;

	void Start () {

        // TODO: Get the wave reference by code or drag an drop in inspector?!

        //Start with random Timer
        stompingInterval = Random.Range(minRange, maxRange);
    }
	
	void Update () {

		stompingTimer += Time.deltaTime * timeMultiplicator;

		if (stompingTimer >= stompingInterval)
			STOMPIT ();

		Debug.Log ("Timer" + stompingTimer);
		
	}

	void STOMPIT()
	{
		// Create waves?!?!
		Instantiate(Wave);

        //Calculate new Interval after each wave
        stompingInterval = Random.Range(minRange, maxRange);

		// TODO: Add some Coordinates for instantiating maybe?
		Debug.Log("BinDrin");
		// ResetTimer
		stompingTimer = 0;
	}
}
