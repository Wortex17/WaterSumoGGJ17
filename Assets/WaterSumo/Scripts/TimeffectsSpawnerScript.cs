using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WaterSumo
{
    public class TimeffectsSpawnerScript : MonoBehaviour
    {

        public List<GameObject> PickUp = new List<GameObject>();

        [SerializeField]
        private float SpawnTimer = 1.0f;
        private int _tmpCounter;
        private PickupBase lastSpawned;

        // Use this for initialization
        void Start()
        {
            InvokeRepeating("Spawn", 0f, SpawnTimer);
        }

        void Spawn()
        {
            GameObject Picked = PickUp.GetRandom();
            if (lastSpawned == null || lastSpawned.PickUpCollected)
            lastSpawned = Instantiate(Picked, this.gameObject.transform.position, Picked.transform.rotation).GetComponent<PickupBase>();
        }
    }
}
