using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace POP.Framework
{
    public class PrefabDatabase : SingletonBehaviour<PrefabDatabase>
    {
        // Start is called before the first frame update
        [SerializeField]
        private StringGODict _prefabDatabase;

        public GameObject GetPrefabInstance(string key)
        {
            if (!_prefabDatabase.ContainsKey(key))
            {
                Debug.LogError("No prefab for key: " + key);
                return null;
            }
            return GameObject.Instantiate(_prefabDatabase[key]);
        }

        protected override void InitializeSingleton()
        {
            //throw new System.NotImplementedException();
        }

        protected override void OnDestroySingleton()
        {
            //throw new System.NotImplementedException();
        }
    }
}
