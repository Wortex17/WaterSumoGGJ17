using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{
    public class WaveAffected : MonoBehaviour
    {
        public bool IsPushedByWaves = true;
        public float WavePushAffection = 1f;

        public void ConsumeWave(Vector3 waveOrigin, float waveStrength)
        {
            Vector3 waveVector = transform.position - waveOrigin;
            if (IsPushedByWaves)
            {
                var rigidBody = GetComponent<Rigidbody>();
                if (rigidBody != null)
                {
                    rigidBody.AddForce(waveVector.normalized * waveStrength * WavePushAffection, ForceMode.Force);
                }
            }
        }
    }

}