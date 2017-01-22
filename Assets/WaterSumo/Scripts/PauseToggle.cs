using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseToggle : MonoBehaviour {

	private bool stopTheTiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiime = false;
	[SerializeField]
	private GameObject Overlay;

	// Use this for initialization
	void Awake () {

		Overlay.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("ControllerStart"))
			{
				stopTheTiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiime = !stopTheTiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiime;
			}

		if (stopTheTiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiime)
		{
			Time.timeScale = 0;
			Overlay.SetActive (true);
		} 
		else
		{
			Time.timeScale = 1;
			Overlay.SetActive (false);
		}

	}
}
