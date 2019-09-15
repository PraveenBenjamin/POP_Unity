using POP.Framework;
using POP.Modules.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace POP.UI.Menus
{
    /// <summary>
    /// implementation of the PauseMenu
    /// </summary>
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

    }
}
