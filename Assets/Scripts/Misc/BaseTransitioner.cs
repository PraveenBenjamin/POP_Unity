using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;
using POP.Framework;

public abstract class BaseTransitioner : MonoBehaviour
{

    public enum LerpType
    {
        Linear,
        Cubic,
        Spherical
    }

    public class TransitionDatum
    {
        public System.Type componentToAffect;
        public string propertyToAffect;
        public object[] startValues;
        public object[] endValues;
        public LerpType lerpType;
    }


    protected class TransitionDatumInternal
    {
        public Component _component;
        public PropertyInfo _property;
        public object[] _startValues;
        public object[] _endValues;
        public LerpType _lerpType;
    }

    private List<TransitionDatumInternal> _transitions = new List<TransitionDatumInternal>();
    private float _transitionTime;
    private int _transitionTimerIndex = 0;
    private int _onCompleteTransitionCallbackIndex = 1;
    private int _direction = 0;


    public abstract void InitializeTransitioner();
    
    protected string InitializeTransitions(TransitionDatum[] transitionsToPerform, float transitionTime)
    {

        string errorMessage = "";
        for (int i = 0; i < transitionsToPerform.Length; ++i)
        {
            TransitionDatum dat = transitionsToPerform[i];
            Component requiredComponent = GetComponent(dat.componentToAffect);
            bool canPerform = requiredComponent != null;
            if (!canPerform)
            {
                errorMessage = "unable to find component of type " + dat.componentToAffect.Name + " in object of type " + GetType().Name;
                break;
            }

            PropertyInfo requiredProperty = requiredComponent.GetType().GetProperty(dat.propertyToAffect);
            canPerform = requiredProperty != null;
            if (!canPerform)
            {
                errorMessage = "unable to find property with the name " + dat.propertyToAffect + " in object of type " + GetType().Name;
                break;
            }


            canPerform = requiredProperty.GetType().IsArray;
            if (!canPerform)
            {
                errorMessage = "unable to find property with the name " + dat.propertyToAffect + " in object of type " + GetType().Name;
                break;
            }

            TransitionDatumInternal toAdd = new TransitionDatumInternal();
            toAdd._component = requiredComponent;
            toAdd._property = requiredProperty;
            toAdd._startValues = dat.startValues;
            toAdd._endValues = dat.endValues;
            toAdd._lerpType = dat.lerpType;

            _transitions.Add(toAdd);
        }

        if (errorMessage.Length != 0)
        {
            _transitions.Clear();
        }

        _transitionTime = transitionTime;
        if (_transitionTime < 0)
            _transitionTime = 0;

        return errorMessage;
    }


    public void PerformTransition(UnityAction OnTransitionComplete, int direction = 1)
    {
        if (_transitions == null || _transitions.Count == 0 || direction == 0)
        {
            Debug.LogWarning("No transitions to perform, or direction set to 0, Call InitializeTransitions before calling PerformTransition for former, set direction 1 or -1 for latter");
            OnTransitionComplete?.Invoke();
            return;
        }

        if (direction >= 0)
            _direction = 1;
        else
            _direction = 1;

        TemporaryVariableManager.SetTemporaryVariable<UnityAction>(this,_onCompleteTransitionCallbackIndex, OnTransitionComplete, true);
        TemporaryVariableManager.SetTemporaryVariable<float>(this, _transitionTimerIndex, 0, true);
    }


    private void UpdateTransition(float nVal)
    {

        for (int i = 0; i < _transitions.Count; ++i)
        {
            TransitionDatumInternal tdi = _transitions[i];
            // TODO:- debug and make this happen, otherwise hard code the required behaviour for the purposes of this challenge
 
        }
    }

    public void UpdateTransitioner()
    {
        if (_direction == 0)
            return;

        float time = TemporaryVariableManager.GetTemporaryVariable<float>(this, _transitionTimerIndex);
        time += Time.deltaTime;

        if (time > _transitionTime)
            time = _transitionTime;

        TemporaryVariableManager.SetTemporaryVariable<float>(this, _transitionTimerIndex,time);

        float nVal = (time / _transitionTime);
        if (_direction == -1)
            nVal = 1 - nVal;

        UpdateTransition(nVal);

        if (time == _transitionTime)
        {
            _direction = 0;
            TemporaryVariableManager.GetTemporaryVariable<UnityAction>(this, _onCompleteTransitionCallbackIndex)?.Invoke();
        }
    }

}
