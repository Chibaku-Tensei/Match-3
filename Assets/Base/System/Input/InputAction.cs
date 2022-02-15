using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Base
{
    public interface InputAction
    {
        InputPhase Phase { get; }

        Vector3 Position { get; }

        Vector3 DeltaPosition { get; }
    }
    
    public enum InputPhase {None, Began, Moved, Ended, Stationary}
}