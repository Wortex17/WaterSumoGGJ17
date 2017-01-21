using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{
	public class IgnoreWavesPickup : TimeBasedPickup
	{
		override public void ActivatePickUp(PlayerController _playerController)
		{
			base.ActivatePickUp(_playerController);

			_playerController.PlayerWaveAffected.IsPushedByWaves = false;
		}

		override public void DeactivatePickUp(PlayerController _playerController)
		{
			base.DeactivatePickUp(_playerController);

			_playerController.PlayerWaveAffected.IsPushedByWaves = true;

		}
	}
}
