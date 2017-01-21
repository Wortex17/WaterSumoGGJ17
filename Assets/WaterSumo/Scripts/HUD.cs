using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace WaterSumo
{
	public class HUD : MonoBehaviour
	{
		[Header("ImageComponents")]
		[SerializeField]
		private Image Player1CharacterImage;
		[SerializeField]
		private Image Player2CharacterImage;
		[SerializeField]
		private Image Player3CharacterImage;
		[SerializeField]
		private Image Player4CharacterImage;
		[Header("TextComponents")]
		[SerializeField]
		private Text Player1DistToBoundsText;
		[SerializeField]
		private Text Player2DistToBoundsText;
		[SerializeField]
		private Text Player3DistToBoundsText;
		[SerializeField]
		private Text Player4DistToBoundsText;

		[SerializeField]
		private Image Player1PickupImage;
		[SerializeField]
		private Image Player2PickupImage;
		[SerializeField]
		private Image Player3PickupImage;
		[SerializeField]
		private Image Player4PickupImage;

		void Start()
		{

		}

		void Update()
		{

		}

		public void SetDistToBounds(PlayerId _playerId, float _dist)
		{
			switch (_playerId)
			{
				case PlayerId.Player1:
					Player1DistToBoundsText.text = _dist.ToString();
					break;
				case PlayerId.Player2:
					Player2DistToBoundsText.text = _dist.ToString();
					break;
				case PlayerId.Player3:
					Player3DistToBoundsText.text = _dist.ToString();
					break;
				case PlayerId.Player4:
					Player4DistToBoundsText.text = _dist.ToString();
					break;
				default:
					break;
			}
		}

		public void SetCharacterImage(PlayerId _playerId, Sprite _charcterSprite)
		{
			switch (_playerId)
			{
				case PlayerId.Player1:
					Player1CharacterImage.sprite = _charcterSprite;
					break;
				case PlayerId.Player2:
					Player2CharacterImage.sprite = _charcterSprite;
					break;
				case PlayerId.Player3:
					Player3CharacterImage.sprite = _charcterSprite;
					break;
				case PlayerId.Player4:
					Player4CharacterImage.sprite = _charcterSprite;
					break;
				default:
					break;
			}
		}

		public void SetEnableCharacter(PlayerId _playerId, bool _enable)
		{
			switch (_playerId)
			{
				case PlayerId.Player1:
					Player1CharacterImage.enabled = _enable;
					Player1DistToBoundsText.enabled = _enable;
					break;
				case PlayerId.Player2:
					Player2CharacterImage.enabled = _enable;
					Player2DistToBoundsText.enabled = _enable;
					break;
				case PlayerId.Player3:
					Player3CharacterImage.enabled = _enable;
					Player3DistToBoundsText.enabled = _enable;
					break;
				case PlayerId.Player4:
					Player4CharacterImage.enabled = _enable;
					Player4DistToBoundsText.enabled = _enable;
					break;
				default:
					break;
			}
		}


		public void SetPickup(PlayerId _playerId, Sprite _sprite)
		{
			switch (_playerId)
			{
				case PlayerId.Player1:
					Player1PickupImage.enabled = _sprite != null;
					Player1PickupImage.sprite = _sprite;
					break;
				case PlayerId.Player2:
					Player2PickupImage.enabled = _sprite != null;
					Player2PickupImage.sprite = _sprite;
					break;
				case PlayerId.Player3:
					Player3PickupImage.enabled = _sprite != null;
					Player3PickupImage.sprite = _sprite;
					break;
				case PlayerId.Player4:
					Player4PickupImage.enabled = _sprite != null;
					Player4PickupImage.sprite = _sprite;
					break;
				default:
					break;
			}
		}
	}
}
