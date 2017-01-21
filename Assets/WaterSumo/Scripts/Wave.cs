using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{
    public class Wave : MonoBehaviour
    {
        public float InnerRadiusMaximum { get { return RadiusMaximum - WaveLengthRadius; } }
        public float InnerRadiusCurrent { get { return RadiusCurrent - WaveLengthRadius; } }
        public float RadiusCurrent { get { return currentSize * 0.5f; } }
        public float RadiusMaximum { get { return maxSize * 0.5f; } }
        public float WaveLengthRadius { get { return waveLength * RadiusMaximum; } }
        
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

        //Fade alpha of inner and outer sphere, then destroy both
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
            var colliders = Physics.OverlapSphere(transform.position, RadiusCurrent, layerMask);

            foreach (var collider in colliders)
            {
                var waveToCollider = collider.transform.position - transform.position;
                var waveToColliderN = waveToCollider.normalized;
                var body = collider.GetComponent<Rigidbody>();

                if (body == null)
                    continue;
                
                var capsuleCollider = collider as CapsuleCollider;
                float affectedByWave = 0f;
                if (capsuleCollider != null && waveToCollider.magnitude - capsuleCollider.radius > InnerRadiusCurrent)
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

        void OnDrawGizmos()
        {
            Color prevColor = Gizmos.color;

            if (!Application.isPlaying)
            {
                Gizmos.color = Color.blue;
                DrawCircleGizmo(transform.position, RadiusMaximum);
                Gizmos.color = Color.cyan;
                DrawCircleGizmo(transform.position, InnerRadiusMaximum);
            }
            else
            {
                Gizmos.color = new Color(0.1f, 0f, 1f, 0.2f);
                DrawCircleGizmo(transform.position, RadiusMaximum);
                Gizmos.color = Color.blue;
                DrawCircleGizmo(transform.position, RadiusCurrent);
                Gizmos.color = Color.cyan;
                DrawCircleGizmo(transform.position, InnerRadiusCurrent);
            }

            Gizmos.color = prevColor;
        }

        void DrawCircleGizmo(Vector3 center, float radius)
        {
            if (radius < 0f)
                return;
            int resolution = Mathf.FloorToInt(radius) * 8;
            for (int i = 1; i <= resolution; i++)
            {
                float prevAngle = Mathf.InverseLerp(0, resolution, i - 1) * 2f * Mathf.PI;
                float curAngle = Mathf.InverseLerp(0, resolution, i) * 2f * Mathf.PI;

                Vector3 prevPoint = new Vector3(Mathf.Cos(prevAngle), 0f, Mathf.Sin(prevAngle)) * radius;
                Vector3 curPoint = new Vector3(Mathf.Cos(curAngle), 0f, Mathf.Sin(curAngle)) * radius;

                Gizmos.DrawLine(center + prevPoint, center + curPoint);
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
