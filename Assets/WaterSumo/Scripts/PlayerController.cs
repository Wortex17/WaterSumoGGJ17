using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{

	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(WaveAffected))]
	public class PlayerController : MonoBehaviour
	{
		private Rigidbody PlayerRigidbody;
		private ArenaBehaviour LocalArenaBehaviour;
		[HideInInspector]
		public WaveAffected PlayerWaveAffected;
        [HideInInspector]
        public AudioSource SoundSource;
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
        [SerializeField]
        private GameObject CharacterModel;

        public int SkinId = 0;

        //Powerup
        [HideInInspector]
		public PickupBase PickupCanUse;
		[HideInInspector]
		public PickupBase ActivePickup;
		[HideInInspector]
		public HUD GameHUD;

	    public SumoSoundProfile SoundProfile
        {
            get { return soundProfile; }
            set { soundProfile = value; }
        }
	    private SumoSoundProfile soundProfile = null;

		//Emogis
		[Header("Emogis")]
		[SerializeField]
		private SpriteRenderer EmogisSpriteRenderer;
		[SerializeField]
		float MinDistCloseToBounds = 1.0f;
		//ResetTime
		[SerializeField]
		float ResetTimeBump = 1.0f;
		[SerializeField]
		float ResetTimeUsePickUp = 1.0f;
		[SerializeField]
		float ResetTimeShuting = 1.0f;
		//Time to reset
		float TimeToResetIsBump = 0.0f;
		float TimeToResetIsUsePickup = 0.0f;
		float TimeToResetIsShuting = 0.0f;
		//bools
		bool IsCloseToBounds = false;
		bool IsBump = false;
		bool IsUsePickup = false;
		bool IsShouting = false;
		//Emogis Sprite
		[SerializeField]
		private Sprite IsCloseToBoundsSprite;
		[SerializeField]
		private Sprite IsBumpSprite;
		[SerializeField]
		private Sprite IsRigdingWaveSprite;
		[SerializeField]
		private Sprite IsUsePickupSprite;
		[SerializeField]
		private Sprite IsShutingSprite;
		[SerializeField]
		private Sprite IdleSprite;


		void Awake()
		{
			PlayerRigidbody = GetComponent<Rigidbody>();
			LocalArenaBehaviour = FindObjectOfType<ArenaBehaviour>();
            PlayerWaveAffected = GetComponent<WaveAffected>();
            SoundSource = GetComponent<AudioSource>();
        }

		public void InitialicePlayer(PlayerId _playerId, HUD _gameHud)
		{
			GameHUD = _gameHud;

			PlayerIdType = _playerId;
			SetInput(_playerId);

            SoundSource.outputAudioMixerGroup = GameHUB.Instance.FXAudioMixerGroup;

            SwimmingRing.GetComponentInChildren<Renderer>().material = GameHUB.Instance.SkinLibrary.RingSkins[(int)_playerId];
		    var characterModelRenderer = CharacterModel.GetComponentInChildren<Renderer>();
            characterModelRenderer.material = GameHUB.Instance.SkinLibrary.SumoSkins[SkinId];

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
				float distToBounds = LocalArenaBehaviour.DistanceToBorder(gameObject).magnitude;
				IsCloseToBounds = (distToBounds < MinDistCloseToBounds);
				GameHUD.SetDistToBounds(PlayerIdType, distToBounds);
				ResetEmogisTimer();
				HandleEmogis();
			}
		}

		//Input Controlls
		private void HandleInput()
		{
			HandleMovement(Input.GetAxis(InputUpString), Input.GetAxis(InputRightString));

            if (Input.GetButtonDown(InputXString) || Input.GetButtonDown(InputBString))
                Shouting();

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
		private void Shouting()
		{
		    if (!IsShouting && SoundProfile != null)
            {
                SoundSource.PlayOneShot(SoundProfile.Shouts.GetRandom());
                IsShouting = true;
                TimeToResetIsShuting = Time.time + ResetTimeShuting;
            }
		}
		private void ActivatePowerUp()
		{
			if (ActivePickup == null && PickupCanUse != null)
			{
				GameHUD.SetPickup(PlayerIdType, null);
				ActivePickup = PickupCanUse;
				ActivePickup.ActivatePickUp(this);

				IsUsePickup = true;
				TimeToResetIsUsePickup = Time.time + ResetTimeUsePickUp;
			}
		}

		//Emogis
		private void HandleEmogis()
		{
			if(IsCloseToBounds)
				SetEmogi(IsCloseToBoundsSprite);
			else if(IsBump)
				SetEmogi(IsBumpSprite);
			else if( PlayerWaveAffected.IsRidingOnWave)
				SetEmogi(IsRigdingWaveSprite);
			else if(IsUsePickup)
				SetEmogi(IsUsePickupSprite);
			else if(IsShouting)
				SetEmogi(IsShutingSprite);
			else
				SetEmogi(IdleSprite);

		}
		private void ResetEmogisTimer()
		{
			if (IsBump && TimeToResetIsBump <= Time.time)
				IsBump = false;
			if (IsUsePickup && TimeToResetIsUsePickup <= Time.time)
				IsUsePickup = false;
			if (IsShouting && TimeToResetIsShuting <= Time.time)
				IsShouting = false;
		}
		private void SetEmogi(Sprite _sprite)
		{
			if(EmogisSpriteRenderer.sprite != _sprite)
			{
				EmogisSpriteRenderer.sprite = _sprite;
			}
		}

		private void OnCollisionEnter(Collision _coll)
		{
            SoundSource.PlayOneShot(GameHUB.Instance.SoundLibrary.SumoBumps.GetRandom());

            IsBump = true;
			TimeToResetIsBump = Time.time + ResetTimeBump;
		}
	}
}
