using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{
	public class ArenaBehaviour : MonoBehaviour
	{
		public enum EShape { Capsule = 0, Box = 1 }

		[SerializeField, Range(0, 100)]
		private float ArenaSize = 1.0f;

		[SerializeField]
		private EShape shape;

		private Vector3 closestPo;

		void Start()
		{
			ModifyColl();
			GenerateBorder();
		}

#if UNITY_EDITOR
		void OnValidate()
		{
			if (UnityEditor.EditorUtility.IsPersistent(this))
				return;
			ModifyColl();
			GenerateBorder();
		}
#endif

		void GenerateBorder()
		{
			var lineRenderer = GetComponent<LineRenderer>();
			if (lineRenderer == null)
				return;
			lineRenderer.numPositions = 0;
			List<Vector3> positions = new List<Vector3>();
			switch (shape)
			{
				case EShape.Box:
					positions.Add(new Vector3(-1f, 0f, -1f) * ArenaSize);
					positions.Add(new Vector3(-1f, 0f, 1f) * ArenaSize);
					positions.Add(new Vector3(1f, 0f, 1f) * ArenaSize);
					positions.Add(new Vector3(1f, 0f, -1f) * ArenaSize);
					positions.Add(positions[0]);
					lineRenderer.numPositions = 4;
					lineRenderer.SetPositions(positions.ToArray());
					break;
				case EShape.Capsule:
					int resolution = Mathf.FloorToInt(ArenaSize) * 8;
					for (int i = 0; i <= resolution; i++)
					{
						float curAngle = Mathf.InverseLerp(0, resolution, i) * 2f * Mathf.PI;
						Vector3 curPoint = new Vector3(Mathf.Cos(curAngle), 0f, Mathf.Sin(curAngle)) * ArenaSize;
						positions.Add(curPoint);
					}
					break;
			}
			lineRenderer.numPositions = positions.Count;
			lineRenderer.SetPositions(positions.ToArray());
		}

		private Collider CurrentCollider
		{
			get
			{
				switch (shape)
				{
					case EShape.Box:
						return boxCollider;
					case EShape.Capsule:
						return capsuleCollider;
					default:
						return null;
				}
			}
		}

		void ModifyColl()
		{
			switch (shape)
			{
				case EShape.Capsule:
					if (boxCollider != null)
					{
						boxCollider.enabled = false;
					}
					if (capsuleCollider != null)
					{
						capsuleCollider.enabled = true;

						capsuleCollider.isTrigger = true;
						capsuleCollider.radius = ArenaSize;
						capsuleCollider.height = 20f;
					}
					break;
				case EShape.Box:
					if (capsuleCollider != null)
					{
						capsuleCollider.enabled = false;
					}

					if (boxCollider != null)
					{
						boxCollider.enabled = true;

						boxCollider.isTrigger = true;
						boxCollider.size = new Vector3(ArenaSize * 2f, 20f, ArenaSize * 2f);
					}

					break;
			}
		}
		void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(closestPo, 1);
		}
		public Vector3 DistanceToBorder(GameObject _player)
		{
			Vector3 tempPlayerPos = _player.transform.position;
			tempPlayerPos.y = 0;

			Vector3 dirToPlayer = tempPlayerPos - CurrentCollider.bounds.center;
			dirToPlayer = dirToPlayer.normalized * 10000.0f;
			Vector3 closestPointToColl = CurrentCollider.ClosestPointOnBounds(dirToPlayer);
			closestPo = closestPointToColl;
			Vector3 finalDir = closestPointToColl - tempPlayerPos;

			switch (shape)
			{
				case EShape.Capsule:
					CapsuleCollider temp = (CapsuleCollider)CurrentCollider;
					if (temp.radius - Vector3.Distance(tempPlayerPos, temp.bounds.center) < 0)
						PlayerDie(_player);
					break;
				case EShape.Box:
					if (Mathf.Abs(finalDir.x) <= 0.06f)
						PlayerDie(_player);
					if (Mathf.Abs(finalDir.z) <= 0.06f)
						PlayerDie(_player);
					break;
			}
			return finalDir;
		}

		private void PlayerDie(GameObject _player)
		{
			GameHUB.Instance.GameManager.PlayerArePlaying--;
			Destroy(_player);
		}

		[SerializeField]
		private CapsuleCollider capsuleCollider = null;
		[SerializeField]
		private BoxCollider boxCollider = null;
	}
}