using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Willow.Library
{
    public class Monoton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        public static T singleton
        {
            get
            {
                if (instance == null)
                {
                    GameObject newGO = new GameObject($"{typeof(T)} Singleton", typeof(T));
                    GameObject.DontDestroyOnLoad(newGO);
                    instance = newGO.GetComponent<T>();
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance)
            {
                Debug.LogError($"An instance of {typeof(T)} already exists!");
                if (Application.isPlaying)
                    Destroy(this.gameObject);
                else
                    DestroyImmediate(this.gameObject);
            }
        }
    }
}