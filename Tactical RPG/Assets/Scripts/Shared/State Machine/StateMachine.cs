using System.Collections;
using UnityEngine;

public class StateMachine : MonoBehaviour {

    /// <summary>
    /// Current Game State, protected through this property
    /// </summary>
    public virtual State CurrentState {
        get { return _currentState; }
        set { Transition(value); }
    }

    protected State _currentState;
    protected bool _inTransition;

    /// <summary>
    /// Get target game state
    /// </summary>
    /// <typeparam name="T">target State</typeparam>
    /// <returns>target</returns>
    public virtual T GetState<T>() where T : State {
        T target = GetComponent<T>();
        if (target == null)
            target = gameObject.AddComponent<T>();
        return target;
    }

    /// <summary>
    /// Changes game states. Sets the current state to the target state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public virtual void ChangeState<T>() where T : State {
        CurrentState = GetState<T>();
    }

    /// <summary>
    /// Handles transitioning between states and checking for state validity
    /// </summary>
    /// <param name="value"></param>
    protected virtual void Transition (State value) {
        // State is the same, exit early
        if (_currentState == value || _inTransition)
            return;

        _inTransition = true;   // Mark beginning of transition

        // previous state is not null, exit
        if (_currentState != null)
            _currentState.Exit();

        _currentState = value;

        // New state is not null, enter the state
        if (_currentState != null)
            _currentState.Enter();

        _inTransition = false;  // Mark end of transition
    }
	
}
