using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Pattern
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private GameState currentState = null;

        private GameState _previousState = null;
        
        private Dictionary<string, GameState> _states = new Dictionary<string, GameState>();
        
        private Queue<GameState> transitionQueue = new Queue<GameState>();

        private InputHandler inputHandler;
        
        /// <summary>
        /// Gets the current state used by the state machine
        /// </summary>
        public GameState CurrentState => currentState;
        
        /// <summary>
        /// Gets the previous state used by the state machine
        /// </summary>
        public GameState PreviousState => _previousState;

        public InputAction InputAction => inputHandler.InputAction;
        
        /// <summary>
        /// Event fire when a state transition occurs.
        /// </summary>
        public event System.Action<GameState, GameState> OnStateChanged;


        public GameState GetState(string stateName)
        {
            GameState result = null;
            bool isValid = _states.TryGetValue(stateName, out result);

            return result;
        }
        
        public GameState GetState<T>() where T : GameState
        {
            foreach (var state in _states.Values)
            {
                if (state.GetType() == typeof(T))
                {
                    return state;
                }
            }

            return null;
        }
        
        /// <summary>
        /// Adds a particular state to the transition state queue (as a potential transition). The state machine will eventually check if the transition is accepted or rejected 
        /// by the target state (CheckEnterTransition). Call this method from within the CheckExitTransition method. 
        /// </summary>
        /// <example>
        /// For instance, if you need to transition to multiple states.
        /// <code>
        /// if( conditionA )
        /// {	
        /// 	EnqueueTransition<TargetStateA>();
        /// }
        /// else if( conditionB )
        /// {
        /// 	EnqueueTransition<TargetStateB>();
        /// 	EnqueueTransition<TargetStateC>(); 	
        /// }
        /// </code>	
        /// </example>
        public void EnqueueTransition<T>() where T : GameState
        {
            GameState state = GetState<T>();

            if( state != null )
                transitionQueue.Enqueue( state );
        }
        
        // --------------------------------------- Unity Event function ---------------------------------------------- //

        private void Awake()
        {
            AddAndInitializeState();

            inputHandler = gameObject.AddComponent<InputHandler>();
            
        }
        
        private void Start()
        {
            inputHandler.CreateInputAction();
            
            if (CurrentState != null)
            {
                CurrentState.EnterStateBehaviour(0, CurrentState);
            }
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            if (CurrentState == null) return;

            bool changeOfState = CheckForTransition();
            
            transitionQueue.Clear();

            if (changeOfState)
            {
                PreviousState.ExitStateBehaviour(dt, CurrentState);
                
                CurrentState.EnterStateBehaviour(dt, PreviousState);
            }
            
            CurrentState.PreUpdateBehaviour(dt);
            CurrentState.UpdateBehaviour(dt);
            CurrentState.PostUpdateBehaviour(dt);
        }
        
        
        // -------------------------------------- End Unity Event function ------------------------------------------ //

        private bool CheckForTransition()
        {
            CurrentState.CheckExitTransition();

            GameState nextState = null;

            while (transitionQueue.Count > 0)
            {
                GameState thisState = transitionQueue.Dequeue();

                if (thisState == null) continue;

                if (!thisState.enabled) continue;

                bool success = thisState.CheckEnterTransition(CurrentState);

                if (success)
                {
                    nextState = thisState;
                    
                    OnStateChanged?.Invoke(CurrentState, nextState);

                    _previousState = CurrentState;
                    currentState = nextState;

                    return true;
                }
            }
            return false;
        }
        
        private void AddAndInitializeState()
        {
            GameState[] stateArray = this.GetComponentsInChildren<GameState>();
            for (int i = 0; i < stateArray.Length; ++i)
            {
                GameState state = stateArray[i];
                string stateName = state.GetType().Name;
                if (GetState(stateName) != null)
                {
                    Debug.Log(String.Format("Warning: Game object {0} of {1} already has the state {2}",
                        new object[] {state.gameObject.name, state.transform.parent.gameObject.name, stateName}));
                    continue;
                }
                
                _states.Add(stateName, state);
            }
        }
    }
}

