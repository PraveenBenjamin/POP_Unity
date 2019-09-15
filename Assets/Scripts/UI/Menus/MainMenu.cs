using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using UnityEngine.UI;
using TMPro;
using POP.Modules.Gameplay;
using POP.Misc;

namespace POP.UI.Menus
{

    /// <summary>
    /// implementation of the MainMenu
    /// </summary>
    public class MainMenu : BaseMenu
    {

        public class BillboardDatum
        {
            public Sprite BillboardImage;
            public string TitleText;
            public string MessageText;
        }


        public enum MainMenuButtonType
        {
            NewGame = 0,
            Options,
            Achievements
        }


        [SerializeField]
        private RectTransform _titlebar;

        [SerializeField]
        private Image _billboardImage;

        [SerializeField]
        private BillboardDatum[] _billboardData;



       

        public void OnMainMenuButtonClicked(int mainMenuButtonType)
        {
            MainMenuButtonType type = (MainMenuButtonType)mainMenuButtonType;
            switch (type)
            {
                case MainMenuButtonType.NewGame:
                    MenuManager.Instance.PopMenu(() =>
                    {
                        CameraBehaviour.CameraPositions posToBe = CameraBehaviour.CameraPositions.LevelSelectEasy;

                        //hack, but eh its not a truly terrible one is it?
                        posToBe = (CameraBehaviour.CameraPositions)(((int)(GameConfigurationContainer.Difficulty)) + 2);

                        CameraBehaviour.Instance.TransitionTo(posToBe, () =>
                         {
                             MenuManager.Instance.PushMenu<LevelSelectMenu>();
                         }, BaseTransitioner.LerpType.Cubic, Constants.globalAnimationSpeed);
                    });
                    break;
                case MainMenuButtonType.Options:
                    MenuManager.Instance.PopMenu(() =>
                    {
                        MenuManager.Instance.PushMenu<OptionsMenu>();
                    });
                        
                    break;
                case MainMenuButtonType.Achievements:
                    break;
            }

        }

    }
}