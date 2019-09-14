using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using TMPro;
using POP.Modules.Gameplay;

namespace POP.UI.Menus
{
    public class InGameMenu : BaseMenu
    {
        [SerializeField]
        TextMeshProUGUI _timerText;
        public string TimerText
        {
            get
            {
                return _timerText.text;
            }
            set
            {
                _timerText.text = value;
            }
        }

        [SerializeField]
        TextMeshProUGUI _scoreText;
        public string ScoreText
        {
            get
            {
                return _scoreText.text;
            }
            set
            {
                _scoreText.text = value;
            }
        }

        [SerializeField]
        TextMeshProUGUI _countdownText;
        public string CountdownText
        {
            get
            {
                return _countdownText.text;
            }
            set
            {
                _countdownText.text = value;
            }
        }

        public Vector2 CountdownTextScale
        {
            get
            {
                return _countdownText.transform.localScale;
            }
            set
            {
                _countdownText.transform.localScale = value;
            }

        }

        public Color CountdownTextColor
        {
            get
            {
                return _countdownText.color;
            }
            set
            {
                _countdownText.color = value;
            }

        }




        public void OnPauseButtonClicked()
        {
            GameplayScript.Instance.Pause();
            MenuManager.Instance.PushMenu<PauseMenu>();
        }

        protected override void ConstructionRoutineInternal()
        {
            //throw new System.NotImplementedException();
        }

        protected override void DestructionRoutineInternal()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitMain()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InputDependantUpdateRoutine()
        {
           // throw new System.NotImplementedException();
        }

        protected override void TerminateMain()
        {
            //throw new System.NotImplementedException();
        }

        protected override void UpdateMain()
        {
            //throw new System.NotImplementedException();
        }

    }
}