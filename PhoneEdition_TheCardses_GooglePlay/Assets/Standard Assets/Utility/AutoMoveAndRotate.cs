using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class AutoMoveAndRotate : MonoBehaviour
    {
        public Vector3andSpace moveUnitsPerSecond;
        public Vector3andSpace rotateDegreesPerSecond;
        public bool ignoreTimescale;
        private float m_LastRealTime;
		public bool randomStartRotation;


        private void Start()
        {
            m_LastRealTime = Time.realtimeSinceStartup;

			if (randomStartRotation) {
				transform.Rotate (rotateDegreesPerSecond.value.normalized * UnityEngine.Random.Range (0f, 360f));
			}
        }


        // Update is called once per frame
        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if (ignoreTimescale)
            {
                deltaTime = (Time.realtimeSinceStartup - m_LastRealTime);
                m_LastRealTime = Time.realtimeSinceStartup;
            }
			if (moveUnitsPerSecond.space == Space.Self) {
				transform.localPosition += (moveUnitsPerSecond.value * deltaTime);
			} else {
				transform.position += (moveUnitsPerSecond.value * deltaTime);
			}
            transform.Rotate(rotateDegreesPerSecond.value*deltaTime, rotateDegreesPerSecond.space);
        }


        [Serializable]
        public class Vector3andSpace
        {
            public Vector3 value;
            public Space space = Space.Self;
        }
    }
}
