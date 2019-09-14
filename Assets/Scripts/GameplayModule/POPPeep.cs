using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using UnityEngine.UI;

namespace POP.Modules.Gameplay
{
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
            Init,
            Idle,
            Shifty,
            Worried,
            Talkative,
            Selected,
            Matched,
            NotMatched,
            Destroyed
        }

        protected FSM<PopPeepStates> _ppFSM;
        protected PopPeepTypes _type;
        protected Vector2 _arrayPos;


        public PopPeepTypes Type
        {
            get
            {
                return _type;
            }
        }
        private Button _buttonHandle;

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

        public void SetArrayPos(int row, int col)
        {
            _arrayPos.x = row;
            _arrayPos.y = col;
        }

        private void InitializeType()
        {
            Color col = _buttonHandle.image.color;
            switch (_type)
            {
                case PopPeepTypes.Red:
                    col = Color.red;
                    break;
                case PopPeepTypes.Green:
                    col = Color.green;
                    break;
                case PopPeepTypes.Blue:
                    col = Color.blue;
                    break;
                case PopPeepTypes.Black:
                    col = Color.black;
                    break;
                case PopPeepTypes.White:
                    col = Color.white;
                    break;
            }

            _buttonHandle.image.color = col;
        }

        public override void DestructionRoutine()
        {
            _ppFSM = null;
        }


        public void InitSelected()
        {
            GameplayScript.Instance.IsSelected(this);
            GetComponentInChildren<Text>().text = "!";
        }

        public void UpdateSelected()
        {
            
        }

        public void TerminateSelected()
        {
            GameplayScript.Instance.IsSelected(this);
            GetComponentInChildren<Text>().text = "";
        }


        public void OnClicked()
        {
            _ppFSM.SetState(PopPeepStates.Selected);
        }

        public override void UpdateActor()
        {
            if(_ppFSM != null)
                _ppFSM.UpdateStateMachine();
        }

    }
}
