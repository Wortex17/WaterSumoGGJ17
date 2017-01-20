using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{
    public class Wave : MonoBehaviour
    {

        public void Reset()
        {
            currentSize = 0f;
            currentLifetime = 0f;
        }


        private void Progress(float grow)
        {
            currentLifetime += grow;

            if (currentLifetime > maxLifetime)
            {
                Reset();
                enabled = false;
                return;
            }

            float lifetime01 = Mathf.InverseLerp(0f, maxLifetime, currentLifetime);
            currentSize = lifetime01 * maxSize;

            transform.localScale = Vector3.one * currentSize;
        }

        void Start()
        {
            Reset();
        }

        void Update()
        {
            Progress(Time.deltaTime);
        }

        void OnValidate()
        {
            if (outerSphere != null)
            {
                outerSphere.radius = 1f;
            }
            if (outerSphere != null)
            {
                innerSphere.radius = 1f - waveLength;
            }
        }

        private float currentSize = 0f;
        private float currentLifetime = 0f;
        [SerializeField]
        private float maxSize = 1f;
        [SerializeField]
        private float maxLifetime = 1f;

        [SerializeField, Range(0f, 1f)]
        private float waveLength = 0.2f;

        [SerializeField]
        private SphereCollider innerSphere;
        [SerializeField]
        private SphereCollider outerSphere;
    }
}
