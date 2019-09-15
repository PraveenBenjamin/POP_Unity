using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using UnityEngine.UI;

namespace POP.UI.Menus
{

    /// <summary>
    /// implementation of the OptionsMenu
    /// </summary>
    public class OptionsMenu : BaseMenu
    {

        [SerializeField]
        private Slider _volumeSliderHandle;
        public void OnVolumeChanged()
        {
            AudioManager.Instance.SetVolume(_volumeSliderHandle.value);
        }

        public void OnBackButton()
        {
            MenuManager.Instance.PopMenu(()=>
            {
                MenuManager.Instance.PushMenu<MainMenu>();
            });
        }


    }
}
