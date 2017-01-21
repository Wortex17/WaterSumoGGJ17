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
                StartCoroutine(FadeTo(0.0f, 0.5f));               
                return;
            }

            float lifetime01 = Mathf.InverseLerp(0f, maxLifetime, currentLifetime);
            currentSize = lifetime01 * maxSize;

            Vector3 scale = Vector3.one*currentSize;
            scale.y = 0.1f;
            transform.localScale = scale;
        }

        IEnumerator FadeTo(float aValue, float aTime)
        {
            float alphaOuter = outerSphere.GetComponent<Renderer>().material.color.a;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alphaOuter, aValue, t));
                outerSphere.GetComponent<Renderer>().material.color = newColor;
                yield return null;
            }
            float alphaInner = innerSphere.GetComponent<Renderer>().material.color.a;
            for (float t = 0.0f; t<1.0f; t += Time.deltaTime / aTime)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alphaInner, aValue, t));
                innerSphere.GetComponent<Renderer>().material.color = newColor;
                yield return null;
            }

            DestroyImmediate(outerSphere);
            DestroyImmediate(innerSphere);
        }

        void Start()
        {
            Reset();
        }

        void Update()
        {
            Progress(Time.deltaTime);
            var colliders = Physics.OverlapSphere(transform.position, transform.TransformVector(Vector3.one*currentSize).magnitude, layerMask);

            foreach (var collider in colliders)
            {
                var waveToCollider = collider.transform.position - transform.position;
                var waveToColliderN = waveToCollider.normalized;
                var body = collider.GetComponent<Rigidbody>();

                if (body == null)
                    continue;

                float innerRadius = currentSize - maxSize*waveLength;
                var capsuleCollider = collider as CapsuleCollider;
                float affectedByWave = 0f;
                if (capsuleCollider != null && waveToCollider.magnitude - capsuleCollider.radius > innerRadius)
                {
                    affectedByWave = 1f;
                }
                body.AddForce(waveToColliderN * pushStrength * affectedByWave, ForceMode.Force);
            }
        }

        void OnValidate()
        {
            Vector3 scale = new Vector3(1f, 0f, 1f);
            if (outerSphere != null)
            {
                outerSphere.transform.localScale = scale * 1f + new Vector3(0f, 0.1f, 0f);
            }
            if (innerSphere != null)
            {
                innerSphere.transform.localScale = scale * (1f - waveLength) + new Vector3(0f, 0.1f, 0f);
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
        private float pushStrength = 1f;
        [SerializeField]
        private LayerMask layerMask = Physics.AllLayers;

        [SerializeField]
        private GameObject innerSphere;
        [SerializeField]
        private GameObject outerSphere;
    }
}
