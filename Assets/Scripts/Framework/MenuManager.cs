using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.UI.MenuSystem;
using UnityEngine.Events;

namespace POP.Framework
{
    public class MenuManager : SingletonBehaviour<MenuManager>
    {

        

        private Stack<BaseMenu> _menuStack;


        public T InstantiateMenuInstance<T>() where T : BaseMenu
        {
            GameObject go = new GameObject(typeof(T).Name);
            T menuToAdd = go.AddComponent<T>();
            return menuToAdd;
        }

        public void PushMenu<T>() where T : BaseMenu
        {
            BaseMenu men = InstantiateMenuInstance<T>();
            men.ConstructionRoutine();
            _menuStack.Push(men);
            men.TransitionIn();
        }


        public void PopMenu()
        {
            BaseMenu men = _menuStack.Peek();
            men.TransitionOut((BaseMenu toDestroy) =>
            {
                toDestroy.DestructionRoutine();
                _menuStack.Pop();
            });
           
        }


        private void OnMenuTransitionInComplete(BaseMenu menuHandle)
        {

        }

        private void OnMenuTransitionOutComplete(BaseMenu menuHandle)
        {

        }


        private void OnMenuDestructionRoutineComplete(BaseMenu menHandle)
        {
            _menuStack.Pop();
        }

        private void OnMenuConstructionRoutineComplete(BaseMenu menHandle)
        {

        }

        protected override void InitializeSingleton()
        {
            _menuStack = new Stack<BaseMenu>();
        }

        protected override void OnDestroySingleton()
        {
            if (_menuStack != null && _menuStack.Count > 0)
            {
                foreach (BaseMenu men in _menuStack)
                {
                    men.DestructionRoutine();
                }

                _menuStack.Clear();
                _menuStack = null;
            }
        }
    }
}
