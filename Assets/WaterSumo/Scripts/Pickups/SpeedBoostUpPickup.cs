using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{
	public class SpeedBoostUpPickup : TimeBasedPickup
	{
		[Header("Pickup Property")]
		[SerializeField]
		private float AddSpeedBoost = 5.0f;
		[SerializeField]
		private float AddMaxSpeedBoost = 10.0f;

		override public void ActivatePickUp(PlayerController _playerController)
		{
			base.ActivatePickUp(_playerController);

			_playerController.MoveSpeed += AddSpeedBoost;
			_playerController.MaxInputVelocity += AddMaxSpeedBoost;
		}

		override public void DeactivatePickUp(PlayerController _playerController)
		{
			base.DeactivatePickUp(_playerController);
			_playerController.MoveSpeed -= AddSpeedBoost;
			_playerController.MaxInputVelocity -= AddMaxSpeedBoost;
		}
	}
}
