using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using UnityEditor.PackageManager;

//Borrows much from https://github.com/QuantumCalzone/UnityGitPackageUpdater
namespace Willow.Library.Editor
{
    public class GitPackageGetter : EditorWindow
    {
        [MenuItem("Alice's Stuff/Package Getter")]
        private static void Init()
        {
            GitPackageGetter window = (GitPackageGetter)GetWindow(
                t: typeof(GitPackageGetter),
                utility: true,
                title: "Alices Package Updater"
            );

            window.packageURLs = GetPackageURLsListedInTXTRecord("packages.bottino.games");
            window.minSize = new Vector2(500f, 200f);
            window.maxSize = new Vector2(500f, 1000f);
            window.ShowUtility();
        }

        public string[] packageURLs;
        Vector2 scrollpos;
        private void OnGUI()
        {
            scrollpos = EditorGUILayout.BeginScrollView(scrollpos);

            foreach(string s in packageURLs)
            {
                if (GUILayout.Button(s))
                {
                    Client.Add(s);
                }
            }

            EditorGUILayout.EndScrollView();
        }






        //ripped some code from https://localcoders.blogspot.com/2016/06/getting-dns-txt-record-using-c.html
        private static string[] /*IList<string>*/ GetPackageURLsListedInTXTRecord(string hostname)
        {

            IList<string> txtRecords = new List<string>();

            string output;

            var startInfo = new ProcessStartInfo("nslookup");

            startInfo.Arguments = string.Format("-type=TXT {0}", hostname);

            startInfo.RedirectStandardOutput = true;

            startInfo.UseShellExecute = false;

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;


            using (var cmd = Process.Start(startInfo))

            {

                output = cmd.StandardOutput.ReadToEnd();

            }

            string identifier = "packages.bottino.games	text =";
            return output.Remove(0, output.IndexOf(identifier) + identifier.Length).Trim().Trim('"').Split(',');
        }
    }

    
}
