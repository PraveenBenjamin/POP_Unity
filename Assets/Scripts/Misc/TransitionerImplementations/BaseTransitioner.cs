using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;
using POP.Framework;

//can extend this to support more than just Vector3, but dont have the time
public class BaseTransitioner : MonoBehaviour
{

    public enum LerpType
    {
        Linear,
        Cubic,
        Sin
    }

    [System.Serializable]
    public class TransitionDatum
    {
        public string hierarchyOfTransform;
        public string typeOfComponentToAffect;
        public string nameOfPropertyToAffect;
        public string typeOfPropertyToAffect;
        public string startValue;
        public string endValue;
        public LerpType lerpType;
    }

    [System.Serializable]
    public class TransitionData
    {
        public BaseTransitioner.TransitionDatum[] Data;
    }

    [SerializeField]
    public TransitionData _transitionData;


    protected class TransitionDatumInternal<T>
    {
        public Component _component;
        public PropertyInfo _property;
        public T _startValue;
        public T _endValue;
        public LerpType _lerpType;
    }

    private List<TransitionDatumInternal<Vector3>> _transitions = new List<TransitionDatumInternal<Vector3>>();
    private float _transitionTime;
    private int _transitionTimerIndex = 0;
    private int _onCompleteTransitionCallbackIndex = 1;
    private int _direction = 0;



    private void LogAllPropertiesAndTheirTypes(Component toLog)
    {
        PropertyInfo[] allProperties = toLog.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        string toPrint = "";
        for (int i = 0; i < allProperties.Length; ++i)
        {
            toPrint += allProperties[i].Name + " " + allProperties[i].PropertyType +"\n";
        }
        //Debug.Log(toPrint);
    }

    public string InitializeTransitions(float transitionTime)
    {
        return InitializeTransitions(_transitionData.Data, transitionTime);
    }

    private string InitializeTransitions(TransitionDatum[] transitionsToPerform, float transitionTime)
    {

        string errorMessage = "";
        for (int i = 0; i < transitionsToPerform.Length; ++i)
        {

            


            TransitionDatum dat = transitionsToPerform[i];
            Transform requiredTransform = transform.Find(dat.hierarchyOfTransform);
            bool canPerform = requiredTransform != null;
            if (!canPerform)
            {
                errorMessage = "unable to find child at hierarchy" + dat.hierarchyOfTransform + " in object of type " + GetType().Name;
                break;
            }

            Component requiredComponent = requiredTransform.GetComponent(dat.typeOfComponentToAffect);

            canPerform = requiredComponent != null;
            if (!canPerform)
            {
                errorMessage = "unable to find component of type " + dat.typeOfComponentToAffect + " in object of type " + GetType().Name;
                break;
            }

            LogAllPropertiesAndTheirTypes(requiredComponent);

            PropertyInfo requiredProperty = requiredComponent.GetType().GetProperty(dat.nameOfPropertyToAffect);
            canPerform = requiredProperty != null;
            if (!canPerform)
            {
                errorMessage = "unable to find property with the name " + dat.nameOfPropertyToAffect + " in object of type " + GetType().Name;
                break;
            }

            TransitionDatumInternal<Vector3> toAdd = new TransitionDatumInternal<Vector3>();
            toAdd._component = requiredComponent;
            toAdd._property = requiredProperty;
            toAdd._startValue = Utils.Vector3FromString(dat.startValue);
            toAdd._endValue = Utils.Vector3FromString(dat.endValue);
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
            _direction = -1;

        TemporaryVariableManager.SetTemporaryVariable<UnityAction>(this,_onCompleteTransitionCallbackIndex, OnTransitionComplete, true);
        TemporaryVariableManager.SetTemporaryVariable<float>(this, _transitionTimerIndex, 0, true);
    }


    private void UpdateTransition(float nVal)
    {

        for (int i = 0; i < _transitions.Count; ++i)
        {

            TransitionDatumInternal<Vector3> tdi = _transitions[i];
            // TODO:- debug and make this happen, otherwise hard code the required behaviour for the purposes of this challenge

            switch (tdi._lerpType)
            {
                default:
                case LerpType.Cubic:
                    nVal = Mathfx.Hermite(0, 1, nVal);
                break;
            }

            Vector3 temp = Vector3.Lerp(tdi._startValue, tdi._endValue, nVal);
            tdi._property.SetValue(tdi._component, temp);
            //Debug.Log(tdi._property.GetValue(tdi._component).ToString()+" "+nVal+" "+temp.ToString());
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
