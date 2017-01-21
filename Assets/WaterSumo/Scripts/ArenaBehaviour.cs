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
	private Collider coll;

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
			coll = capsuleColl;
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
			coll = boxColl;
			break;
		}
	}

	public Vector3 DistanceToBorder(GameObject _player)
	{
		Vector3 tempPlayerPos = _player.transform.position;
		tempPlayerPos.y = 0;
		Vector3 closestPoint = coll.ClosestPointOnBounds(tempPlayerPos * -1);
		Vector3 distance = closestPoint - tempPlayerPos;

		Debug.Log (distance);

		switch ((int)shape) 
		{
		case 0:
			if (Mathf.Abs (distance.x) > CapsuleColliderRadius)
				Destroy (_player);
			if (Mathf.Abs (distance.z) > CapsuleColliderRadius)
				Destroy (_player);
			break;
		case 1:
			if (Mathf.Abs(distance.x) > BoxColliderSize)
				Destroy (_player);
			if (Mathf.Abs(distance.z) > BoxColliderSize)
				Destroy (_player);
			break;
		}
		return distance;
	}
}
