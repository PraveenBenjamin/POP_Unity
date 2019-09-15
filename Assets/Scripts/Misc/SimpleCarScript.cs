using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VROOM!
/// lerps between the children of the circuit parent at the speed specified. 
/// Hacked in some animation to make it look pretty :D
/// </summary>
public class SimpleCarScript : MonoBehaviour
{
    [SerializeField]
    private Transform _circuitParent;

    [SerializeField]
    private float _carSpeed = 1;

    private int _currentIndex = 0;

    private float _lerpVal = 0;

    [SerializeField]
    private float _animPulseTime = 0.5f;
    private float _animPulseTimer = 0;
    private int _animPulseDirection = 1;

    //kinda unoptimized yeah... but hey its a throwaway visual script
    private Vector3 lerpStart
    {
        get
        {
            return _circuitParent.GetChild(_currentIndex).position;
        }
    }

    private Vector3 lerpEnd
    {
        get
        {
            int indexToConsider = _currentIndex + 1 >= _circuitParent.childCount ? 0 : _currentIndex + 1;
            return _circuitParent.GetChild(indexToConsider).position;
        }
    }


    public void Update()
    {

        Vector3 lStart = lerpStart;
        Vector3 lEnd = lerpEnd;
        Vector3 forward = lerpEnd - lerpStart;
        float lDist = forward.magnitude;
        float incrementPerUpdate = (_carSpeed / lDist) * Time.deltaTime;
        _lerpVal += incrementPerUpdate;

        transform.position = Vector3.Lerp(lerpStart, lerpEnd, _lerpVal);
        transform.forward = forward;

        _animPulseTimer += Time.deltaTime * (1 / _animPulseTime) * _animPulseDirection;
        if (_animPulseTimer > 1)
        {
            _animPulseTimer = 1;
            _animPulseDirection *= -1;
        }
        else if (_animPulseTimer < 0)
        {
            _animPulseTimer = 0;
            _animPulseDirection *= -1;
        }

        transform.localScale = Vector3.Lerp(Vector3.one * 0.9f,Vector3.one* 1.1f, Mathfx.Sinerp(0,1, _animPulseTimer));
        //Debug.Log(_animPulseTimer);

        if (_lerpVal >= 1)
        {
            _lerpVal = 0;
            ++_currentIndex;
            if (_currentIndex >= _circuitParent.childCount)
                _currentIndex = 0;
        }
    }
}
