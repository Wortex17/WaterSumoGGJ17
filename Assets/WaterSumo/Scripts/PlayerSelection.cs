using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace WaterSumo
{

	public class PlayerSelection : MonoBehaviour {
	
		[SerializeField]
		private Image[] PlayerPortraits = new Image[4];
	
		[SerializeField]
		private Sprite[] Sumo = new Sprite[4];

		private bool[] IsPictureTaken = new bool[4];

		private bool[] IsPlayerConnected = new bool[4];

		private int[] id = new int[4];

		private bool[] LockInput = new bool[4];
		
		private WaterSumo.GameManager Manager;
		
		// Update is called once per frame
		void Update () {
	
			DetectPlayers ();
			Select ("Controller1Right", "Controller1X", 0, ref id[0]);
			Select ("Controller2Right", "Controller2X", 1, ref id[1]);
			Select ("Controller3Right", "Controller3X", 2, ref id[2]);
			Select ("Controller4Right", "Controller4X", 3, ref id[3]);
	
		}
	
		void DetectPlayers()
		{
			if (Input.GetButtonDown ("Controller1A"))
			{
				IsPlayerConnected [0] = true;
				PlayerPortraits [0].sprite = Sumo [0];
			}
			if (Input.GetButtonDown ("Controller1B"))
				IsPlayerConnected [0] = false;
			if (Input.GetButtonDown ("Controller2A"))
			{
				PlayerPortraits [1].sprite = Sumo [0];
				IsPlayerConnected [1] = true;
			}
			if (Input.GetButtonDown ("Controller2B"))
				IsPlayerConnected [1] = false;
			if (Input.GetButtonDown ("Controller3A"))
			{
				PlayerPortraits [2].sprite = Sumo [0];
				IsPlayerConnected [2] = true;
			}
			if (Input.GetButtonDown ("Controller3B"))
				IsPlayerConnected [2] = false;
			if (Input.GetButtonDown ("Controller4A"))
			{
				PlayerPortraits [3].sprite = Sumo [0];
				IsPlayerConnected [3] = true;
			}
			if (Input.GetButtonDown ("Controller4B"))
				IsPlayerConnected [3] = false;
		}


		void Select(string _Axis1, string _ButtonX1, int _arrayIndex, ref int _id)
		{
			if (IsPlayerConnected [_arrayIndex])
			{
				if (Input.GetAxis (_Axis1) == 0 && !IsPictureTaken [_arrayIndex])
					LockInput [_arrayIndex] = false;
				if (Input.GetAxis (_Axis1) > 0 && !LockInput[_arrayIndex])
				{
					_id++;
					if (_id++ == 4)
						_id = 0;

					if (IsPictureTaken [_arrayIndex])
						_id++;
					PlayerPortraits [_arrayIndex].sprite = Sumo [_id];
					LockInput[_arrayIndex] = true;
				}
				if (Input.GetAxis (_Axis1) < 0 && !LockInput[_arrayIndex])
				{
					_id--;
					if (_id == -1)
						_id = 3;

					if (IsPictureTaken [_arrayIndex])
						_id--;
					PlayerPortraits [_arrayIndex].sprite = Sumo [_id];
					LockInput[_arrayIndex] = true;
				}

				// Lock the data into the Game Manager
				if (Input.GetButtonDown (_ButtonX1))
				{
					IsPictureTaken [_arrayIndex] = true;
					Manager.PlayerDatas [_arrayIndex].IsLoggedIn = true;
					Manager.PlayerDatas [_arrayIndex].MaterialId = _id;
					LockInput[_arrayIndex] = true;
				}
			}



			//// Player 1 is connected
			//if (IsPlayerConnected [0])
			//{
			//	if (Input.GetAxis ("Controller1Right") == 0 && !IsPictureTaken [0])
			//		LockInput [0] = false;
			//	if (Input.GetAxis ("Controller1Right") > 0 && !LockInput[0])
			//	{
			//		id [0]++;
			//		if (id [0] == 4)
			//			id [0] = 0;
			//
			//		if (IsPictureTaken [0])
			//			id [0]++;
			//		PlayerPortraits [0].sprite = Sumo [id[0]];
			//		LockInput[0] = true;
			//	}
			//	if (Input.GetAxis ("Controller1Right") < 0 && !LockInput[0])
			//	{
			//		id [0]--;
			//		if (id [0] == -1)
			//			id [0] = 3;
			//
			//		if (IsPictureTaken [0])
			//			id [0]--;
			//		PlayerPortraits [0].sprite = Sumo [id[0]];
			//		LockInput[0] = true;
			//	}
			//
			//	// Lock the data into the Game Manager
			//	if (Input.GetButtonDown ("Controller1X"))
			//	{
			//		IsPictureTaken [0] = true;
			//		Manager.PlayerDatas [0].IsLoggedIn = true;
			//		Manager.PlayerDatas [0].MaterialId = id[0];
			//		LockInput[0] = true;
			//		Debug.Log ("Player1 logged in");
			//	}
			//}
			//// Player 2 is connected
			//if (IsPlayerConnected [1])
			//{
			//	if (Input.GetAxis ("Controller2Right") == 0 && !IsPictureTaken [1])
			//		LockInput [1] = false;
			//	if (Input.GetAxis ("Controller2Right") > 0 && !LockInput[1])
			//	{
			//		id [1]++;
			//		if (id [1] == 4)
			//			id [1] = 0;
			//
			//		if (IsPictureTaken [1])
			//			id [1]++;
			//		PlayerPortraits [1].sprite = Sumo [id[0]];
			//		LockInput[1] = true;
			//	}
			//	if (Input.GetAxis ("Controller2Right") < 0 && !LockInput[1])
			//	{
			//		id [1]--;
			//		if (id [1] == -1)
			//			id [1] = 3;
			//
			//		if (IsPictureTaken [1])
			//			id [1]--;
			//		PlayerPortraits [1].sprite = Sumo [id[1]];
			//		LockInput[1] = true;
			//	}
			//
			//	// Lock the data into the Game Manager
			//	if (Input.GetButtonDown ("Controller2X"))
			//	{
			//		IsPictureTaken [1] = true;
			//		Manager.PlayerDatas [1].IsLoggedIn = true;
			//		Manager.PlayerDatas [1].MaterialId = id[1];
			//		LockInput[1] = true;
			//		Debug.Log ("Player2 logged in");
			//	}
			//}
			//// Player 3 is connected
			//if (IsPlayerConnected [2])
			//{
			//	if (Input.GetAxis ("Controller3Right") == 0 && !IsPictureTaken [2])
			//		LockInput [2] = false;
			//	if (Input.GetAxis ("Controller3Right") > 0 && !LockInput[2])
			//	{
			//		id [2]++;
			//		if (id [2] == 4)
			//			id [2] = 0;
			//
			//		if (IsPictureTaken [2])
			//			id [2]++;
			//		PlayerPortraits [2].sprite = Sumo [id[2]];
			//		LockInput[2] = true;
			//	}
			//	if (Input.GetAxis ("Controller3Right") < 0 && !LockInput[2])
			//	{
			//		id [2]--;
			//		if (id [2] == -1)
			//			id [2] = 3;
			//
			//		if (IsPictureTaken [2])
			//			id [2]--;
			//		PlayerPortraits [2].sprite = Sumo [id[2]];
			//		LockInput[2] = true;
			//	}
			//
			//	// Lock the data into the Game Manager
			//	if (Input.GetButtonDown ("Controller3X"))
			//	{
			//		IsPictureTaken [2] = true;
			//		Manager.PlayerDatas [2].IsLoggedIn = true;
			//		Manager.PlayerDatas [2].MaterialId = id[2];
			//		LockInput[2] = true;
			//		Debug.Log ("Player3 logged in");
			//	}
			//}
			//// Player 4 is connected
			//if (IsPlayerConnected [3])
			//{
			//	if (Input.GetAxis ("Controller4Right") == 0 && !IsPictureTaken [3])
			//		LockInput [3] = false;
			//	if (Input.GetAxis ("Controller4Right") > 0 && !LockInput[3])
			//	{
			//		id [3]++;
			//		if (id [3] == 4)
			//			id [3] = 0;
			//
			//		if (IsPictureTaken [3])
			//			id [3]++;
			//		PlayerPortraits [3].sprite = Sumo [id[3]];
			//		LockInput[3] = true;
			//	}
			//	if (Input.GetAxis ("Controller4Right") < 0 && !LockInput[3])
			//	{
			//		id [3]--;
			//		if (id [3] == -1)
			//			id [3] = 3;
			//
			//		if (IsPictureTaken [3])
			//			id [3]--;
			//		PlayerPortraits [3].sprite = Sumo [id[3]];
			//		LockInput[3] = true;
			//	}
			//
			//	// Lock the data into the Game Manager
			//	if (Input.GetButtonDown ("Controller4X"))
			//	{
			//		IsPictureTaken [3] = true;
			//		Manager.PlayerDatas [3].IsLoggedIn = true;
			//		Manager.PlayerDatas [3].MaterialId = id[3];
			//		LockInput[3] = true;
			//		Debug.Log ("Player4 logged in");
			//	}
			}
 
 
 

		}
}

