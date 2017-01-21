using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace WaterSumo
{
	public enum PlayerId { Player1, Player2, Player3, Player4, None }

	public class GameManager : MonoBehaviour
	{
		//Player Prefab
		[SerializeField]
		private GameObject PlayerPrefab;

		//LoggedIn states
		public bool[] PlayersLoggedIn;

		//SpwanPoints
		private Transform[] SpawnPoints;

		private void Awake()
		{
			DontDestroyOnLoad(this);
			SpawnPoints = new Transform[4];
			PlayersLoggedIn = new bool[4];
			for (int id = 0; id < PlayersLoggedIn.Length; id++)
				PlayersLoggedIn[id] = true;


			SceneManager.sceneLoaded += OnLevelLoaded;
		}

		private void Update()
		{

		}

		private void OnLevelLoaded(Scene _actualScene, LoadSceneMode _loadSceneMode)
		{
			switch (_actualScene.name)
			{
				case "MaineMenu":
					PlayersLoggedIn[0] = true;
					PlayersLoggedIn[1] = false;
					PlayersLoggedIn[2] = false;
					PlayersLoggedIn[3] = false;
					break;
				default:
					StartGame();
					break;
			}
			Debug.Log("OnLevelLoaded");
		}

		//InGame
		private void StartGame()
		{
			SetPlayerSpawnPoints();
			SpawnAllPlayer();
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
			print("SpwanPlayer");
			if (!PlayersLoggedIn[(int)_playerId])
				return;

			GameObject newPlayer = (GameObject)Instantiate(PlayerPrefab, SpawnPoints[(int)_playerId].position, SpawnPoints[(int)_playerId].rotation);
			newPlayer.GetComponent<PlayerController>().InitialicePlayer(_playerId);


		}

	}
}
