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
                float slopeHeight = wave.GetSlopeHeight(waveVector.magnitude);
                Vector3 position = this.transform.position;

                //var ray = new Ray();
                //Physics.Raycast()

                //GetComponent<Rigidbody>().AddForce(Vector3.up * slopeHeight, ForceMode.Acceleration);

                this.transform.position = position;
            }

            hasConsumedWave = true;
        }

        protected void LateUpdate()
        {
            hasConsumedWave = false;
        }


        private bool hasConsumedWave = false;
    }

}