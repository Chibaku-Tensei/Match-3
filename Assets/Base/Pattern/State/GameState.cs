using System;
using System.Collections;
using System.Collections.Generic;
using Base.Module;
using UnityEngine;

namespace Base.Pattern
{
    public abstract class GameState : BaseMono, IUpdateable
    {
        protected GameStateController GameStateController { get; private set; }

        protected virtual void Awake()
        {
            GameStateController = Parent.GetComponent<GameStateController>();
        }

        /// <summary>
        /// This method runs once when the state has entered the state machine
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fromState"></param>
        public virtual void EnterStateBehaviour(float dt, GameState fromState)
        {
            Debug.Log("Enter " + name + " state");
        }

        /// <summary>
        /// This method runs once when the state has exited the state machine
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="toState"></param>
        public virtual void ExitStateBehaviour(float dt, GameState toState)
        {
            Debug.Log("Exit " + name + " state");
        }

        
        /// <summary>
        /// This method runs before the main Update method
        /// </summary>
        /// <param name="dt"></param>
        public virtual void PreUpdateBehaviour(float dt) { }

        /// <summary>
        /// This method runs every frame, and should be implement by derived class
        /// </summary>
        /// <param name="dt"> the delta time in every frame </param>
        public abstract void UpdateBehaviour(float dt);

        /// <summary>
        /// This method runs after the main Update method
        /// </summary>
        /// <param name="dt"></param>
        public virtual void PostUpdateBehaviour(float dt) { }
        
        /// <summary>
        /// Checks if the required conditions to exit this state are true. If so it returns the desired state (null otherwise). After this the state machine will
        /// proceed to evaluate the "enter transition" condition on the target state.
        /// </summary>
        public virtual void CheckExitTransition()
        {
            
        }

        /// <summary>
        /// Checks if the required conditions to enter this state are true. If so the state machine will automatically change the current state to the desired one.
        /// </summary>
        /// <param name="fromState"></param>
        public virtual bool CheckEnterTransition( GameState fromState )
        {
            return true;
        }
    }
}

