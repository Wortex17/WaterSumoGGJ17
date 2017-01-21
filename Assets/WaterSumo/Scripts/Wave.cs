using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Serialization;

namespace WaterSumo
{
    public class Wave : MonoBehaviour
    {
        public float InnerRadiusMaximum { get { return RadiusMaximum - WaveLengthRadius; } }
        public float InnerRadiusCurrent { get { return RadiusCurrent - WaveLengthRadius; } }
        public float RadiusCurrent { get { return currentSize * 0.5f; } }
        public float RadiusMaximum { get { return maxSize * 0.5f; } }
        public float WaveLengthRadius { get { return waveLength * RadiusMaximum; } }

        public float Radius { get { return Application.isPlaying ? RadiusCurrent : RadiusMaximum; } }
        public float InnerRadius { get { return Application.isPlaying ? InnerRadius : InnerRadiusMaximum; } }

        public float CurrentLifetime01 {  get { return Mathf.InverseLerp(0f, maxLifetime, currentLifetime); } }

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

                if (autoDestroy)
                    Destroy(this.gameObject);
                return;
            }

            float lifetime01 = Mathf.InverseLerp(0f, maxLifetime, currentLifetime);
            currentSize = lifetime01 * maxSize;

            Vector3 scale = Vector3.one*currentSize;
            transform.localScale = scale;
        }

        private void ApplyVisuals()
        {
            if (waveParticleSystem != null)
            {
                var main = waveParticleSystem.main;
                float startSpeed = Mathf.Abs(main.startSpeedMultiplier);
                float neededLifetime = WaveLengthRadius / (startSpeed > 0f ? startSpeed : 1f);
                main.startLifetimeMultiplier = Mathf.Max(neededLifetime, 0f) + Mathf.Max(0.1f, waveTrailExtra);
                
                var shape = waveParticleSystem.shape;
                shape.radius = Radius;
            }
        }

        void OnEnable()
        {
            if (waveParticleSystem != null)
            {
                /*
                var main = waveParticleSystem.main;
                main.loop = true;
                var emission = waveParticleSystem.emission;
                emission.enabled = true;
                */

                waveParticleSystem.Play();
            }
        }

        void OnDisable()
        {
            if (waveParticleSystem != null)
            {
                /*
                var main = waveParticleSystem.main;
                main.loop = false;
                var emission = waveParticleSystem.emission;
                emission.enabled = false;
                */

                waveParticleSystem.Stop();
            }

            Reset();
        }

        void Update()
        {
            Progress(Time.deltaTime);
            var colliders = Physics.OverlapSphere(transform.position, RadiusCurrent, layerMask);
            
            float strength = pushStrengthFactor * pushStrengthOverLifetime.Evaluate(CurrentLifetime01);

            foreach (var collider in colliders)
            {
                var waveToCollider = collider.transform.position - transform.position;
                var waveToColliderN = waveToCollider.normalized;
                var body = collider.GetComponent<Rigidbody>();

                if (body == null)
                    continue;
                
                var capsuleCollider = collider as CapsuleCollider;
                float affectedByWave = 0f;
                if (capsuleCollider != null && (InnerRadiusCurrent >0f && waveToCollider.magnitude - capsuleCollider.radius > InnerRadiusCurrent))
                {
                    affectedByWave = 1f;
                }

                var affected = capsuleCollider.GetComponent<WaveAffected>();
                if (affected != null)
                {
                    if(affected.IsPushedByWaves)
                        body.AddForce(waveToColliderN * strength * affectedByWave, ForceMode.Force);
                }
            }
            ApplyVisuals();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (UnityEditor.EditorUtility.IsPersistent(this))
                return;
            ApplyVisuals();
        }
#endif

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
        
        [SerializeField]
        private bool autoDestroy = true;

        private float currentSize = 0f;
        private float currentLifetime = 0f;
        [SerializeField]
        private float maxSize = 1f;
        [SerializeField]
        private float maxLifetime = 1f;

        [SerializeField, Range(0f, 1f)]
        private float waveLength = 0.2f;
        [SerializeField]
        private float waveTrailExtra = 0.2f;
        [SerializeField, FormerlySerializedAs("pushStrength")]
        private float pushStrengthFactor = 1f;
        [SerializeField]
        private AnimationCurve pushStrengthOverLifetime = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        [SerializeField]
        private LayerMask layerMask = Physics.AllLayers;

        [SerializeField]
        private ParticleSystem waveParticleSystem;
    }
}
