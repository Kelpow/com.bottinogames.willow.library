using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

public class AudioGroupBuildTester : IProcessSceneWithReport
{
    const string MENU_NAME = "Alice's Stuff/Audio/Test audio mixer groups on build";
    const string PREF_NAME = "CheckMixerGroupBeforeBuilds";

    public int callbackOrder { get { return 0; } }

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        if (!EditorPrefs.GetBool(PREF_NAME, false))
            return;

        GameObject[] bases = scene.GetRootGameObjects();

        bool toThrow = false;

        foreach (GameObject b in bases)
        {
            foreach (AudioSource s in b.GetComponentsInChildren<AudioSource>())
            {
                if (s.outputAudioMixerGroup == null)
                {
                    Debug.LogError($"AudioSource on \"{s.name}\" in scene \"{scene.name}\" does not have a Mixer Group assigned.");
                    toThrow = true;
                }
            }
        }

        if (toThrow)
            throw new BuildFailedException("Build Failed due to improper Audio Mixer Setup.");
    }


    [MenuItem(MENU_NAME, priority = 25)]
    public static void ToggleMixerGroupTesting() { EditorPrefs.SetBool(PREF_NAME, !EditorPrefs.GetBool(PREF_NAME, false)); }

    [MenuItem(MENU_NAME, validate = true)]
    public static bool ToggleMixerGroupTestingValidate() { Menu.SetChecked(MENU_NAME, EditorPrefs.GetBool(MENU_NAME, false)); return true; }
}