using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using UnityEngine.UI;
using POP.Misc;

namespace POP.Modules.Gameplay
{

    /// <summary>
    /// The only actor we will ever need!
    /// Derived from BaseActor. This is the actor whose matching makes up the entirety of the gameplay
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Button))]
    public class PopPeep : BaseActor
    {
        public enum PopPeepTypes
        {
            Red = 0,
            Green,
            Blue,
            Black,
            White
        }

        public enum PopPeepStates
        {
            Idle,
            Selected,
            Matched
        }

        protected FSM<PopPeepStates> _ppFSM;
        protected PopPeepTypes _type;
        protected Vector2 _arrayPos;


        [SerializeField]
        PopPeepTypeGODict _prefabToInstantiate;

        public Vector2 ArrayPos
        {
            get
            {
                return _arrayPos;
            }
        }


        public PopPeepTypes Type
        {
            get
            {
                return _type;
            }
        }
        private Button _buttonHandle;
        private GameObject _bodyHandle;


        private string _idleAnimationSuffix = "Idle";
        private string _selectedAnimationSuffix = "Walk";
        private string _matchedAnimationSuffix = "Jump";

        private int _timerIndex = 0;

        public override void CreationRoutine()
        {
            _ppFSM = new FSM<PopPeepStates>();
            _ppFSM.Initialize(this);

            _buttonHandle = GetComponent<Button>();
        }

        public void Enable(bool enable)
        {
            _buttonHandle.interactable = enable;
        }

        public void SetType(PopPeepTypes toSet)
        {
            _type = toSet;
            InitializeType();
        }

        public void SetArrayPos(Vector2 arrayPos)
        {
            _arrayPos = arrayPos;
        }


        public void SetArrayPos(int row, int col)
        {
            _arrayPos.x = row;
            _arrayPos.y = col;
        }

        private void InitializeType()
        {

            Color colToSet = GameConfigurationContainer.Instance.GetColorCode(_type);

            _buttonHandle.image.color = colToSet;
            _bodyHandle = GameObject.Instantiate(_prefabToInstantiate[_type]);
            _bodyHandle.name = _prefabToInstantiate[_type].name;

            //HACK!
            _bodyHandle.transform.SetParent(this.transform,false);
            ResetBodyPlacement();
            //body.transform.localRotation = Quaternion.Euler(0, 140, 0);
            _bodyHandle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _bodyHandle.GetComponentInChildren<MeshRenderer>().material.color = colToSet;

            //end hack

            //_buttonHandle.image.color = col;
        }

        private void ResetBodyPlacement()
        {
            Vector3 dir = transform.position - CameraBehaviour.Instance.transform.position;
            dir.Normalize();
            _bodyHandle.transform.localPosition = -dir * 0.5f;
            _bodyHandle.transform.localPosition += Vector3.down * 0.33f;
            _bodyHandle.transform.forward = -dir;
        }

        public override void DestructionRoutine()
        {
            _ppFSM = null;
        }

        private void InitSelected()
        {
            GetComponentInChildren<Animator>().Play(_bodyHandle.name + _selectedAnimationSuffix);
            TemporaryVariableManager.SetTemporaryVariable<float>(this, _timerIndex, 0, true);
        }

        private void UpdateSelected()
        {
            float timer = TemporaryVariableManager.GetTemporaryVariable<float>(this, _timerIndex);
            timer += Time.deltaTime;
            if (timer > GameConfigurationContainer.MaxSelectedTime)
            {
                _ppFSM.SetState(PopPeepStates.Idle);
                GameplayScript.Instance.IsSelected(null);
            }
            TemporaryVariableManager.SetTemporaryVariable<float>(this, _timerIndex, timer, true);
        }

        private void InitMatched()
        {

            GetComponentInChildren<Animator>().Play(_bodyHandle.name + _matchedAnimationSuffix);
        }


        private void InitIdle()
        {
            GetComponentInChildren<Animator>().Play(_bodyHandle.name + _idleAnimationSuffix);
            ResetBodyPlacement();
        }

        public void SetState(PopPeepStates toSet)
        {
            _ppFSM.SetState(toSet);
        }


        private void OnClicked()
        {
            _ppFSM.SetState(PopPeepStates.Selected);
            GameplayScript.Instance.IsSelected(this);
        }

        public override void UpdateActor()
        {
            if(_ppFSM != null)
                _ppFSM.UpdateStateMachine();

            if (_bodyHandle != null)
                _bodyHandle.transform.LookAt(CameraBehaviour.Instance.transform);
        }

    }
}
