using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Base.GameEventSystem;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Base.Module
{
    public static class UtilsClass
    {
        public static string FormatMoney(int money, int decPlace = 2)
        {
            string result = String.Empty;
            float place = Mathf.Pow(10f, decPlace);

            string[] abbrev = {"K", "M", "B", "T"};
            string str = (money < 0) ? "-" : "";
            float size;

            money = Mathf.Abs(money);

            for (int i = abbrev.Length - 1; i >= 0; --i)
            {
                size = Mathf.Pow(10, (i + 1) * 3);
                if (size <= money)
                {
                    money = (int) (Mathf.Floor(money * place / size) / place);
                    if ((money == 1000) && (i < abbrev.Length - 1))
                    {
                        money = 1;
                        i += 1;
                    }

                    result = money + abbrev[i];
                    break;
                }
            }

            return str + result;
        }

        /// <summary>
        /// Convert money string with format x.xxx.xxx or x,xxx,xxx to Float
        /// </summary>
        /// <param name="moneyStr"></param>
        /// <returns>money in float</returns>
        public static uint ConvertMoneyToNumber(string moneyStr)
        {
            var resultStr = moneyStr.Split('.');
            if (resultStr.Length <= 1)
            {
                resultStr = moneyStr.Split(',');
            }

            string joinString = string.Join("", resultStr);
            return uint.Parse(joinString);
        }

        /// <summary>
        /// Format a number to string with comma seperated format.
        /// </summary>
        /// <param name="money"></param>
        /// <returns>money in string</returns>
        public static string FormatStringCommaSeparated(uint money)
        {
            object o = money;
            return $"{o:#,##0.##}";
        }

        public static string ToOrdinalString(this long number)
        {
            if (number < 0) return number.ToString();
            long rem = number % 100;
            if (rem >= 11 && rem <= 13) return number + "th";

            switch (number % 10)
            {
                case 1:
                    return number + "st";
                case 2:
                    return number + "nd";
                case 3:
                    return number + "rd";
                default:
                    return number + "th";
            }
        }

        public static string ToOrdinalString(this int number)
        {
            return ((long) number).ToOrdinalString();
        }

        public static Vector3 CalcBallisticVelocityVector(Vector3 source, Vector3 target, float angle)
        {
            Vector3 direction = target - source;
            float h = direction.y;
            direction.y = 0;
            float distance = direction.magnitude;
            float a = angle * Mathf.Deg2Rad;
            direction.y = distance * Mathf.Tan(a);
            distance += h / Mathf.Tan(a);

            // calculate velocity
            float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
            return velocity * direction.normalized;
        }

        /// <summary>
        /// Returns true if the target value is between a and b ( both exclusive ). 
        /// To include the limits values set the "inclusive" parameter to true.
        /// </summary>
        public static bool IsBetween(float target, float a, float b, bool inclusive = false)
        {
            if (b > a)
                return (inclusive ? target >= a : target > a) && (inclusive ? target <= b : target < b);
            else
                return (inclusive ? target >= b : target > b) && (inclusive ? target <= a : target < a);
        }

        /// <summary>
        /// Returns true if the target value is between a and b ( both exclusive ). 
        /// To include the limits values set the "inclusive" parameter to true.
        /// </summary>
        public static bool IsBetween(int target, int a, int b, bool inclusive = false)
        {
            if (b > a)
                return (inclusive ? target >= a : target > a) && (inclusive ? target <= b : target < b);
            else
                return (inclusive ? target >= b : target > b) && (inclusive ? target <= a : target < a);
        }

        public static bool IsCloseTo(Vector3 input, Vector3 target, float tolerance)
        {
            return Vector3.Distance(input, target) <= tolerance;
        }

        public static bool IsCloseTo(float input, float target, float tolerance)
        {
            return Mathf.Abs(target - input) <= tolerance;
        }

        /// <summary>
        /// Gets a "target" component within a particular branch (inside the hierarchy). The branch is defined by the "branch root object", which is also defined by the chosen 
        /// "branch root component". The returned component must come from a child of the "branch root object".
        /// </summary>
        /// <param name="callerComponent"></param>
        /// <param name="includeInactive">Include inactive objects?</param>
        /// <typeparam name="T1">Branch root component type.</typeparam>
        /// <typeparam name="T2">Target component type.</typeparam>
        /// <returns>The target component.</returns>
        public static T2 GetComponentInBranch<T1, T2>(this Component callerComponent, bool includeInactive = true) where T1 : Component where T2 : Component
        {
            T1[] rootComponents = callerComponent.transform.root.GetComponentsInChildren<T1>(includeInactive);

            if (rootComponents.Length == 0)
            {
                Debug.LogWarning($"Root component: No objects found with {typeof(T1).Name} component");
                return null;
            }

            for (int i = 0; i < rootComponents.Length; i++)
            {
                T1 rootComponent = rootComponents[i];

                // Is the caller a child of this root?
                if (!callerComponent.transform.IsChildOf(rootComponent.transform) && !rootComponent.transform.IsChildOf(callerComponent.transform)) continue;

                T2 targetComponent = rootComponent.GetComponentInChildren<T2>(includeInactive);

                if (targetComponent == null) continue;

                return targetComponent;
            }

            return null;
        }
        
        public static T2 GetOrRegisterValue< T1, T2 >( this Dictionary< T1, T2 > dictionary , T1 key ) where T1 : Component where T2 : Component
        {
            if( key == null )
                return null;

            bool found = dictionary.TryGetValue( key, out var value );
		
            if( !found )
            {
                value = key.GetComponent<T2>();
			
                if( value!= null )
                    dictionary.Add( key , value );
            }

            return value;
        }

        /// <summary>
        /// Gets a "target" component within a particular branch (inside the hierarchy). The branch is defined by the "branch root object", which is also defined by the chosen 
        /// "branch root component". The returned component must come from a child of the "branch root object".
        /// </summary>
        /// <param name="callerComponent"></param>
        /// <param name="includeInactive">Include inactive objects?</param>
        /// <typeparam name="T1">Target component type.</typeparam>	
        /// <returns>The target component.</returns>
        public static T1 GetComponentInBranch<T1>(this Component callerComponent, bool includeInactive = true) where T1 : Component
        {
            return callerComponent.GetComponentInBranch<T1, T1>(includeInactive);
        }

        #region Animator

        /// <summary>
        /// Gets the current clip effective length, that is, the original length divided by the playback speed. The length value is always positive, regardless of the speed sign. 
        /// It returns false if the clip is not valid.
        /// </summary>
        public static bool GetCurrentClipLength(this Animator animator, ref float length)
        {
            if (animator.runtimeAnimatorController == null) return false;

            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

            if (clipInfo.Length == 0) return false;

            float clipLength = clipInfo[0].clip.length;
            float speed = animator.GetCurrentAnimatorStateInfo(0).speed;

            length = Mathf.Abs(clipLength / speed);

            return true;
        }
        
        public static bool GetCurrentClipLength(this Animator animator, int layerIndex, ref float length)
        {
            if (animator.runtimeAnimatorController == null) return false;

            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(layerIndex);

            if (clipInfo.Length == 0) return false;

            float clipLength = clipInfo[0].clip.length;
            float speed = animator.GetCurrentAnimatorStateInfo(0).speed;

            length = Mathf.Abs(clipLength / speed);

            return true;
        }

        public static bool MatchTarget(this Animator animator, Vector3 targetPosition, Quaternion targetRotation, AvatarTarget avatarTarget, float startNormalizedTime,
            float targetNormalizedTime)
        {
            if (animator.runtimeAnimatorController == null) return false;

            if (animator.isMatchingTarget) return false;

            if (animator.IsInTransition(0)) return false;

            MatchTargetWeightMask weightMask = new MatchTargetWeightMask(Vector3.one, 1f);

            animator.MatchTarget(targetPosition, targetRotation, avatarTarget, weightMask, startNormalizedTime, targetNormalizedTime);

            return true;
        }

        public static bool MatchTarget(this Animator animator, Vector3 targetPosition, AvatarTarget avatarTarget, float startNormalizedTime, float targetNormalizedTime)
        {
            if (animator.runtimeAnimatorController == null) return false;

            if (animator.isMatchingTarget) return false;

            if (animator.IsInTransition(0)) return false;

            MatchTargetWeightMask weightMask = new MatchTargetWeightMask(Vector3.one, 0f);

            animator.MatchTarget(targetPosition, Quaternion.identity, avatarTarget, weightMask, startNormalizedTime, targetNormalizedTime);

            return true;
        }

        public static bool MatchTarget(this Animator animator, Transform target, AvatarTarget avatarTarget, float startNormalizedTime, float targetNormalizedTime)
        {
            if (animator.runtimeAnimatorController == null) return false;

            if (animator.isMatchingTarget) return false;

            if (animator.IsInTransition(0)) return false;

            MatchTargetWeightMask weightMask = new MatchTargetWeightMask(Vector3.one, 1f);

            animator.MatchTarget(target.position, target.rotation, avatarTarget, weightMask, startNormalizedTime, targetNormalizedTime);

            return true;
        }

        public static bool MatchTarget(this Animator animator, Transform target, AvatarTarget avatarTarget, float startNormalizedTime, float targetNormalizedTime,
            MatchTargetWeightMask weightMask)
        {
            if (animator.runtimeAnimatorController == null) return false;

            if (animator.isMatchingTarget) return false;

            if (animator.IsInTransition(0)) return false;

            animator.MatchTarget(target.position, target.rotation, AvatarTarget.Root, weightMask, startNormalizedTime, targetNormalizedTime);

            return true;
        }

        #endregion
    }
}
