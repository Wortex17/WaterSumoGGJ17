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
		private GameObject[] PlayerPrefabs;

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
		[HideInInspector]
		public int PlayerArePlaying = 0;
		private bool IsGameStartet = false;

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
			if (IsGameStartet && PlayerArePlaying <= 1)
				GameOver();
		}

		private void OnLevelLoaded(Scene _actualScene, LoadSceneMode _loadSceneMode)
		{
			switch (_actualScene.name)
			{
				case "MaineMenu":
					PlayerDatas[0].IsLoggedIn = true;
					PlayerDatas[1].IsLoggedIn = false;
					PlayerDatas[2].IsLoggedIn = false;
					PlayerDatas[3].IsLoggedIn = false;
					PlayerArePlaying = 0;
					IsGameStartet = false;
					PlayMusic(MenuMusic);
					break;
				case "CharacterSelection":
					PlayerDatas[0].IsLoggedIn = true;
					PlayerDatas[1].IsLoggedIn = false;
					PlayerDatas[2].IsLoggedIn = false;
					PlayerDatas[3].IsLoggedIn = false;
					PlayerArePlaying = 0;
					IsGameStartet = false;
					PlayMusic(MenuMusic);
					break;
				default:
					StartGame();
					break;
			}
			Debug.Log("OnLevelLoaded");
		}

		private void PlayMusic(AudioClip _audioClip)
		{
			GameManagerAudioSource.clip = _audioClip;
			GameManagerAudioSource.Play();
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

			GameObject newPlayer = (GameObject)Instantiate(PlayerPrefabs[PlayerDatas[(int)_playerId].MaterialId], SpawnPoints[spawnPointId].position, SpawnPoints[spawnPointId].rotation);
			newPlayer.GetComponent<PlayerController>().InitialicePlayer(_playerId, HUDInstanz);
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
			Debug.Log("GameOver");
		}
	}
}
