using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WaterSumo
{
	public class TimeBasedPickup : PickupBase
	{

		[SerializeField]
		private float ActiveTime = 5.0f;
		private float TimerActive = 0.0f;
		private bool IsActive = false;

		PlayerController UsingPlayerController;
		protected void Update()
		{
			if(IsActive)
			{
				TimerActive += Time.deltaTime;
				if(TimerActive >= ActiveTime)
				{
					DeactivatePickUp(UsingPlayerController);
				}
			}
		}

		override public void ActivatePickUp(PlayerController _playerController)
		{
			base.ActivatePickUp(_playerController);
			UsingPlayerController = _playerController;
			IsActive = true;
		}

		override public void DeactivatePickUp(PlayerController _playerController)
		{
			base.DeactivatePickUp(_playerController);
		}
	}
}
