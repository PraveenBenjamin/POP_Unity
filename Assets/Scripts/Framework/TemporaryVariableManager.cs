using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace POP.Framework
{
    public static class TemporaryVariableManager
    {
        public static Dictionary<UnityEngine.Object, Dictionary<int, object>> _temporaryVariables = new Dictionary<UnityEngine.Object, Dictionary<int, object>>();

        public static UnityAction GetMaintainenceDelegate()
        {
            Debug.Log("GetMaintainenceDelegate called, ensure it isnt called unnecessarily, for performance reasons. Recommendation:- once per update.");
            return PerformMaintainence;
        }

        private static Dictionary<int, object> GetDictionary(UnityEngine.Object instanceBoundTo, int index)
        {
            Dictionary<int, object> dic = null;
            if (!_temporaryVariables.ContainsKey(instanceBoundTo))
                _temporaryVariables.Add(instanceBoundTo, new Dictionary<int, object>());


            dic = _temporaryVariables[instanceBoundTo];

            return dic;
        }

        public static T GetTemporaryVariable<T>(UnityEngine.Object instanceBoundTo, int index)
        {
            Dictionary<int, object> dic = GetDictionary(instanceBoundTo,index);

            if (!dic.ContainsKey(index))
                dic.Add(index, (T)Activator.CreateInstance(typeof(T)));

            return (T)dic[index];
            
        }

        public static bool SetTemporaryVariable<T>(UnityEngine.Object instanceBoundTo, int index,T value,bool forceAdd = false)
        {

            Dictionary<int, object> dic = GetDictionary(instanceBoundTo, index);

            if (dic.ContainsKey(index))
                dic[index] = (object)value;
            else if (forceAdd)
                dic.Add(index, value);

            return dic.ContainsKey(index);

        }



        private static void PerformMaintainence()
        {
            var badKeys = _temporaryVariables.Where(pair => pair.Key == null || pair.Value.Count == 0)
                        .Select(pair => pair.Key)
                        .ToList();
            foreach (var badKey in badKeys)
            {
                _temporaryVariables.Remove(badKey);
            }
        }
    }

}