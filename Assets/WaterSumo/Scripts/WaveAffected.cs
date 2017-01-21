using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{
    public class WaveAffected : MonoBehaviour
    {
        public bool IsPushedByWaves = true;
        public float WavePushAffection = 1f;
        public bool RidesOnWaveSlope = true;

        public float buoyancyOffset = 0.5f;

        public bool HasConsumedWave
        {
            get { return hasConsumedWave; }
        }
        public bool IsRidingOnWave
        {
            get { return hasConsumedWave; }
        }

        public void ConsumeWave(Wave wave, float waveStrength)
        {
            Vector3 waveVector = transform.position - wave.transform.position;
            if (IsPushedByWaves)
            {
                var rigidBody = GetComponent<Rigidbody>();
                if (rigidBody != null)
                {
                    rigidBody.AddForce(waveVector.normalized * waveStrength * WavePushAffection, ForceMode.Force);
                }
            }

            if (RidesOnWaveSlope)
            {
                slopeHeight = wave.GetSlopeHeight(waveVector.magnitude);
            }

            hasConsumedWave = true;
            hasConsumedWaveLock = true;
        }

        protected void FixedUpdate()
        {
            if (RidesOnWaveSlope && slopeHeight > 0f)
            {
                var rigidBody = GetComponent<Rigidbody>();
                if (rigidBody != null)
                {
                    //Cancel out gravity
                    rigidBody.AddForce(-Physics.gravity, ForceMode.Acceleration);
                }
                transform.position = new Vector3(transform.position.x, buoyancyOffset + slopeHeight, transform.position.z);

            }
        }

        protected void LateUpdate()
        {
            if (hasConsumedWaveLock)
            {
                hasConsumedWaveLock = false;
            }
            else
            {
                hasConsumedWave = false;
                slopeHeight = 0f;
            }
        }

        private float slopeHeight = 0f;
        private bool hasConsumedWave = false;
        private bool hasConsumedWaveLock = false;
    }

}