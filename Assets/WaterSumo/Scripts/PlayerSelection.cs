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

					if (_id == 4)
						_id = 0;

					if (IsPictureTaken [_id])
						_id++;
					
					if (_id == 4)
						_id = 0;


					PlayerPortraits [_arrayIndex].sprite = Sumo [_id];
					LockInput[_arrayIndex] = true;
				}
				if (Input.GetAxis (_Axis1) < 0 && !LockInput[_arrayIndex])
				{
					_id--;

					if (_id == -1)
						_id = 3;

					if (IsPictureTaken [_id])
						_id--;

					if (_id == -1)
						_id = 3;


					PlayerPortraits [_arrayIndex].sprite = Sumo [_id];
					LockInput[_arrayIndex] = true;
				}

				// Lock the data into the Game Manager
				if (Input.GetButtonDown (_ButtonX1))
				{
					IsPictureTaken [_id] = true;
					Manager.PlayerDatas [_arrayIndex].IsLoggedIn = true;
					Manager.PlayerDatas [_arrayIndex].MaterialId = _id;
					LockInput[_arrayIndex] = true;
				}
			}
			}
 
 
 

		}
}

