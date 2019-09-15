using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using TMPro;
using POP.Modules.Gameplay;

namespace POP.UI.Menus
{

    /// <summary>
    /// implementation of the InGameMenu
    /// </summary>
    public class InGameMenu : BaseMenu
    {
        [SerializeField]
        TextMeshProUGUI _timerText;
        public string TimerText
        {
            get
            {
                //the font i like doesnt have 0s :/
                string toReturn = _timerText.text.Replace("O", "0");
                return toReturn;
            }
            set
            {
                string toSet = value.Replace("0", "O");
                _timerText.text = toSet;
            }
        }

        [SerializeField]
        TextMeshProUGUI _scoreText;
        public string ScoreText
        {
            get
            {
                //the font i like doesnt have 0s :/
                string toReturn = _scoreText.text.Replace("O", "0");
                return toReturn;
            }
            set
            {
                string toSet = value.Replace("0", "O");
                _scoreText.text = toSet;
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

    }
}