using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraTransitioner : SingletonBehaviour<CameraTransitioner>
{
    //alright, so im not gonna bother making this one too complicated,
    //0 = Start Position 
    //1 = MainMenu
    //2 = LevelSelectEasy
    //3 = LevelSelectMed
    //4 = LevelSelectHard
    //5 = GameOver
    [SerializeField]
    private List<Vector3> _possibleCameraPositions;


    //hacky yes, but i really dont have the time :(
    [SerializeField]
    private List<Vector3> _possibleCameraOrientations;

    public enum CameraPositions
    {
        StartPosition = 0,
        MainMenu,
        LevelSelectEasy,
        LevelSelectMedium,
        LevelSelectHard,
        GameOver
    }


    private BaseTransitioner.LerpType _lerpType;

    private Vector3 _transitionStartPos;
    private Quaternion _transitionStartRot;
    private Vector3 _targetPosition;
    private Quaternion _targetRotation;

    private float _transitionTime;
    private float _timer = 0;
    private UnityAction _onComplete = null;


    private void Start()
    {
        transform.position = _possibleCameraPositions[0];
        transform.rotation = Quaternion.Euler(_possibleCameraOrientations[0]);
    }

    public void TransitionTo(CameraPositions position, UnityAction onComplete = null, BaseTransitioner.LerpType lerpType = BaseTransitioner.LerpType.Cubic,float transitionTime = 2)
    {
        TransitionTo(_possibleCameraPositions[(int)position], _possibleCameraOrientations[(int)position], onComplete, lerpType,transitionTime);
    }


    public void TransitionTo(Vector3 position,Vector3 rotation, UnityAction onComplete = null, BaseTransitioner.LerpType lerpType = BaseTransitioner.LerpType.Cubic, float transitionTime = 2)
    {
        _timer = 0;
        _transitionStartPos = transform.position;
        _transitionStartRot = transform.rotation;
        _targetPosition = position;
        _targetRotation = Quaternion.Euler(rotation);
        _lerpType = lerpType;
        _transitionTime = transitionTime;
        _onComplete = onComplete;
    }


    public void Update()
    {
        if (_timer >= _transitionTime)
            return;

        _timer += Time.deltaTime;
        if (_timer > _transitionTime)
            _timer = _transitionTime;


        Quaternion rot = transform.rotation;
        Vector3 pos = transform.position;
        float val = 0;

        switch (_lerpType)
        {
            case BaseTransitioner.LerpType.Cubic:
                val = Mathfx.Hermite(0, 1, _timer / _transitionTime);
            break;
            case BaseTransitioner.LerpType.Sin:
                val = Mathfx.Sinerp(0, 1, _timer / _transitionTime);
            break;
            case BaseTransitioner.LerpType.Linear:
                val = Mathfx.Lerp(0, 1, _timer / _transitionTime);
            break;
        }

        rot = Quaternion.Lerp(_transitionStartRot, _targetRotation, val);
        pos = Vector3.Lerp(_transitionStartPos, _targetPosition, val);
        transform.position = pos;
        transform.rotation = rot;

        if (_timer == _transitionTime)
        {
            _onComplete?.Invoke();
        }
    }


    protected override void InitializeSingleton()
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnDestroySingleton()
    {
        //throw new System.NotImplementedException();
    }

    
}
