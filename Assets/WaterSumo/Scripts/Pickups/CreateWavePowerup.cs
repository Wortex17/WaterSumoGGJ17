using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{

	public class CreateWavePowerup : PickupBase
	{
		[SerializeField]
		private GameObject WavePrefab;

		override public void ActivatePickUp(PlayerController _playerController)
		{
			base.ActivatePickUp(_playerController);

			Instantiate(WavePrefab, transform.position, transform.rotation);
			DeactivatePickUp(_playerController);
		}
	}
}
