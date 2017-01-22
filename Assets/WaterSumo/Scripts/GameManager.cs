using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace WaterSumo
{
	public enum PlayerId { Player1, Player2, Player3, Player4, None }

	public struct PlayerData
	{
		public bool IsLoggedIn;
		public int MaterialId;

		public PlayerData(bool _isLoggedIn)
		{
			IsLoggedIn = _isLoggedIn;
			MaterialId = 0;
		}

	}
	[RequireComponent(typeof(AudioSource))]
	public class GameManager : MonoBehaviour
	{
		//LoggedIn states
		public PlayerData[] PlayerDatas;
		//SpwanPoints
		private Transform[] SpawnPoints;
		private bool[] SpawnPointsUsed;

		//PlayerPrefabs
        [SerializeField]
        private GameObject PlayerPrefab;

        //Sounds
        private AudioSource GameManagerAudioSource;
		[Header("Music")]
		[SerializeField]
		private AudioClip MenuMusic;
		[SerializeField]
		private AudioClip InGameMusic;

		//HUD
		[Header("HUD")]
		[SerializeField]
		private GameObject HUDPrefab;
		private HUD HUDInstanz;
		[SerializeField]
		private Sprite[] CharacterSprite;

		//GameStats
	    public int PlayersArePlaying
	    {
	        get {  return playersArePlaying; }
            set { playersArePlaying = value; }
	    }
		[HideInInspector]
		private int playersArePlaying = 0;
		private bool IsGameStartet = false;
		private bool IsGameOver= false;

	    public bool IsDebugEndless = false;

		private void Awake()
		{
			GameManagerAudioSource = GetComponent<AudioSource>();
			GameManagerAudioSource.playOnAwake = false;
			GameManagerAudioSource.loop = true;

			SpawnPoints = new Transform[4];
			PlayerDatas = new PlayerData[4];
			for (int id = 0; id < PlayerDatas.Length; id++)
				PlayerDatas[id] = new PlayerData(true);
			SpawnPointsUsed = new bool[4];
			for (int id = 0; id < SpawnPointsUsed.Length; id++)
				SpawnPointsUsed[id] = false;

			SceneManager.sceneLoaded += OnLevelLoaded;
		}
		private void Start()
		{
			GameManagerAudioSource.outputAudioMixerGroup = GameHUB.Instance.MusicAudioMixerGroup;

		}

		private void Update()
		{

			if (!IsDebugEndless && IsGameStartet && playersArePlaying <= 1)
				GameOver();
			if (IsGameOver && Input.GetButtonDown("ControllerStart"))
				BackToSelectMenu();
		}

		private void OnLevelLoaded(Scene _actualScene, LoadSceneMode _loadSceneMode)
		{
			switch (_actualScene.name)
			{
				case "MainMenu":
					PlayerDatas[0].IsLoggedIn = false;
					PlayerDatas[1].IsLoggedIn = false;
					PlayerDatas[2].IsLoggedIn = false;
					PlayerDatas[3].IsLoggedIn = false;
					playersArePlaying = 0;
					IsGameStartet = false;
					IsGameOver = false;
					PlayMusic(MenuMusic);
					break;
				case "CharacterSelection":
					PlayerDatas[0].IsLoggedIn = false;
					PlayerDatas[1].IsLoggedIn = false;
					PlayerDatas[2].IsLoggedIn = false;
					PlayerDatas[3].IsLoggedIn = false;
					playersArePlaying = 0;
					IsGameStartet = false;
					IsGameOver = false;
					PlayMusic(MenuMusic);
					break;
				default:
					StartGame();
					break;
			}
		}

		private void PlayMusic(AudioClip _audioClip)
		{
		    if (GameManagerAudioSource != null)
            {
                GameManagerAudioSource.clip = _audioClip;
                GameManagerAudioSource.Play();
            }
		}

		//InGame
		private void StartGame()
		{
			SpawnHUD();
			for (int id = 0; id < SpawnPointsUsed.Length; id++)
				SpawnPointsUsed[id] = false;

			PlayMusic(InGameMusic);
			SetPlayerSpawnPoints();
			SpawnAllPlayer();

			IsGameStartet = true;
            GameHUB.Instance.BeatMaster.ResetProgress();
            GameHUB.Instance.BeatMaster.enabled = true;

        }
		private void SetPlayerSpawnPoints()
		{
			SpawnPoints[0] = GameObject.FindGameObjectWithTag("SpawnPoint1").transform;
			SpawnPoints[1] = GameObject.FindGameObjectWithTag("SpawnPoint2").transform;
			SpawnPoints[2] = GameObject.FindGameObjectWithTag("SpawnPoint3").transform;
			SpawnPoints[3] = GameObject.FindGameObjectWithTag("SpawnPoint4").transform;
		}
		private void SpawnAllPlayer()
		{
			SpwanPlayer(PlayerId.Player1);
			SpwanPlayer(PlayerId.Player2);
			SpwanPlayer(PlayerId.Player3);
			SpwanPlayer(PlayerId.Player4);

		}
		private void SpwanPlayer(PlayerId _playerId)
		{
			if (!PlayerDatas[(int)_playerId].IsLoggedIn)
				return;
		    var otherPlayers = FindObjectsOfType<PlayerController>();

            playersArePlaying++;

			int spawnPointId = -1;
			do
			{
				spawnPointId = Random.Range(0, 4);
				if (!SpawnPointsUsed[spawnPointId])
				{
					SpawnPointsUsed[spawnPointId] = true;
				}
				else
					spawnPointId = -1;
			}
			while (spawnPointId == -1);

			GameObject newPlayer = (GameObject)Instantiate(PlayerPrefab, SpawnPoints[spawnPointId].position, SpawnPoints[spawnPointId].rotation);
            var playerController = newPlayer.GetComponent<PlayerController>();
		    playerController.SkinId = PlayerDatas[(int) _playerId].MaterialId;
            playerController.InitialicePlayer(_playerId, HUDInstanz);

		    var profiles = GameHUB.Instance.SoundLibrary.SumoProfiles;
            SumoSoundProfile profile = null;
            bool soundProfileInUse = false;
		    int trial = 0;
		    do
		    {
		        profile = profiles.GetRandom();
		        soundProfileInUse = false;

		        foreach (var player in otherPlayers)
		        {
		            if (player != null && player.SoundProfile == profile)
		            {
		                soundProfileInUse = true;
		                break;
		            }
		        }
		        trial++;

		    } while (soundProfileInUse && trial < 100);

		    playerController.SoundProfile = profile;


		}

		private void SpawnHUD()
		{
			GameObject newHUD = Instantiate(HUDPrefab, new Vector3(9000.0f, 9000.0f, 9000.0f), Quaternion.identity);
			HUDInstanz = newHUD.GetComponentInChildren<HUD>();

			if (PlayerDatas[0].IsLoggedIn)
			{
				HUDInstanz.SetEnableCharacter(PlayerId.Player1, true);
				HUDInstanz.SetCharacterImage(PlayerId.Player1, CharacterSprite[PlayerDatas[0].MaterialId]);
			}
			if (PlayerDatas[1].IsLoggedIn)
			{
				HUDInstanz.SetEnableCharacter(PlayerId.Player2, true);
				HUDInstanz.SetCharacterImage(PlayerId.Player2, CharacterSprite[PlayerDatas[1].MaterialId]);
			}
			if (PlayerDatas[2].IsLoggedIn)
			{

				HUDInstanz.SetEnableCharacter(PlayerId.Player3, true);
				HUDInstanz.SetCharacterImage(PlayerId.Player3, CharacterSprite[PlayerDatas[2].MaterialId]);
			}
			if (PlayerDatas[3].IsLoggedIn)
			{
				HUDInstanz.SetEnableCharacter(PlayerId.Player4, true);
				HUDInstanz.SetCharacterImage(PlayerId.Player4, CharacterSprite[PlayerDatas[3].MaterialId]);
			}
		}

		private void GameOver()
		{
			if (!IsGameOver)
			{
				HUDInstanz.WinScreen.SetActive(true);
				PlayerId playerId = GameObject.FindObjectOfType<PlayerController>().PlayerIdType;
				int imageId = PlayerDatas[(int)playerId].MaterialId;
				HUDInstanz.WinPlayerImage.sprite = CharacterSprite[imageId];
				IsGameOver = true;

			    GameHUB.Instance.BeatMaster.enabled = false;
			}
		}

		private void BackToSelectMenu()
		{
			SceneManager.LoadScene(1);
		}
	}
}