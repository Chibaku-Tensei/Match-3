using System;
using System.Collections;
using System.Collections.Generic;
using Base.Pattern;
using UnityEngine;

namespace Base
{
    public class InputHandler : MonoBehaviour
    {
        private InputAction inputAction;

        public InputAction InputAction => inputAction;

        public void CreateInputAction()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                inputAction = gameObject.AddComponent<TouchInputAction>();
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                inputAction = gameObject.AddComponent<MouseInputAction>();
            }
        }
    }
}

