using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

public class NxtCreateAssetBundles {

	//target platform for the assetbundles
	static BuildTarget target = BuildTarget.StandaloneWindows;

	//url: http://www.dotblogs.com.tw/yc421206/archive/2010/08/10/17108.aspx
	//url: http://wiki.unity3d.com/index.php?title=Save_and_Load_from_XML
	//url: http://msdn.microsoft.com/zh-tw/library/a8ta6tz4(v=vs.110).aspx

	[MenuItem("Assets/XML Test Save")]
	static void LaunchXMLTestSave(){

		//save a xml
		/*XmlDocument doc = new XmlDocument();
		XmlDeclaration xmldecl;
		xmldecl = doc.CreateXmlDeclaration("1.0","UTF-8",null);
*/

		StreamWriter writer; 
		FileInfo t = new FileInfo(Application.dataPath+"/../AssetBundles/"+ "Test.xml"); 
		if(!t.Exists) 
		{ 
			writer = t.CreateText(); 
		} 
		else 
		{ 
			t.Delete(); 
			writer = t.CreateText(); 
		} 
		//writer.Write("hihi"); 
		XmlDocument doc = new XmlDocument();
		XmlDeclaration xmldecl;
		xmldecl = doc.CreateXmlDeclaration("1.0","UTF-8",null);
		XmlElement company = doc.CreateElement("Company");

		doc.AppendChild(company);
		doc.InsertBefore(xmldecl,company);

		//建立子節點
		XmlElement department = doc.CreateElement("Department");
		department.SetAttribute("部門名稱", "技術部");//設定屬性
		department.SetAttribute("部門負責人", "余小章");//設定屬性
		//加入至company節點底下
		company.AppendChild(department); 
		
		XmlElement members = doc.CreateElement("Members");//建立節點
		//加入至department節點底下
		department.AppendChild(members); 
		
		XmlElement info = doc.CreateElement("Information");
		info.SetAttribute("名字", "余小章");
		info.SetAttribute("電話", "0806449");
		//加入至members節點底下
		members.AppendChild(info);
		info = doc.CreateElement("Information");
		info.SetAttribute("名字", "王大明");
		info.SetAttribute("電話", "080644978");
		//加入至members節點底下
		members.AppendChild(info);

		writer.Write(doc.InnerXml);

		writer.Close(); 
		Debug.Log("File written."); 
	}

	[MenuItem("Assets/XML Test Load")]
	static void LaunchXMLTestLoad(){

		/*StreamReader r = File.OpenText(Application.dataPath+"/../AssetBundles/"+ "Test.xml");
		string _info = r.ReadToEnd(); 
		r.Close();*/

		XmlTextReader reader = new XmlTextReader(Application.dataPath+"/../AssetBundles/"+ "Test.xml");

		XmlDocument doc = new XmlDocument();
		doc.Load(reader);

		XmlNode node = doc.SelectSingleNode("Company/Department");//選擇節點
		if (node == null)
			return;
		XmlElement main = doc.CreateElement("newPerson"); //添加person節點
		main.SetAttribute("name", "小明");
		main.SetAttribute("sex", "女");
		main.SetAttribute("age", "25");
		node.AppendChild(main);
		XmlElement sub1 = doc.CreateElement("phone");
		sub1.InnerText = "123456778";
		main.AppendChild(sub1);
		XmlElement sub2 = doc.CreateElement("address");
		sub2.InnerText = "高雄";
		main.AppendChild(sub2);

		StreamWriter writer; 
		FileInfo t = new FileInfo(Application.dataPath+"/../AssetBundles/"+ "Test2.xml"); 
		if(!t.Exists) 
		{ 
			writer = t.CreateText(); 
		} 
		else 
		{ 
			t.Delete(); 
			writer = t.CreateText(); 
		} 

		writer.Write(doc.InnerXml);
		
		writer.Close(); 
		Debug.Log("File2 written."); 
	}

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
		List<string> files = new List<string>();
		getAssets(assets, names, files);

		//for(int i = 0; i < assets.Count; ++i) {
			//Debug.Log(string.Format("{0} : {1}", names[i], assets[i].name));
		//}

		/*long SizeCounter = 0;
		List<string> tempNames = new List<string>();
		List<UnityEngine.Object> tempAssets = new List<UnityEngine.Object>();
		int batch = 0;
		for(int index = 0; index < files.Count; index++)
		{
			FileInfo fileInfo = new FileInfo(files[index]);
			Debug.Log("the \"" + files[index] + " \"file size is: " + fileInfo.Length);

			tempAssets.Add(assets[index]);
			tempNames.Add(names[index]);
			SizeCounter += fileInfo.Length;


			if(SizeCounter >= 5000000){ //this is 5MB
				//give a package name
				string name = "docm_Resources"+batch.ToString();
				//edit xml
				//Do packing
				BuildPipeline.BuildAssetBundleExplicitAssetNames(tempAssets.ToArray(), tempNames.ToArray(), "AssetBundles/" + name, BuildAssetBundleOptions.CollectDependencies|BuildAssetBundleOptions.UncompressedAssetBundle, target);
				batch++;
			//reset totSize, clear temp assets and names;
			SizeCounter = 0;
			tempAssets.Clear();
			tempNames.Clear();
			}
		}

		Debug.Log("totSize is: " + SizeCounter.ToString());*/
		
		BuildPipeline.BuildAssetBundleExplicitAssetNames(assets.ToArray(), names.ToArray(), "AssetBundles/docm_Resources", BuildAssetBundleOptions.CollectDependencies|BuildAssetBundleOptions.UncompressedAssetBundle, target);
	}

	public static void getAssets(List<UnityEngine.Object> assets, List<string> names, List<string> files)
	{
		DirectoryInfo dinfo = new DirectoryInfo(Application.dataPath);
		foreach(DirectoryInfo child in dinfo.GetDirectories()) {
			if(child.Name[0] == '.') {
				continue;
			}
			
			if(child.Name == "Resources") {
				Uri newRoot = new Uri(child.FullName);
				getAssetsPath(newRoot, child, assets, names, files);
			} else {
				getAssetsPath(null, child, assets, names, files);
			}
		}
	}

	public static void getAssetsPath(Uri root, DirectoryInfo dinfo, List<UnityEngine.Object> assets, List<string> names, List<string> files)
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
				files.Add(file.FullName.ToString());
				Debug.Log("files: " + file.FullName.ToString());
				
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
				getAssetsPath(newRoot, child, assets, names,files);
			} else {
				getAssetsPath(root, child, assets, names, files);
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
