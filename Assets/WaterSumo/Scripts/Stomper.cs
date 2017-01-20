using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomper : MonoBehaviour {

	[SerializeField]
	private GameObject Wave;

	private float stompingTimer = 0;

	[SerializeField]
	private float stompingInterval = 1;

	[SerializeField]
	private float timeMultiplicator;

	void Start () {

		// TODO: Get the wave reference by code or drag an drop in inspector?!

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

		// TODO: Add some Coordinates for instantiating maybe?
		Debug.Log("BinDrin");
		// ResetTimer
		stompingTimer = 0;
	}
}
