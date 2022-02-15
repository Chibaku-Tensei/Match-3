using System;
using NaughtyAttributes;
using UnityEngine;


namespace Base
{
    [RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
    public class Collectible : MonoBehaviour
    {
        [Tag] public string filterTag;

        protected void OnTriggerEnter(Collider other)
        {
            if (filterTag != String.Empty)
            {
                if (other.CompareTag(filterTag))
                {
                    gameObject.SetActive(false);
                }
            }
        }

        protected void OnCollisionEnter(Collision other)
        {
            if (filterTag != String.Empty)
            {
                if (other.collider.CompareTag(filterTag))
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}

