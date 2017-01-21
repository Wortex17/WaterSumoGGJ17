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
		public PlayerId PlayerIdType = PlayerId.None;
		private string InputUpString = "";
		private string InputRightString = "";
		private string InputXString = "";
		private string InputBString = "";
		private string InputAString = "";

		//Movement
		[SerializeField]
		public float MoveSpeed = 5.0f;
		[SerializeField]
		public float MaxInputVelocity = 10.0f;

		//Geometry
		[SerializeField]
		private GameObject SwimmingRing;

		//Powerup
		[HideInInspector]
		public PickupBase PickupCanUse;
		[HideInInspector]
		public PickupBase ActivePickup;

		//ArenaBehaviour
		private ArenaBehaviour LocalArenaBehaviour;

		void Awake()
		{
			PlayerRigidbody = GetComponent<Rigidbody>();
			LocalArenaBehaviour = FindObjectOfType<ArenaBehaviour>();
		}

		public void InitialicePlayer(PlayerId _playerId, Material _swimmingRingMaterial)
		{
			PlayerIdType = _playerId;
			SetInput(_playerId);

			//Set swimmingRing material
			SwimmingRing.GetComponent<MeshRenderer>().material = _swimmingRingMaterial;
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
					InputBString = "Controller1B";
					InputAString = "Controller1A";
					break;
				case PlayerId.Player2:
					InputUpString = "Controller2Up";
					InputRightString = "Controller2Right";
					InputXString = "Controller2X";
					InputBString = "Controller2B";
					InputAString = "Controller2A";
					break;
				case PlayerId.Player3:
					InputUpString = "Controller3Up";
					InputRightString = "Controller3Right";
					InputXString = "Controller3X";
					InputBString = "Controller3B";
					InputAString = "Controller3A";
					break;
				case PlayerId.Player4:
					InputUpString = "Controller4Up";
					InputRightString = "Controller4Right";
					InputXString = "Controller4X";
					InputBString = "Controller4B";
					InputAString = "Controller4A";
					break;
			}
		}

		void Update()
		{
			if (IsInitialice)
			{
				HandleInput();

				//DistanceToBorder
				LocalArenaBehaviour.DistanceToBorder(gameObject);
			}
        }

		private void HandleInput()
		{
			HandleMovement(Input.GetAxis(InputUpString), Input.GetAxis(InputRightString));

			if (Input.GetButtonDown(InputXString))
				Shuting();

			if (Input.GetButtonDown(InputAString))
				ActivatePowerUp();

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

		private void ActivatePowerUp()
		{
			if (ActivePickup == null && PickupCanUse != null)
			{
				ActivePickup = PickupCanUse;
				ActivePickup.ActivatePickUp(this);
			}
		}
	}
}
