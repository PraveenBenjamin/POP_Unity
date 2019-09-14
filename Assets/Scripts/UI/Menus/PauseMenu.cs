using POP.Framework;
using POP.Modules.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace POP.UI.Menus
{

    public class PauseMenu : BaseMenu
    {

        public void OnBackToMainMenuClicked()
        {
            //TODO
        }

        public void OnResumeClicked()
        {
            MenuManager.Instance.PopMenu(() =>
            {
                GameplayScript.Instance.Resume();
            });
        }

        public void OnOptionsClicked()
        {
          //TODO
        }

        protected override void ConstructionRoutineInternal()
        {
           
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
            //throw new System.NotImplementedException();
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
