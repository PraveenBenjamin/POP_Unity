using POP.Modules.Gameplay;
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
            //do not have the time to make this scalable, introducing a prefab hack to get by.
            GameObject newMen = PrefabDatabase.Instance.GetPrefabInstance(typeof(T).ToString());
            T men = newMen.GetComponentInChildren<T>();
            return men;
        }

        public void PushMenu<T>(UnityAction onComplete = null) where T : BaseMenu
        {
            BaseMenu men = InstantiateMenuInstance<T>();

            //add transition data
            string typeKey = typeof(T).ToString();
            BaseTransitioner t = men.GetComponent<BaseTransitioner>();
            t.InitializeTransitions( Constants.globalAnimationSpeed);

            //hack
            men.transform.parent.SetParent(AppManager.Instance.UIRoot.transform, false);
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

                //HACK, notice the men.parent :/
                Destroy(men.transform.parent.gameObject);

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
