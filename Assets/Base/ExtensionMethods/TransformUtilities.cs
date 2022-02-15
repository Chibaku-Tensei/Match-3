using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace Base
{
    public static class TransformUtilities
    {
        #region Scale

        public static void SetScale(this Transform target, float endValue)
        {
            target.localScale = new Vector3(endValue, endValue, endValue);
        }

        public static void SetScale(this Transform target, float x, float y, float z)
        {
            target.localScale = new Vector3(x, y, z);
        }

        #endregion
        
        #region Position

        public static void SetPosX(this Transform target, float xPos)
        {
            Vector3 position = target.position;
            target.position = new Vector3(xPos, position.y, position.z);
        }

        public static void SetPosY(this Transform target, float yPos)
        {
            Vector3 position = target.position;
            target.position = new Vector3(position.x, yPos, position.z);
        }

        public static void SetPosZ(this Transform target, float zPos)
        {
            Vector3 position = target.position;
            target.position = new Vector3(position.x, position.y, zPos);
        }

        public static void SetPosition(this Transform target, Vector3 position)
        {
            target.position = position;
        }

        public static void SetPosition(this Transform target, float x, float y, float z)
        { 
            target.SetPosition(new Vector3(x, y, z));
        }

        public static void SetLocalPosition(this Transform target, Vector3 newLocalPos)
        {
            target.localPosition = newLocalPos;
        }
        
        public static void SetLocalPosition(this Transform target, float x, float y, float z)
        {
            target.SetLocalPosition(new Vector3(x, y, z));
        }

        #endregion
        
        #region Child Interactions

        public static List<Transform> GetAllChildren(this Transform target)
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in target)
            {
                children.Add(child);
            }

            return children;
        }

        public static void DestroyAllChildren(this Transform target, bool isStopDOTween = false)
        {
            foreach (Transform child in target)
            {
                if (isStopDOTween)
                {
                    DOTween.Kill(child);
                }
                Object.Destroy(child.gameObject);
            }
        }
        
        /// <summary>
        /// Get the index of child in parent transform
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static int IndexOf(this Transform parent, Transform child)
        {
            if (child.IsChildOf(parent))
            {
                int childCount = parent.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    if (child.Equals(parent.GetChild(i)))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public static void RemoveChildren(this Transform target, Transform child, Transform newParent = null)
        {
            if (newParent)
            {
                child.SetParent(newParent);
            }
            else
            {
                child.SetParent(target.parent);
            }
        }

        public static void RemoveAllChildren(this Transform target, Transform newParent = null)
        {
            foreach (Transform child in target)
            {
                target.RemoveChildren(child, newParent);
            }
        }

        #endregion

    }
}
