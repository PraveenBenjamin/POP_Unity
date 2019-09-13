using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public void PushMenu<T>(UnityAction onComplete = null) where T : BaseMenu
        {
            BaseMenu men = InstantiateMenuInstance<T>();
            men.ConstructionRoutine();
            _menuStack.Push(men);
            men.TransitionIn((BaseMenu menu) =>
            {
                OnMenuTransitionInComplete(menu);
                onComplete?.Invoke();
            }); 
        }


        public void PopMenu(UnityAction onComplete = null)
        {
            BaseMenu men = _menuStack.Peek();
            men.TransitionOut((BaseMenu menu) =>
            {
                OnMenuTransitionOutComplete(menu);
                onComplete?.Invoke();
            });

        }

        public T PeekMenu<T>() where T:BaseMenu
        {
            return (T)_menuStack.Peek();
        }


        private void OnMenuTransitionInComplete(BaseMenu menuHandle)
        {
            // do i even need this?
            
        }

        private void OnMenuTransitionOutComplete(BaseMenu menuHandle)
        {
            menuHandle.DestructionRoutine();
            _menuStack.Pop();
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

        public void UpdateMenuManager()
        {
            if (_menuStack.Count > 0)
                _menuStack.Peek().UpdateMenu();
        }
    }
}
