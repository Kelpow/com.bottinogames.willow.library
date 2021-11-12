using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Willow.Library;

namespace Willow.IDLUI
{
    public static class Manager
    {
        public enum Category : int
        {
            Default,
            MainMenu,
            PauseMenu,
            InGame,
            Alpha,
            Beta,
            Gamma,
            Delta,
            Epsilon,
#if IDLUI_EXTRA_CATEGORIES
            Zeta,
            Eta,
            Theta,
            Iota,
            Kappa,
            Lambda,
            Mu,
            Nu,
            Xi,
            Omicron,
            Pi,
            Rho,
            Sigma,
            Tau,
            Upsilon,
            Phi,
            Chi,
            Psi,
            Omega
#endif
        }


        public static void Init () {
            if (!_instance)
            {
                GameObject instanceGO = new GameObject("IDLUI Manager", typeof(IDLUIManager));
                instanceGO.hideFlags = HideFlags.DontSave;
                GameObject.DontDestroyOnLoad(instanceGO);
                _instance = instanceGO.GetComponent<IDLUIManager>();
            }
        }
        private static IDLUIManager _instance;
        public static IDLUIManager instance
        {
            get
            {
                Init();
                return _instance;
            }
        }

        private static List<IDLUIButton> _activeButtons;
        private static List<IDLUIButton> activeButtons { get { if (_activeButtons == null) { _activeButtons = new List<IDLUIButton>(); } return _activeButtons; } } 

        public static void AddActiveButton(IDLUIButton button)
        {
            Init();
            IDLUIManager instance = Manager.instance;
            activeButtons.Add(button);
            activeButtons.Sort((button1,button2)=> -button1.priority.CompareTo(button2.priority));
        }

        public static void RemoveActiveButton(IDLUIButton button)
        {
            activeButtons.Remove(button);
        }

        private static bool isFocused;
        private static IDLUIButton focus;

        public static void FocusButton(IDLUIButton button, bool ignoreOnAnalogue = true)
        {

            if (!button)
                return;
            if (button.category != activeCategory)
                return;

            if(button == focus)
            {
                if (isFocused)
                    return;
                else if (!(usingAnalogue && ignoreOnAnalogue))
                {
                    button.onGainFocus.Invoke();
                    isFocused = true;
                }
                else
                    return;
            }
            else
            {
                if (focus && isFocused)
                    focus.onLoseFocus.Invoke();
                
                focus = button;

                if (!(usingAnalogue && ignoreOnAnalogue))
                {
                    focus.onGainFocus.Invoke();
                    isFocused = true;
                }
            }

        }

        public static void ClearFocus()
        {
            if (focus && isFocused)
                focus.onLoseFocus.Invoke();

            focus = null;
            isFocused = false;
        }

        static bool freezeInput;

        public static void FreezeInput()
        {
            freezeInput = true;
        }

        public static void UnfreezeInput()
        {
            freezeInput = false;
        }

        private static bool usingAnalogue;

        private static IDLUICamera camera;
        public static void ActivateCamera(IDLUICamera camera)
        {
            Init();

            if (Manager.camera)
            {
                Debug.LogWarning("An IDLUI Camera was already active. Deactivating and replacing.");
                Manager.camera.enabled = false;
            }

            Manager.camera = camera;
        }

        public static void DeactivateCamera(IDLUICamera camera)
        {
            if (Manager.camera == camera)
                Manager.camera = null;
        }

        public static Category activeCategory;

        public class IDLUIManager : MonoBehaviour
        {
            private void Update()
            {
                if (Manager.camera == null || activeButtons.Count == 0 || freezeInput)
                    return;

                Input input = Manager.camera.input; 

                Direction digitalInputDir = input.Digital_GetDirInput();

                if (!focus)
                    focus = RecoverLostFocus();
                if (focus.category != activeCategory)
                    focus = RecoverLostFocus();

                if (digitalInputDir != Direction.None || input.Digital_select)
                {
                    if (usingAnalogue)
                    {
                        usingAnalogue = false;
                        if (focus && !isFocused)
                        {
                            isFocused = true;
                            focus.onGainFocus.Invoke();
                        }
                    }
                    else if (input.Digital_select)
                    {
                        if(focus)
                        {
                            if (isFocused)
                            {
                                focus.onSelect.Invoke();
                            }
                            else
                            {
                                isFocused = true;
                                focus.onGainFocus.Invoke();
                            }
                        }
                        else
                        {
                            focus = RecoverLostFocus();
                        }
                    }
                    else
                    {
                        switch (digitalInputDir)
                        {
                            case Direction.Up:
                                if (focus.up)
                                {
                                    FocusButton(focus.up);
                                }
                                break;
                            case Direction.Down:
                                if (focus.down)
                                {
                                    FocusButton(focus.down);
                                }
                                break;
                            case Direction.Left:
                                if (focus.left)
                                {
                                    FocusButton(focus.left);
                                }
                                break;
                            case Direction.Right:
                                if (focus.right)
                                {
                                    FocusButton(focus.right);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                else 
                {
                    if (!usingAnalogue)
                        if (input.Analogue_select || input.Analogue_screenDelta.sqrMagnitude > 0f)
                            usingAnalogue = true;

                    if (usingAnalogue)
                    {
                        IDLUIButton hovered = null;
                        foreach (IDLUIButton button in activeButtons)
                        {
                            if (button.category == activeCategory && Manager.camera.camera.ScreenPositionOverlapsBounds(button.bounds, input.Analogue_screenPosition, button.transform))
                            {
                                hovered = button;
                                break;
                            }
                        }

                        if (hovered == null)
                        {
                            if(focus && isFocused)
                            {
                                focus.onLoseFocus.Invoke();
                                isFocused = false;
                            }
                        } 
                        else
                        {
                            FocusButton(hovered, false);
                            if (input.Analogue_select)
                                hovered.onSelect.Invoke();
                        }
                    }
                }
            }

            private void OnGUI()
            {
                if (focus)
                    GUILayout.Label("Focus: " + focus.name);
                else
                    GUILayout.Label("Focus: ");
                GUILayout.Label("Focused: " + isFocused);
                GUILayout.Label("Analogue: " + usingAnalogue);
            }
        }

        private static IDLUIButton RecoverLostFocus()
        {
            ClearFocus();

            foreach (IDLUIButton button in activeButtons)
            {
                if (button.category == activeCategory)
                    return button;
            }
            return null;
        }
    }
    
}