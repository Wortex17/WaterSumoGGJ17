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
		private bool[] BlockEverything = new bool[4];


		// Update is called once per frame
		void Update ()
		{
	
			DetectPlayers ("Controller1A","Controller1B",0);
			DetectPlayers ("Controller2A","Controller2B",1);
			DetectPlayers ("Controller3A","Controller3B",2);
			DetectPlayers ("Controller4A","Controller4B",3);
			Select ("Controller1Right", "Controller1X", 0, ref id[0]);
			Select ("Controller2Right", "Controller2X", 1, ref id[1]);
			Select ("Controller3Right", "Controller3X", 2, ref id[2]);
			Select ("Controller4Right", "Controller4X", 3, ref id[3]);
	
		}
	
		void DetectPlayers(string _inputA, string _inputB, int _arrayIndex)
		{
			if (Input.GetButtonDown (_inputA))
				{
				IsPlayerConnected [_arrayIndex] = true;
				PlayerPortraits [_arrayIndex].enabled = true;
				for (int i = 0; i < 4; i++)
				{
					if (!IsPictureTaken[i])
					{
						PlayerPortraits[_arrayIndex].sprite = Sumo[i];
						break;
					}
				}

			}
			if (Input.GetButtonDown (_inputB))
				IsPlayerConnected [_arrayIndex] = false;
		}


		void Select(string _Axis1, string _ButtonX1, int _arrayIndex, ref int _id)
		{
			if (IsPlayerConnected [_arrayIndex] && !BlockEverything[_arrayIndex])
			{
				if (Input.GetAxis (_Axis1) < 0.2f && Input.GetAxis (_Axis1) > -0.2f )
					LockInput [_arrayIndex] = false;
				if (Input.GetAxis (_Axis1) > 0.2f && !LockInput[_arrayIndex])
				{
					do
					{
						_id++;

						if (_id == 4)
							_id = 0;
					}
					while (IsPictureTaken[_id]);

					PlayerPortraits [_arrayIndex].sprite = Sumo [_id];
					LockInput[_arrayIndex] = true;
				}
				if (Input.GetAxis (_Axis1) < -0.2f && !LockInput[_arrayIndex])
				{

					do
					{
						_id--;

						if (_id == -1)
							_id = 3;
					}
					while (IsPictureTaken[_id]);

					PlayerPortraits [_arrayIndex].sprite = Sumo [_id];
					LockInput[_arrayIndex] = true;
				}

				// Lock the data into the Game Manager
				if (Input.GetButtonDown (_ButtonX1))
				{
					IsPictureTaken [_id] = true;
					BlockEverything[_arrayIndex] = true;
					GameHUB.Instance.GameManager.PlayerDatas [_arrayIndex].IsLoggedIn = true;
					GameHUB.Instance.GameManager.PlayerDatas [_arrayIndex].MaterialId = _id;
					LockInput[_arrayIndex] = true;
				}
			}
		}
	}
}

