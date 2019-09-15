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
            // Color col = _buttonHandle.image.color;

            /*switch (_type)
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
            }*/

            Color colToSet = GameConfigurationContainer.Instance.GetColorCode(_type);


            _bodyHandle = GameObject.Instantiate(_prefabToInstantiate[_type]);

            //HACK!
            _bodyHandle.transform.SetParent(this.transform,false);
            Vector3 dir = transform.position - CameraTransitioner.Instance.transform.position;
            dir.Normalize();
            _bodyHandle.transform.localPosition = -dir * 0.5f;
            _bodyHandle.transform.forward = -dir;
            //body.transform.localRotation = Quaternion.Euler(0, 140, 0);
            _bodyHandle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _bodyHandle.GetComponentInChildren<MeshRenderer>().material.color = colToSet;

            //end hack

            //_buttonHandle.image.color = col;
        }

        public override void DestructionRoutine()
        {
            _ppFSM = null;
        }



        public void OnClicked()
        {
            GameplayScript.Instance.IsSelected(this);
            GetComponentInChildren<Text>().text = "!";
        }

        public override void UpdateActor()
        {
            if(_ppFSM != null)
                _ppFSM.UpdateStateMachine();

            if (_bodyHandle != null)
                _bodyHandle.transform.LookAt(CameraTransitioner.Instance.transform);
        }

    }
}
