using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    public class CameraFollow : BaseMono
    {
        [SerializeField] protected Transform followTarget;
        [SerializeField] protected Vector3 offset;
        [SerializeField] protected float smoothSpeed = .125f;

        public void InitOffset()
        {
            offset = followTarget.position - Position;
        }

        public void FollowTarget()
        {
            if (offset != Vector3.zero)
            {
                Vector3 desiredPos = followTarget.position - offset;
                Vector3 smoothPos = Vector3.Lerp(Position, desiredPos, smoothSpeed);
                Position = new Vector3(smoothPos.x, Position.y, smoothPos.z);
            }
        }
    }
}

