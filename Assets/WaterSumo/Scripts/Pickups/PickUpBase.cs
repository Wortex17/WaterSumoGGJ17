using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{
	public class PickupBase : MonoBehaviour
	{
		virtual public void ActivatePickUp(PlayerController _playerController)
		{

		}

		virtual public void DeactivatePickUp(PlayerController _playerController)
		{
			_playerController.ActivePickup = null;
			Destroy(gameObject);
		}


		protected void OnTriggerEnter(Collider _coll)
		{
			PlayerController playerController = _coll.GetComponent<PlayerController>();
			if(playerController)
			{
				GetComponent<MeshRenderer>().enabled = false;
				playerController.PickupCanUse = this;
				transform.SetParent(_coll.transform);
				transform.localPosition = Vector3.zero;
			}
		}
	}
}
