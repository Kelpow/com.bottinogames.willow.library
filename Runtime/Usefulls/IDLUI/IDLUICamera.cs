using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Willow.IDLUI
{
    public class IDLUICamera : MonoBehaviour
    {
        new public Camera camera { get; private set; }

        public Input input;

        private void Awake() 
        { 
            camera = GetComponent<Camera>(); 
            input = GetComponent<Input>(); 
            if (input == null) 
                input = new DefaultInput();
        }


        private void OnEnable()
        {
            Manager.ActivateCamera(this);
        }

        private void OnDisable()
        {
            Manager.DeactivateCamera(this);
        }

    }
}