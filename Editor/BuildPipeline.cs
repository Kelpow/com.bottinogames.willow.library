using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildPipelineWindow : EditorWindow
{
    [MenuItem("Build Pipeline/Build GUI")]
    public static void ShowWindow() { EditorWindow.GetWindow(typeof(BuildPipelineWindow), false, "Build Pipeline"); }

    bool winC;
    bool winS;
    bool linC;
    bool linS;

    bool run;

#if UNITY_EDITOR_WIN
    BuildOptions winRunOption = BuildOptions.AutoRunPlayer;
    BuildOptions linRunOption = BuildOptions.None;
#elif UNITY_EDITOR_LINUX
    BuildOptions winRunOption = BuildOptions.None;
    BuildOptions linRunOption = BuildOptions.AutoRunPlayer;
#else   
    BuildOptions winRunOption = BuildOptions.None;
    BuildOptions linRunOption = BuildOptions.None;
#endif


    public void OnGUI()
    {
        if (GUILayout.Button("Edit file and filder names"))
            BuildPipelineNamesWindow.ShowWindow(GUIUtility.GUIToScreenPoint(Event.current.mousePosition));

        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("SERVER");
                linS = GUILayout.Toggle(linS, "Linux");
                winS = GUILayout.Toggle(winS, "Windows");
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            {
                GUILayout.Label("CLIENT");
                linC = GUILayout.Toggle(linC, "Linux");
                winC = GUILayout.Toggle(winC, "Windows");
            }
            GUILayout.EndVertical();

        }
        GUILayout.EndHorizontal();
        
        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Build"))
        {

            string path = System.IO.Directory.GetCurrentDirectory() + "/Builds";
            string[] levels = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

            if (linS)
            {
                BuildPipeline.BuildPlayer(levels, $"{path}/{BuildPipelineNamesWindow.LinServerFolder}/{BuildPipelineNamesWindow.ServerExecutableName}.x86_64", BuildTarget.StandaloneLinux64, BuildOptions.EnableHeadlessMode | (run ? linRunOption : BuildOptions.None));
            }
            if (winS)
            {
                BuildPipeline.BuildPlayer(levels, $"{path}/{BuildPipelineNamesWindow.WinServerFolder}/{BuildPipelineNamesWindow.ServerExecutableName}.exe", BuildTarget.StandaloneWindows64, BuildOptions.EnableHeadlessMode | (run ? winRunOption : BuildOptions.None));
            }
            if (linC)
            {
                BuildPipeline.BuildPlayer(levels, $"{path}/{BuildPipelineNamesWindow.LinClientFolder}/{BuildPipelineNamesWindow.ClientExecutableName}.exe", BuildTarget.StandaloneLinux64, run ? linRunOption : BuildOptions.None);
            }
            if (winC)
            {
                BuildPipeline.BuildPlayer(levels, $"{path}/{BuildPipelineNamesWindow.WinClientFolder}/{BuildPipelineNamesWindow.ClientExecutableName}.exe", BuildTarget.StandaloneWindows64, run ? winRunOption : BuildOptions.None);
            }
        }
        run = GUILayout.Toggle(run, "Run");
        GUILayout.EndHorizontal();
    }
}

public class BuildPipelineNamesWindow : EditorWindow
{

    public static string ClientExecutableName { get { UpdateStrings();  return clientExecutableName; } }
    public static string ServerExecutableName { get { UpdateStrings(); return serverExecutableName; } }
    public static string WinClientFolder { get { UpdateStrings(); return winClientFolder; } }
    public static string WinServerFolder { get { UpdateStrings(); return winServerFolder; } }
    public static string LinClientFolder { get { UpdateStrings(); return linClientFolder; } }
    public static string LinServerFolder { get { UpdateStrings(); return linServerFolder; } }

    private static string clientExecutableName;
    private static string serverExecutableName;
    private static string winClientFolder;
    private static string winServerFolder;
    private static string linClientFolder;
    private static string linServerFolder;

    static void UpdateStrings()
    {
        string appname = Application.productName.Replace(" ", "");
        clientExecutableName = PlayerPrefs.GetString("editor_buildpipeline_clientExecutableName", appname);
        serverExecutableName = PlayerPrefs.GetString("editor_buildpipeline_serverExecutableName", appname + "server");
        winClientFolder = PlayerPrefs.GetString("editor_buildpipeline_winClientFolder", "Windows");
        winServerFolder = PlayerPrefs.GetString("editor_buildpipeline_winServerFolder", "Windows Server");
        linClientFolder = PlayerPrefs.GetString("editor_buildpipeline_linClientFolder", "Linux");
        linServerFolder = PlayerPrefs.GetString("editor_buildpipeline_linServerFolder", "Linux Server");
    }

    static void PushStrings()
    {
        PlayerPrefs.SetString("editor_buildpipeline_clientExecutableName", clientExecutableName);
        PlayerPrefs.SetString("editor_buildpipeline_serverExecutableName", serverExecutableName);
        PlayerPrefs.SetString("editor_buildpipeline_winClientFolder", winClientFolder);
        PlayerPrefs.SetString("editor_buildpipeline_winServerFolder", winServerFolder);
        PlayerPrefs.SetString("editor_buildpipeline_linClientFolder", linClientFolder);
        PlayerPrefs.SetString("editor_buildpipeline_linServerFolder", linServerFolder);
    }

    public static void ShowWindow(Vector2 position) 
    {
        BuildPipelineNamesWindow newWindow = ScriptableObject.CreateInstance<BuildPipelineNamesWindow>();
        newWindow.position = new Rect(position, new Vector2(500, 170));

        UpdateStrings();
        PushStrings();

        newWindow.ShowPopup();

    }

    private void OnLostFocus()
    {
        this.Close();
    }

    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("x", GUILayout.Width(20)))
        {
            this.Close();
        }
        GUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Client file name", GUILayout.Width(100));
        clientExecutableName= GUILayout.TextField(clientExecutableName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Server file name", GUILayout.Width(100));
        serverExecutableName = GUILayout.TextField(serverExecutableName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Label("Windows Folders", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Client", GUILayout.Width(45));
        winClientFolder = GUILayout.TextField(winClientFolder, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Server", GUILayout.Width(45));
        winServerFolder = GUILayout.TextField(winServerFolder, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("Linux Folders", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Client", GUILayout.Width(45));
        linClientFolder = GUILayout.TextField(linClientFolder, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Server", GUILayout.Width(45));
        linServerFolder = GUILayout.TextField(linServerFolder, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck())
        {
            PushStrings();
        }
    }
}
