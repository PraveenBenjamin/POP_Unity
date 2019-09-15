using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace POP.Framework
{

    /// <summary>
    /// Maintains references to all prefabs that may need to be instantiated at runtime
    /// </summary>
    public class PrefabDatabase : SingletonBehaviour<PrefabDatabase>
    {
        // Start is called before the first frame update
        [SerializeField]
        private StringGODict _prefabDatabase;

        /// <summary>
        /// Instantiate the prefab indexed by the key. the key is usually the type of the prefab, but may be anything you wish to set in the editor
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public GameObject GetPrefabInstance(string key)
        {
            if (!_prefabDatabase.ContainsKey(key))
            {
                Debug.LogError("No prefab for key: " + key);
                return null;
            }
            return GameObject.Instantiate(_prefabDatabase[key]);
        }
    }
}
