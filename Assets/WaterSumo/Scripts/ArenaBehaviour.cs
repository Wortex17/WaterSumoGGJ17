using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBehaviour : MonoBehaviour {
    public enum EShape { Capsule = 0, Box = 1 }
    
    [SerializeField, Range(0, 100)]
    private float ArenaSize = 1.0f;

    [SerializeField]
	private EShape shape;

	void Start ()
    {
        ModifyColl();
        GenerateBorder();
    }

	void OnValidate()
	{
	    ModifyColl();
	    GenerateBorder();
	}

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

    private Collider CurrentCollider {
        get {
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
                    boxCollider.size = new Vector3(ArenaSize*2f, 20f, ArenaSize*2f);
                }

			    break;
		}
	}

	public Vector3 DistanceToBorder(GameObject _player)
	{
		Vector3 tempPlayerPos = _player.transform.position;
		tempPlayerPos.y = 0;
		Vector3 closestPoint = CurrentCollider.ClosestPointOnBounds(tempPlayerPos * -1);
	    Vector3 distance = closestPoint - tempPlayerPos;

		switch ((int)shape) 
		{
		case 0:
			if (Mathf.Abs (distance.x) > ArenaSize)
				Destroy (_player);
			if (Mathf.Abs (distance.z) > ArenaSize)
				Destroy (_player);
			break;
		case 1:
			if (Mathf.Abs(distance.x) > ArenaSize)
				Destroy (_player);
			if (Mathf.Abs(distance.z) > ArenaSize)
				Destroy (_player);
			break;
		}
		return distance;
    }

    [SerializeField]
    private CapsuleCollider capsuleCollider = null;
    [SerializeField]
    private BoxCollider boxCollider = null;
}
