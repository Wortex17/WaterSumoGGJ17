using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{

	public enum InputType { None, Controller1, Controller2, Controller3, Controller4 }

	[RequireComponent(typeof(Rigidbody))]
	public class PlayerController : MonoBehaviour
	{
		private Rigidbody PlayerRigidbody;

		//Input
		[SerializeField]
		private InputType PlayerInputType = InputType.None;
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
			SetInput(PlayerInputType);
		}

		private void SetInput(InputType _inputType)
		{
			switch (_inputType)
			{
				case InputType.Controller1:
					InputUpString = "Controller1Up";
					InputRightString = "Controller1Right";
					InputXString = "Controller1X";
					break;
				case InputType.Controller2:
					InputUpString = "Controller2Up";
					InputRightString = "Controller2Right";
					InputXString = "Controller2X";
					break;
				case InputType.Controller3:
					InputUpString = "Controller3Up";
					InputRightString = "Controller3Right";
					InputXString = "Controller3X";
					break;
				case InputType.Controller4:
					InputUpString = "Controller4Up";
					InputRightString = "Controller4Right";
					InputXString = "Controller4X";
					break;
			}
		}

		void Update()
		{
			HandleInput();
        }

		private void HandleInput()
		{
			HandleMovement(Input.GetAxis(InputUpString), Input.GetAxis(InputRightString));
			if (Input.GetButtonDown(InputXString))
				Shuting();
        }

		private void HandleMovement(float _upInput, float _rightInput)
		{
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
