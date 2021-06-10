#if UNITY_IPHONE 
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

namespace IronSource.Editor
{
	public class AdColonySettings : IAdapterSettings
	{
		public void updateProject (BuildTarget buildTarget, string projectPath)
		{
			Debug.Log ("IronSource - Update project for AdColony");

			PBXProject project = new PBXProject ();
			project.ReadFromString (File.ReadAllText (projectPath));

			string targetId = project.TargetGuidByName (PBXProject.GetUnityTargetName ());

			// Required System Frameworks
			project.AddFrameworkToProject (targetId, "AudioToolbox.framework", false);
			project.AddFrameworkToProject (targetId, "EventKit.framework", false);
			project.AddFrameworkToProject (targetId, "EventKitUI.framework", false);
			project.AddFrameworkToProject (targetId, "MediaPlayer.framework", false);
			project.AddFrameworkToProject (targetId, "MessageUI.framework", false);
			project.AddFrameworkToProject (targetId, "JavaScriptCore.framework", false);

			project.AddFileToBuild (targetId, project.AddFile ("usr/lib/libz.1.2.5.tbd", "Frameworks/libz.1.2.5.tbd", PBXSourceTree.Sdk));

			// Optional Frameworks
			project.AddFrameworkToProject (targetId, "Social.framework", true);

			// Custom Link Flag
			project.AddBuildProperty (targetId, "OTHER_LDFLAGS", "-fobjc-arc");
			project.AddBuildProperty (targetId, "OTHER_LDFLAGS", "-ObjC");

			File.WriteAllText (projectPath, project.WriteToString ());
		}

		public void updateProjectPlist (BuildTarget buildTarget, string plistPath)
		{
			Debug.Log ("IronSource - Update plist for AdColony");
		}
	}
}
#endif
