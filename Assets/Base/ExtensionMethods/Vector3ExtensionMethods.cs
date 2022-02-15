using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    public static class Vector3ExtensionMethods
    {
        public static bool IsNaN(this Vector3 target)
        {
            return (float.IsNaN(target.x) || float.IsNaN(target.y) || float.IsNaN(target.z));
        }
    }
}

