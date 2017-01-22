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

            wasPushedByWaves = _playerController.PlayerWaveAffected.IsPushedByWaves;
            _playerController.PlayerWaveAffected.IsPushedByWaves = false;

            wasRidingOnWaveSlope = _playerController.PlayerWaveAffected.RidesOnWaveSlope;
            _playerController.PlayerWaveAffected.RidesOnWaveSlope = false;
        }

		override public void DeactivatePickUp(PlayerController _playerController)
		{
			base.DeactivatePickUp(_playerController);

            _playerController.PlayerWaveAffected.IsPushedByWaves = wasPushedByWaves;
            _playerController.PlayerWaveAffected.RidesOnWaveSlope = wasRidingOnWaveSlope;

        }

        private bool wasPushedByWaves = false;
        private bool wasRidingOnWaveSlope = false;
    }
}
