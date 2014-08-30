using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.ComponentModel;

public class NxtCreateAssetBundles {

	//target platform for the assetbundles
	static BuildTarget target = BuildTarget.StandaloneWindows;

	[MenuItem("Assets/Nxtomo build Resources bundle")]
	static void ExportResourcesBundle () {

		Exporting();

	}

	static void Exporting () {

		//Ensure there is AssetBundles directory
		string outPath = Application.dataPath + "/../AssetBundles";
		if(!Directory.Exists(outPath)) {
			Directory.CreateDirectory(outPath);
		}
		//end
		
		List<UnityEngine.Object> assets = new List<UnityEngine.Object>();
		List<string> names = new List<string>();
		getAssets(assets, names);

		for(int i = 0; i < assets.Count; ++i) {
			Debug.Log(string.Format("{0} : {1}", names[i], assets[i].name));
		}

		
		
		
		BuildPipeline.BuildAssetBundleExplicitAssetNames(assets.ToArray(), names.ToArray(), "AssetBundles/docm_Resources", BuildAssetBundleOptions.CollectDependencies|BuildAssetBundleOptions.UncompressedAssetBundle, target);
	}

	public static void getAssets(List<UnityEngine.Object> assets, List<string> names)
	{
		DirectoryInfo dinfo = new DirectoryInfo(Application.dataPath);
		foreach(DirectoryInfo child in dinfo.GetDirectories()) {
			if(child.Name[0] == '.') {
				continue;
			}
			
			if(child.Name == "Resources") {
				Uri newRoot = new Uri(child.FullName);
				getAssetsPath(newRoot, child, assets, names);
			} else {
				getAssetsPath(null, child, assets, names);
			}
		}
	}

	public static void getAssetsPath(Uri root, DirectoryInfo dinfo, List<UnityEngine.Object> assets, List<string> names)
	{
		Uri dataRoot = new Uri(Application.dataPath);
		
		if(null != root) {
			Uri uri;
			foreach(FileInfo file in dinfo.GetFiles()) {
				if(file.Name[0] == '.') {
					continue;
				}
				if(file.Extension == ".meta") {
					continue;
				}
				
				int extLength = file.Extension.Length;
				
				uri = new Uri(root, file.FullName);
				string name = dataRoot.MakeRelativeUri(uri).ToString();
				
				assets.Add(AssetDatabase.LoadAssetAtPath(name, typeof(UnityEngine.Object)));
				
				name = root.MakeRelativeUri(uri).ToString();
				if(extLength > 0) {
					name = name.Substring(0, name.Length - extLength);
				}
				
				if(name.StartsWith("Resources/")) {
					names.Add(name.Substring("Resources/".Length));
				} else {
					names.Add(name);
				}
			}
		}
		
		foreach(DirectoryInfo child in dinfo.GetDirectories()) {
			if(child.Name[0] == '.') {
				continue;
			}
			
			if(child.Name == "Resources") {
				Uri newRoot = new Uri(child.FullName);
				getAssetsPath(newRoot, child, assets, names);
			} else {
				getAssetsPath(root, child, assets, names);
			}
		}
	}

	static string resDir = Application.dataPath + "/Resources/";
	static string tempDir = Application.dataPath + "/../Resouces_";

	[MenuItem("Assets/Build exec split from bundle")]
	static void BuildScene()
	{
		if(EditorUserBuildSettings.activeBuildTarget != target) {
			EditorUserBuildSettings.SwitchActiveBuildTarget(target);
		}

		if(true) {
			DirectoryInfo dinfo = new DirectoryInfo(Application.dataPath + "/Resources");
			if(dinfo.Exists) {
				MoveDirectory(resDir,tempDir);
				Debug.Log("should have moved to Resources_");
			}
		}

		
		string[] levels = new string[] { "Assets/AssetBundling/TestScenes/LoadBundle.unity" };
		
		string outPath = Application.dataPath + "/../Output";
		if(!Directory.Exists(outPath)) {
			Directory.CreateDirectory(outPath);
		}
		
		BuildOptions options = (true)
			? BuildOptions.AllowDebugging | BuildOptions.Development
				: BuildOptions.None;
		BuildPipeline.BuildPlayer(levels, "Output/play.exe", BuildTarget.StandaloneWindows, options);

		if(true) {
			DirectoryInfo dinfo = new DirectoryInfo(Application.dataPath + "/../Resources_");
			if(dinfo.Exists) {
				MoveDirectory(tempDir,resDir);
				Debug.Log("should have moved back to Resources");
			}
			AssetDatabase.Refresh();
	}
	}

	public static void MoveDirectory(string source, string target)
	{
		var stack = new Stack<Folders>();
		stack.Push(new Folders(source, target));
		
		while (stack.Count > 0)
		{
			var folders = stack.Pop();
			Directory.CreateDirectory(folders.Target);
			foreach (var file in Directory.GetFiles(folders.Source, "*.*"))
			{
				string targetFile = Path.Combine(folders.Target, Path.GetFileName(file));
				if (File.Exists(targetFile)) File.Delete(targetFile);
				File.Move(file, targetFile);
			}
			
			foreach (var folder in Directory.GetDirectories(folders.Source))
			{
				stack.Push(new Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
			}
		}
		Directory.Delete(source, true);
	}
	public class Folders
	{
		public string Source { get; private set; }
		public string Target { get; private set; }
		
		public Folders(string source, string target)
		{
			Source = source;
			Target = target;
		}
	}
	
	//key to export icon assets
	/*
	 *         List<UnityEngine.Object> assets = new List<UnityEngine.Object>();
        List<string> names = new List<string>();
        getAssets(assets, names);
        //for(int i = 0; i < assets.Count; ++i) {
        //    Debug.Log(string.Format("{0} : {1}", names[i], assets[i].name));
        //}

        BuildPipeline.BuildAssetBundleExplicitAssetNames(assets.ToArray(), names.ToArray(), "AssetBundles/docm_Resources", BuildAssetBundleOptions.CollectDependencies, target);
        levelPathes.Add(Application.dataPath + "/../AssetBundles/docm_Resources");
        */
}
