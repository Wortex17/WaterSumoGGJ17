using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private bool GOCheck;

	// Use this for initialization
	void Start () {

        if (GetComponent<CapsuleCollider> () != null)
		{
			capsuleColl = GetComponent<CapsuleCollider> ();
			GOCheck = false;
            capsuleColl.isTrigger = true;
        }

        if (GetComponent<BoxCollider> () != null)
		{
			GOCheck = true;
			boxColl = GetComponent<BoxCollider> ();
            boxColl.isTrigger = true;
        }

	}
	
	// Update is called once per frame
	void Update () {

		ModifyColl ();

	}

	void OnValidate()
	{
		
	}

	void ModifyColl()
	{
		if (GOCheck)
		{
			boxColl.size = new Vector3 (BoxColliderSize, 0.1f, BoxColliderSize);
        } 
		else
		{
			capsuleColl.radius = CapsuleColliderRadius;
			capsuleColl.height = CapsuleColliderHeight;
		}
	}
}
