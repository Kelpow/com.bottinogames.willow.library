using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Willow.IDLUI
{
    

    public class IDLUIDynamicCamera : IDLUICamera
    {
        public AnimationCurve transitionCurve = new AnimationCurve(new Keyframe(0,0,0,0), new Keyframe(1,1,0,0));
        
        [SerializeField] [HideInInspector] public Position[] positions = new Position[1];

        [HideInInspector] public int startingPosition;

        private bool transitioning;
        int currentPosition;

        private void Start()
        {
            if (startingPosition < 0 || startingPosition >= positions.Length)
                startingPosition = 0;

            transform.position = positions[startingPosition].position;
            transform.rotation = positions[startingPosition].rotation;
        }

        public void TransitionToPosition(string name)
        {
            if (transitioning)
            {
                Debug.LogWarning("Transition failed: attempted to transition while another transition is still in progress.", gameObject);
                return;
            }
            else
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    if(positions[i].name == name)
                    {
                        if(i == currentPosition)
                        {
                            Debug.LogWarning($"Transition failed: already at postion {name}.", gameObject);
                            return;
                        }

                        StartCoroutine(Transition(i));
                        return;
                    }
                }
                Debug.LogWarning($"Transition failed: postion {name} does not exist.", gameObject);
                return;
            }
        }

        public void SkipToPosition(string name)
        {
            if (transitioning)
            {
                Debug.LogWarning("Skip failed: attempted to skip while another transition is still in progress.", gameObject);
                return;
            }
            else
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    if (positions[i].name == name)
                    {
                        if (i == currentPosition)
                        {
                            Debug.LogWarning($"Skip failed: already at postion {name}.", gameObject);
                            return;
                        }

                        transform.position = positions[i].position;
                        transform.rotation = positions[i].rotation;
                        return;
                    }
                }
                Debug.LogWarning($"Skip failed: postion {name} does not exist.", gameObject);
                return;
            }
        }

        IEnumerator Transition(int transferTo)
        {
            transitioning = true;

            if (positions[transferTo].clearFocusOnTransition)
                Manager.ClearFocus();

            if (positions[transferTo].freezeInputDuringTransition)
            {
                Manager.FreezeInput();
            }

            float t = 0f;
            do
            {
                yield return null;
                t += Time.deltaTime;
                transform.position = Vector3.LerpUnclamped(positions[currentPosition].position, positions[transferTo].position, transitionCurve.Evaluate(t));
                transform.rotation = Quaternion.SlerpUnclamped(positions[currentPosition].rotation, positions[transferTo].rotation, transitionCurve.Evaluate(t));
            } while (t < transitionCurve.keys[transitionCurve.length-1].time);

            transform.position = positions[transferTo].position;
            transform.rotation = positions[transferTo].rotation;
            currentPosition = transferTo;
            transitioning = false;

            Manager.activeCategory = positions[currentPosition].category;

            if (positions[currentPosition].freezeInputDuringTransition)
                Manager.UnfreezeInput();

            if (positions[currentPosition].buttonToFocus)
                Manager.FocusButton(positions[currentPosition].buttonToFocus);

        }



        private void OnDrawGizmosSelected ()
        {
            for (int i = 0; i < positions.Length; i++)
            {
                Gizmos.matrix = Matrix4x4.TRS(positions[i].position, positions[i].rotation, Vector3.one);
                Gizmos.DrawFrustum(Vector3.zero, 60, 5f, 0f, 1f);
            }
        }
        

        [System.Serializable]
        public class Position
        {
            public string name;
            public Vector3 position;
            public Quaternion rotation = Quaternion.identity;
            public IDLUIButton buttonToFocus;
            public bool clearFocusOnTransition = true;
            public bool freezeInputDuringTransition = true;
            public Manager.Category category;

            public Position Copy()
            {
                Position newPosition = new Position();
                newPosition.name = name;
                newPosition.position =position;
                newPosition.rotation =rotation;
                newPosition.buttonToFocus =buttonToFocus;
                newPosition.clearFocusOnTransition =clearFocusOnTransition;
                newPosition.freezeInputDuringTransition =freezeInputDuringTransition;
                newPosition.category = category;

                return newPosition;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(IDLUIDynamicCamera))]
    public class IDLUIDynamicCameraEditor : Editor
    {
        bool previewing;
        bool capturing;
        int focusedIndex;

        Vector3 savedPosition;
        Quaternion savedRotation;

        IDLUIDynamicCamera cam;

        private void OnEnable()
        {
            cam = (IDLUIDynamicCamera)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            Undo.RecordObject(target, "IDLUIDynamicCamera Editor");

            for (int i = 0; i < cam.positions.Length; i++)
            {
                if (focusedIndex == i)
                    if (capturing)
                        UnityEngine.GUI.color = new Color(1f, .5f, .5f);
                    else if (previewing)
                        UnityEngine.GUI.color = new Color(.5f, .7f, 1f);
                    else
                        UnityEngine.GUI.color = cam.startingPosition == i ? new Color(1f,1f,0.7f) : Color.white;
                else
                    UnityEngine.GUI.color = cam.startingPosition == i ? new Color(1f, 1f, 0.7f) : Color.white;


                GUILayout.Space(8f);

                GUILayout.BeginVertical("HelpBox");
                {
                    GUILayout.BeginHorizontal();
                    {
                        float w = EditorGUIUtility.labelWidth;
                        EditorGUIUtility.labelWidth = 16f;
                        cam.positions[i].name = EditorGUILayout.TextField(i.ToString(), cam.positions[i].name);
                        EditorGUIUtility.labelWidth = w;

                        if (GUILayout.Button("Duplicate"))
                            DuplicatePosition(i, cam);
                        UnityEngine.GUI.enabled = cam.positions.Length > 1;
                        if (GUILayout.Button("Delete"))
                        {
                            DeletePosition(i, cam);
                            break;
                        }
                        UnityEngine.GUI.enabled = true;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginVertical("HelpBox");
                    {
                        cam.positions[i].buttonToFocus = EditorGUILayout.ObjectField("Force Focus", cam.positions[i].buttonToFocus, typeof(IDLUIButton), allowSceneObjects: true) as IDLUIButton;
                        cam.positions[i].clearFocusOnTransition = EditorGUILayout.Toggle("Clear Focus", cam.positions[i].clearFocusOnTransition);
                        cam.positions[i].freezeInputDuringTransition = EditorGUILayout.Toggle("Freeze Inputs", cam.positions[i].freezeInputDuringTransition);
                        cam.positions[i].category = (Manager.Category)EditorGUILayout.EnumPopup("Categories", cam.positions[i].category);
                    }
                    GUILayout.EndVertical();

                    UnityEngine.GUI.enabled = false;
                    EditorGUILayout.Vector3Field("Position", cam.positions[i].position);
                    EditorGUILayout.Vector3Field("Rotation", cam.positions[i].rotation.eulerAngles);
                    
                    GUILayout.Space(4f);
                    
                    UnityEngine.GUI.enabled = !previewing && !capturing;
                    GUILayout.BeginHorizontal("HelpBox");
                    {

                        UnityEngine.GUI.enabled = (!previewing && !capturing && cam.startingPosition != i);
                        if (GUILayout.Button("Make Default"))
                        {
                            cam.startingPosition = i;
                        }
                        UnityEngine.GUI.enabled = !previewing && !capturing;
                        if (GUILayout.Button("Camera to Postion"))
                        {
                            Undo.RecordObject(cam.transform, "IDLUI DC Set Camera to Position");
                            cam.transform.position = cam.positions[i].position;
                            cam.transform.rotation = cam.positions[i].rotation;
                        }
                        if(GUILayout.Button("Position to Camera"))
                        {
                            cam.positions[i].position = cam.transform.position;
                            cam.positions[i].rotation = cam.transform.rotation;
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(4f);

                    UnityEngine.GUI.enabled = true;
                    GUILayout.BeginHorizontal("HelpBox");
                    {
                        if ((previewing || capturing) && focusedIndex == i)
                        {
                            if (GUILayout.Button(previewing ? "Stop Previewing" : "Stop Editing"))
                            {
                                StopPreviewingOrCapturing();
                            }
                        }
                        else
                        {
                            UnityEngine.GUI.enabled = !(previewing || capturing);
                            if (GUILayout.Button("Preview Position"))
                            {
                                savedPosition = cam.transform.position;
                                savedRotation = cam.transform.rotation;
                                previewing = true;
                                capturing = false;
                                focusedIndex = i;

                                cam.transform.position = cam.positions[focusedIndex].position;
                                cam.transform.rotation = cam.positions[focusedIndex].rotation;
                            }
                            if (GUILayout.Button("Edit Position"))
                            {
                                savedPosition = cam.transform.position;
                                savedRotation = cam.transform.rotation;
                                previewing = false;
                                capturing = true;
                                focusedIndex = i;

                                cam.transform.position = cam.positions[focusedIndex].position;
                                cam.transform.rotation = cam.positions[focusedIndex].rotation;
                            }
                            UnityEngine.GUI.enabled = UnityEngine.GUI.enabled && Application.isPlaying;
                            if (GUILayout.Button("Transition"))
                            {
                                cam.TransitionToPosition(cam.positions[i].name);
                            }
                            UnityEngine.GUI.enabled = true;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical(); 
            }
            EditorGUI.indentLevel = 0;

            if (capturing)
            {
                if (focusedIndex >= cam.positions.Length || focusedIndex < 0)
                {
                    StopPreviewingOrCapturing();
                }
                else
                {
                    cam.positions[focusedIndex].position = cam.transform.position;
                    cam.positions[focusedIndex].rotation = cam.transform.rotation;
                }
            }

            if (previewing)
            {
                if (focusedIndex >= cam.positions.Length || focusedIndex < 0)
                {
                    StopPreviewingOrCapturing();
                }
                else
                {
                    cam.transform.position = cam.positions[focusedIndex].position;
                    cam.transform.rotation = cam.positions[focusedIndex].rotation;
                }
            }

        }

        private void DuplicatePosition(int toCopy, IDLUIDynamicCamera cam)
        {
            IDLUIDynamicCamera.Position[] newPositions = new IDLUIDynamicCamera.Position[cam.positions.Length + 1];
            
            for (int i = 0; i < newPositions.Length; i++)
            {
                int offset = i > toCopy ? 1 : 0;
                if (i == toCopy - 1)
                    newPositions[i - offset] = cam.positions[i - offset].Copy();
                else
                    newPositions[i] = cam.positions[i - offset];
            }

            cam.positions = newPositions;

            if (focusedIndex > toCopy)
                focusedIndex++;

        }
        private void DeletePosition(int toDelete, IDLUIDynamicCamera cam)
        {
            IDLUIDynamicCamera.Position[] newPositions = new IDLUIDynamicCamera.Position[cam.positions.Length - 1];

            for (int i = 0; i < newPositions.Length; i++)
            {
                int offset = i >= toDelete ? 1 : 0;
                newPositions[i] = cam.positions[i + offset];
            }

            if (focusedIndex > toDelete)
                focusedIndex--;
            else if (focusedIndex == toDelete)
                StopPreviewingOrCapturing();

            cam.positions = newPositions;
        }


        private void StopPreviewingOrCapturing()
        {
            if (!previewing && !capturing)
                return;

            previewing = false;
            capturing = false;

            cam.transform.position = savedPosition;
            cam.transform.rotation = savedRotation;
        }

        private void OnDisable()
        {
            
        }
    }
#endif
}