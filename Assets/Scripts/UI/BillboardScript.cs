using POP.Framework;
using POP.Modules;
using POP.Modules.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace POP.Misc
{

    public class BillboardScript : SingletonBehaviour<BillboardScript>
    {
        [System.Serializable]
        public class BillboardDatum
        {
            public Sprite _image;
            public string _title;
            public PopPeep.PopPeepTypes _type;
            public string _flavor;
            public Color _flavorCol;
        }


        public enum BillboardScreenStates
        {
            TransitionIn,
            Idle,
            TransitionOut,
        }


        FSM<BillboardScreenStates> _bbFSM;


        [SerializeField]
        private BillboardDatum[] _data;

        [SerializeField]
        private Image _billboardRoot;

        [SerializeField]
        private Image _billboardImage;

        [SerializeField]
        TextMeshProUGUI _billboardHeading;


        public string HeadingText
        {
            get
            {
                return _billboardHeading.text;
            }
            set
            {
                _billboardHeading.text = value;
            }
        }

        [SerializeField]
        TextMeshProUGUI _billboardMessage;
        public string MessageText
        {
            get
            {
                return _billboardMessage.text;
            }
            set
            {
                _billboardMessage.text = value;
            }
        }

        protected override void InitializeSingleton()
        {
            _bbFSM = new FSM<BillboardScreenStates>();
            _bbFSM.Initialize(this);
        }
        private float _billboardTimer = 0;
        private int _bbDataIndex = -1;

        private void InitTransitionIn()
        {
            _billboardTimer = 0;
            ++_bbDataIndex;
            if (_bbDataIndex >= _data.Length)
                _bbDataIndex = 0;

            ChangeBillboardAppearence(_bbDataIndex);
        }

        private void UpdateTransitionIn()
        {
            _billboardTimer += Time.deltaTime;

            float nVal = _billboardTimer / GameConfigurationContainer.BillboardTransitionTime;
            ChangeAlpha(nVal);

            if (nVal >= 1)
            {
                _bbFSM.SetState(BillboardScreenStates.Idle);
            }
        }

        private void TerminateTransitionOut()
        {
            _billboardTimer = 0;
        }


        private void UpdateIdle()
        {
            _billboardTimer += Time.deltaTime;

            if (_billboardTimer >= GameConfigurationContainer.BillboardHangTime)
            {
                _bbFSM.SetState(BillboardScreenStates.TransitionOut);
            }
        }


        private void InitTransitionOut()
        {
            _billboardTimer = 0;

        }

        private void UpdateTransitionOut()
        {
            _billboardTimer += Time.deltaTime;

            float nVal = _billboardTimer / GameConfigurationContainer.BillboardTransitionTime;
            ChangeAlpha( 1- nVal);

            if (nVal >= 1)
            {
                _bbFSM.SetState(BillboardScreenStates.TransitionIn);
            }
        }


        private void ChangeBillboardAppearence(int index)
        {
            _billboardImage.sprite = _data[index]._image;
            HeadingText = _data[index]._title;

            //HACK! basically to skip myself since i dont have a poppeeptype to go with my name :D :p
            if (index < _data.Length - 1)
            {
                Color col =  GameConfigurationContainer.Instance.GetColorCode(_data[index]._type);
                col.a = 0;
                _billboardHeading.color = col;
            }
               

            MessageText = _data[index]._flavor;
            _billboardMessage.color = _data[index]._flavorCol;
        }

        protected void ChangeAlpha(float normalizedAlpha)
        {

            Color col = _billboardImage.color;
            col.a = normalizedAlpha;
            _billboardImage.color = col;

            col = _billboardHeading.color;
            col.a = normalizedAlpha;
            _billboardHeading.color = col;

            col = _billboardMessage.color;
            col.a = normalizedAlpha;
            _billboardMessage.color = col;
        }

        public void EnableBillboard(bool enable)
        {
            _billboardRoot.gameObject.SetActive(enable);
        }

        public void Update()
        {
            _bbFSM.UpdateStateMachine();
        }


        // i know i can make this cleaner, but i seriously dont have the time


        protected override void OnDestroySingleton()
        {
            System.Array.Clear(_data, 0, _data.Length);
        }
    }
}
