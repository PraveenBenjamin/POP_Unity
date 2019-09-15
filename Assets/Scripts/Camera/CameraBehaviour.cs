using POP.Framework;
using POP.Modules.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Part of the POP Misc namespace because it is inextricably tied to the POP application itself, but can be replaced with other scripts if necessary
namespace POP.Misc
{

    /// <summary>
    /// Camera Behaviour:- Singleton that controls the behaviour of the camera
    /// A script that exposes functions that allows the camera to be transformed to positions and orientations set in the editor
    /// </summary>
    public class CameraBehaviour : SingletonBehaviour<CameraBehaviour>
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

        //same as above
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


        protected override void InitializeSingleton()
        {
            // set to initial position
            transform.position = _possibleCameraPositions[0];
            transform.rotation = Quaternion.Euler(_possibleCameraOrientations[0]);
        }

        /// <summary>
        /// Transforms the camera to a position and rotation defined in the editor, indexed by the CameraPositions enum
        /// </summary>
        /// <param name="position">transformation indexer</param>
        /// <param name="onComplete">callback that will be called after the transformation</param>
        /// <param name="lerpType">type of smoothing to apply</param>
        /// <param name="transitionTime">time to animate transformation</param>
        public void TransitionTo(CameraPositions position, UnityAction onComplete = null, BaseTransitioner.LerpType lerpType = BaseTransitioner.LerpType.Cubic, float transitionTime = 2)
        {
            TransitionTo(_possibleCameraPositions[(int)position], _possibleCameraOrientations[(int)position], onComplete, lerpType, transitionTime);
        }

        /// <summary>
        /// Transforms the camera to the position and rotation passed as parameter
        /// </summary>
        /// <param name="position">position to transition to</param>
        /// <param name="rotation">rotation to transition to</param> 
        /// <param name="onComplete">callback that will be called after the transformation</param>
        /// <param name="lerpType">type of smoothing to apply</param>
        /// <param name="transitionTime">time to animate transformation</param>
        public void TransitionTo(Vector3 position, Vector3 rotation, UnityAction onComplete = null, BaseTransitioner.LerpType lerpType = BaseTransitioner.LerpType.Cubic, float transitionTime = 2)
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


        protected override void UpdateSingleton()
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


    }
}
