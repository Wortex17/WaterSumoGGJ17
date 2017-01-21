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
			MaterialId = -1;
		}

	}

	public class GameManager : MonoBehaviour
	{
		//Player Prefab
		[SerializeField]
		private GameObject PlayerPrefab;

		//LoggedIn states
		public PlayerData[] PlayersLoggedIn;

		//SpwanPoints
		private Transform[] SpawnPoints;
		private bool[] SpawnPointsUsed;

		//SwimmingRing Materials
		[SerializeField]
		private Material[] SwimmingRingMaterials;


		private void Awake()
		{
			SpawnPoints = new Transform[4];
			PlayersLoggedIn = new PlayerData[4];
			for (int id = 0; id < PlayersLoggedIn.Length; id++)
				PlayersLoggedIn[id] = new PlayerData(true);
			SpawnPointsUsed = new bool[4];
			for (int id = 0; id < SpawnPointsUsed.Length; id++)
				SpawnPointsUsed[id] = false;


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
					PlayersLoggedIn[0].IsLoggedIn = true;
					PlayersLoggedIn[1].IsLoggedIn = false;
					PlayersLoggedIn[2].IsLoggedIn = false;
					PlayersLoggedIn[3].IsLoggedIn = false;
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
			for (int id = 0; id < SpawnPointsUsed.Length; id++)
				SpawnPointsUsed[id] = false;

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
			if (!PlayersLoggedIn[(int)_playerId].IsLoggedIn)
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

			GameObject newPlayer = (GameObject)Instantiate(PlayerPrefab, SpawnPoints[spawnPointId].position, SpawnPoints[spawnPointId].rotation);
			newPlayer.GetComponent<PlayerController>().InitialicePlayer(_playerId, SwimmingRingMaterials[PlayersLoggedIn[(int)_playerId].MaterialId]);


		}

	}
}
