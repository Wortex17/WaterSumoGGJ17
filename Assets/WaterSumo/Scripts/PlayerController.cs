using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{

	[RequireComponent(typeof(Rigidbody))]
	public class PlayerController : MonoBehaviour
	{
		private Rigidbody PlayerRigidbody;
		private bool IsInitialice = false;
		//Input
		[SerializeField]
		private PlayerId PlayerIdType = PlayerId.None;
		private string InputUpString = "";
		private string InputRightString = "";
		private string InputXString = "";

		//Movement
		[SerializeField]
		private float MoveSpeed = 5.0f;
		[SerializeField]
		private float MaxInputVelocity = 10.0f;

		void Awake()
		{
			PlayerRigidbody = GetComponent<Rigidbody>();
		}

		public void InitialicePlayer(PlayerId _playerId, Material _swimmingRingMaterial)
		{
			PlayerIdType = _playerId;
			SetInput(_playerId);
			IsInitialice = true;
		}
		private void SetInput(PlayerId _playerId)
		{
			switch (_playerId)
			{
				case PlayerId.Player1:
					InputUpString = "Controller1Up";
					InputRightString = "Controller1Right";
					InputXString = "Controller1X";
					break;
				case PlayerId.Player2:
					InputUpString = "Controller2Up";
					InputRightString = "Controller2Right";
					InputXString = "Controller2X";
					break;
				case PlayerId.Player3:
					InputUpString = "Controller3Up";
					InputRightString = "Controller3Right";
					InputXString = "Controller3X";
					break;
				case PlayerId.Player4:
					InputUpString = "Controller4Up";
					InputRightString = "Controller4Right";
					InputXString = "Controller4X";
					break;
			}
		}

		void Update()
		{
			if (IsInitialice)
			{
				HandleInput();
			}
        }

		private void HandleInput()
		{
			HandleMovement(Input.GetAxis(InputUpString), Input.GetAxis(InputRightString));
			if (Input.GetButtonDown(InputXString))
				Shuting();
        }
		private void HandleMovement(float _upInput, float _rightInput)
		{
			Debug.Log(transform.name + " up: " + _upInput);
			Vector3 moveDirection = new Vector3(-_rightInput, 0.0f, _upInput);
			float actualVelocityMagnitude = PlayerRigidbody.velocity.magnitude;

			PlayerRigidbody.velocity += moveDirection * MoveSpeed * Time.deltaTime;

			if(PlayerRigidbody.velocity.magnitude > MaxInputVelocity)
			{
				if(actualVelocityMagnitude > MaxInputVelocity)
					PlayerRigidbody.velocity = PlayerRigidbody.velocity.normalized * actualVelocityMagnitude;
				else
					PlayerRigidbody.velocity = PlayerRigidbody.velocity.normalized * MaxInputVelocity;
			}
		}

		private void Shuting()
		{
			Debug.Log("arsch");
		}
	}
}
