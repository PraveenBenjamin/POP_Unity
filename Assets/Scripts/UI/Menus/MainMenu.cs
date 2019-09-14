using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using UnityEngine.UI;
using TMPro;
using POP.Modules.Gameplay;

namespace POP.UI.Menus
{
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



       

        protected override void ConstructionRoutineInternal()
        {
            //perform sanity checks here
        }

        protected override void DestructionRoutineInternal()
        {
            //release resources here
            // the main menu must only be destroyed on application exit
        }

        protected override void InputDependantUpdateRoutine()
        {
            // do i even need this?
        }

        protected override void InputIndependantUpdateRoutine()
        {
            //animate the billboard and the sky and everything to do with the environment here
        }

        protected override void InitMain()
        {
            CameraTransitioner.Instance.TransitionTo(CameraTransitioner.CameraPositions.MainMenu,null, BaseTransitioner.LerpType.Cubic, Constants.globalAnimationSpeed);
        }

        protected override void TerminateMain()
        {

        }

        protected override void UpdateMain()
        {

        }

        public void OnMainMenuButtonClicked(int mainMenuButtonType)
        {
            MainMenuButtonType type = (MainMenuButtonType)mainMenuButtonType;
            switch (type)
            {
                case MainMenuButtonType.NewGame:
                    MenuManager.Instance.PopMenu(() =>
                    {
                        CameraTransitioner.CameraPositions posToBe = CameraTransitioner.CameraPositions.LevelSelectEasy;

                        //hack, but eh its not a truly terrible one is it?
                        posToBe = (CameraTransitioner.CameraPositions)(((int)(GameConfigurationContainer.Difficulty)) + 2);

                        CameraTransitioner.Instance.TransitionTo(posToBe, () =>
                         {
                             MenuManager.Instance.PushMenu<LevelSelectMenu>();
                         }, BaseTransitioner.LerpType.Cubic, Constants.globalAnimationSpeed);
                    });
                    break;
                case MainMenuButtonType.Options:
                    
                    break;
                case MainMenuButtonType.Achievements:
                    break;
            }

        }

    }
}