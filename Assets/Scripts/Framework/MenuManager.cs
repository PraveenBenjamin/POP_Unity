using POP.Modules.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace POP.Framework
{
    /// <summary>
    /// Manages the menu stack of the game by pushing and popping game menus as instructed through its exposed functions
    /// </summary>
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

        /// <summary>
        /// pushes a basemenu to the stack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onComplete"></param>
        public void PushMenu<T>(UnityAction onComplete = null) where T : BaseMenu
        {
            BaseMenu men = InstantiateMenuInstance<T>();

            //add transition data
            string typeKey = typeof(T).ToString();
            BaseTransitioner t = men.GetComponent<BaseTransitioner>();
            t.InitializeTransitions( Constants.globalAnimationSpeed);

            //hack
            // Unity needs a canvas to draw UI elements on. 
            // Every menu prefab is forced to have one, so it is reasonably safe to assume it will always be there
            // still, a generic way to do this would be better
            Transform canvas = men.transform.parent;
            men.transform.SetParent(AppManager.Instance.UIRoot.transform, false);
            if (canvas != null)
                DestroyImmediate(canvas.gameObject);

            men.ConstructionRoutine();
            _menuStack.Push(men);
            men.TransitionIn((BaseMenu menu) =>
            {
                //OnMenuTransitionInComplete(menu);
                onComplete?.Invoke();
            }); 
        }


        /// <summary>
        /// pops the menu at the top of the stack
        /// </summary>
        /// <param name="onComplete"></param>
        public void PopMenu(UnityAction onComplete = null)
        {
            BaseMenu men = _menuStack.Peek();
            men.TransitionOut((BaseMenu menu) =>
            {
                OnMenuTransitionOutComplete(menu);

                //HACK, notice the men.parent :/
                Destroy(men.transform.gameObject);

                onComplete?.Invoke();
            });

        }

        /// <summary>
        /// returns a reference to the menu at the top of the stack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T PeekMenu<T>() where T:BaseMenu
        {
            return (T)_menuStack.Peek();
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

        /// <summary>
        /// updates the menu at the top of the stack
        /// </summary>
        public void UpdateMenuManager()
        {
            if (_menuStack.Count > 0)
                _menuStack.Peek().UpdateMenu();
        }
    }
}
