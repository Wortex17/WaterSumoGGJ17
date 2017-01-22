using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace WaterSumo
{
	[RequireComponent(typeof(AudioSource))]
	public class PickupBase : MonoBehaviour
	{
		[SerializeField]
		private Sprite PickupSprite;
		private AudioSource PickupAudioSource;

		virtual protected void Start()
		{
			PickupAudioSource = GetComponent<AudioSource>();
			PickupAudioSource.loop = false;
			PickupAudioSource.playOnAwake = false;
			PickupAudioSource.outputAudioMixerGroup = GameHUB.Instance.FXAudioMixerGroup;
		}

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
				if (playerController.PickupCanUse == null)
				{
					playerController.GameHUD.SetPickup(playerController.PlayerIdType, PickupSprite);
					PickupAudioSource.Play();
					GetComponent<MeshRenderer>().enabled = false;
					GetComponent<Collider>().enabled = false;
					playerController.PickupCanUse = this;
					transform.SetParent(_coll.transform);
					transform.localPosition = Vector3.zero;
				}
				else
					Destroy(gameObject);
			}
		}
	}
}
