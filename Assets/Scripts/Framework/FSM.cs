using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;
using POP;

namespace POP.Framework
{

    public class FSM<T> where T : struct, IConvertible
    {
        

        public class State
        {

            public enum StateComponent
            {
                Init,
                Update,
                Terminate
            }


            private T _baseEnumValue;
            private TypeInfo _typeofState;
            private UnityAction _initCallback;
            private UnityAction _updateCallback;
            private UnityAction _terminateCallback;

            public State()
            {
                _initCallback = null;
                _updateCallback = null;
                _terminateCallback = null;
            }

            //todo :- see if we need to return a variable that tells us the available callbacks
            public void Initialize(T enumValue, UnityEngine.Object toInitializeWith, string initPrefix, string updatePrefix, string terminatePrefix)
            {
                _baseEnumValue = enumValue;
                _typeofState = toInitializeWith.GetType().GetTypeInfo();

                MethodInfo[] allMethods = toInitializeWith.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);


                //use reflection to auto assign required callbacks
                MethodInfo curr;
                string methodNameToConsider;
                for (int i = 0; i < allMethods.Length; ++i)
                {
                    curr = allMethods[i];
                    methodNameToConsider = _baseEnumValue.ToString();
                    methodNameToConsider.Insert(0, initPrefix);
                    if (curr.Name.Equals(methodNameToConsider))
                    {
                        _initCallback = (UnityAction)curr.CreateDelegate(typeof(T), toInitializeWith);
                    }

                    methodNameToConsider.Remove(0, initPrefix.Length);
                    methodNameToConsider.Insert(0, updatePrefix);
                    if (curr.Name.Equals(methodNameToConsider))
                    {
                        _updateCallback = (UnityAction)curr.CreateDelegate(typeof(T), toInitializeWith);
                    }

                    methodNameToConsider.Remove(0, updatePrefix.Length);
                    methodNameToConsider.Insert(0, terminatePrefix);
                    if (curr.Name.Equals(methodNameToConsider))
                    {
                        _terminateCallback = (UnityAction)curr.CreateDelegate(typeof(T), toInitializeWith);
                    }
                }

                if (_initCallback == null && _updateCallback == null && _terminateCallback == null)
                {
                    Debug.LogWarning("empty state detected, was this intended?");
                }
            }

            public void Invoke(StateComponent comp)
            {
                switch (comp)
                {
                    case StateComponent.Init:
                        _initCallback?.Invoke();
                        break;
                    case StateComponent.Update:
                        _updateCallback?.Invoke();
                        break;
                    case StateComponent.Terminate:
                        _terminateCallback?.Invoke();
                        break;
                }
            }

            ~State()
            {
                _initCallback = null;
                _terminateCallback = null;
                _updateCallback = null;
            }
        }


        private State _currentState;
        private Nullable<T> _currentStateIndex;
        private Nullable<T>[] _nextStatesIndices = new Nullable<T>[2];
        private Nullable<T> _prevStateIndex;

        private T[] _enumValsCache;
        private State[] _states;


        public bool Initialize(UnityEngine.Object instanceToInitializeFor)
        {

            if (instanceToInitializeFor = null)
            {
                Debug.LogError("instance is Null, why you do dis?");
                return false;
            }

            //refresh
            _currentState = null;
            _currentStateIndex = null;
            System.Array.Clear(_nextStatesIndices, 0, _nextStatesIndices.Length);
            _nextStatesIndices = new Nullable<T>[2];
            _prevStateIndex = null;

            try
            {
                T[] enumValsCache = (T[])System.Enum.GetValues(typeof(T));

                _states = new State[enumValsCache.Length];
                for (int i = 0; i < enumValsCache.Length; ++i)
                {
                    State toCreate = new State();
                    toCreate.Initialize((T)enumValsCache.GetValue(i), instanceToInitializeFor, Constants.FSMInitPrefix, Constants.FSMUpdatePrefix, Constants.FSMTerminatePrefix);
                    _states[i] = toCreate;
                }


                _currentStateIndex = (T)_enumValsCache.GetValue(0);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Initialization of FSM failed for object: " + instanceToInitializeFor.GetType() + instanceToInitializeFor.name + " with error: " + e.Message);
                return false;
            }
        }

        public void SetState(T toSet)
        {
            if (toSet.Equals(_currentStateIndex))
                return;

            if (_nextStatesIndices[0].Equals(toSet))
                return;

            _nextStatesIndices[1] = toSet;
           
        }

        public T GetState()
        {
            return (T)_currentStateIndex;
        }


        private bool CheckAndChangeState()
        {

            //first launch
            if (_currentState == null)
            {
                _currentState = _states[System.Convert.ToInt32(_currentStateIndex.Value)];
                _currentState.Invoke(State.StateComponent.Init);
                return true;
            }

            //if the user wants to switch states
            if (_nextStatesIndices[0] != null)
            {
                //terminate current
                _currentState.Invoke(State.StateComponent.Terminate);

                //switch index to new state
                _currentStateIndex = _nextStatesIndices[0];
                _nextStatesIndices[0] = _nextStatesIndices[1];
                _nextStatesIndices[1] = null;

                //since T is always expected to be an enum, we can assume this wont fail (the check happens in the Initialize method)
                //assign state associated with index of current state
                _currentState = _states[System.Convert.ToInt32(_currentStateIndex.Value)];

                //initialize the next (now set to current) state
                _currentState.Invoke(State.StateComponent.Init);

                return true;
            }
            return false;
        }

        public void UpdateStateMachine()
        {
            if (_currentStateIndex == null)
                return;

            //if a state change didnt happen this cycle
            if (!CheckAndChangeState())
            {

                //update the state
                _currentState.Invoke(State.StateComponent.Update);
            }
        }

        private void ReleaseResources()
        {
            //invoke destructor within the states
            System.Array.Clear(_states, 0, _states.Length);

            //clear erething!
            _states = null;
            _enumValsCache = null;
        }

        ~FSM()
        {
            ReleaseResources();
        }
    }
}