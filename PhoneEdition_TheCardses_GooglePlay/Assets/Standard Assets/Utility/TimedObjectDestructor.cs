using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class TimedObjectDestructor : MonoBehaviour
    {
        [SerializeField] private float m_TimeOut = 1.0f;
        [SerializeField] private bool m_DetachChildren = false;
		[SerializeField] private bool m_MindParticleSystems = false;

        private void Awake()
        {
            Invoke("DestroyNow", m_TimeOut);
        }


        private void DestroyNow()
        {
            if (m_DetachChildren)
            {
                transform.DetachChildren();
            }
			if (m_MindParticleSystems) {
				ParticleSystem[] myParts = GetComponentsInChildren<ParticleSystem> ();

				for (int i = 0; i < myParts.Length; i++) {
					myParts [i].transform.SetParent (null);
					myParts [i].Stop ();
					Destroy (myParts[i].gameObject, 4f);
				}
			}

            DestroyObject(gameObject);
        }
    }
}
