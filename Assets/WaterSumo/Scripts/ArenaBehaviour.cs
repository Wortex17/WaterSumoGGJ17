using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EShape {Capsule = 0, Box = 1}

[ExecuteInEditMode]
public class ArenaBehaviour : MonoBehaviour {

	[SerializeField, Range(0, 100)]
	private float CapsuleColliderRadius = 0.5f;
	[SerializeField]
	private float CapsuleColliderHeight = 5.0f;

	[SerializeField, Range(0, 100)]
	private float BoxColliderSize = 1.0f;

	private CapsuleCollider capsuleColl;
	private BoxCollider boxColl;

	[SerializeField]
	private EShape shape;
	
	// Update is called once per frame
	void Update () {

		ModifyColl ();

	}

	void OnValidate()
	{
		
	}

	void ModifyColl()
	{
		switch ((int)shape)
		{
		case 0:
			if (GetComponent<BoxCollider> () != null)
				GameObject.DestroyImmediate (GetComponent<BoxCollider> ());
			if (GetComponent<CapsuleCollider> () == null)
				capsuleColl = gameObject.AddComponent<CapsuleCollider> ();
			else
				capsuleColl = GetComponent<CapsuleCollider> ();
			
			capsuleColl.isTrigger = true;
			capsuleColl.radius = CapsuleColliderRadius;
			capsuleColl.height = CapsuleColliderHeight;

			break;
		case 1:
			if (GetComponent<CapsuleCollider> () != null)
				GameObject.DestroyImmediate (GetComponent<CapsuleCollider> ());
			if (GetComponent<BoxCollider> () == null)
				boxColl = gameObject.AddComponent<BoxCollider> ();
			else
				boxColl = GetComponent<BoxCollider> ();
			
			boxColl.isTrigger = true;
			boxColl.size = new Vector3 (BoxColliderSize, 0.1f, BoxColliderSize);

			break;
		}
	}

	public float DistanceToBorder(GameObject _player, Collider _coll)
	{
		Vector3 closestPoint = _coll.ClosestPointOnBounds(_player.transform.position);
		float distance = Vector3.Distance(closestPoint, _player.transform.position);
		Debug.Log ("Dist" + distance);
		return distance;
	}
}
