
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class menuItems
{

		[MenuItem("Custom Editor/Create AssetBunldes")]
		static void ExecCreateAssetBunldes ()
		{
		
				// AssetBundle 的資料夾名稱及副檔名
				string targetDir = "_AssetBunldes";
				string extensionName = ".assetBunldes";
		
				//取得在 Project 視窗中選擇的資源(包含資料夾的子目錄中的資源)
				Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
		
				//建立存放 AssetBundle 的資料夾
				if (!Directory.Exists (targetDir))
						Directory.CreateDirectory (targetDir);
		
				foreach (Object obj in SelectedAsset) {
			
						//資源檔案路徑
						string sourcePath = AssetDatabase.GetAssetPath (obj);
			
						// AssetBundle 儲存檔案路徑
						string targetPath = targetDir + Path.DirectorySeparatorChar + obj.name + extensionName;
			
						if (File.Exists (targetPath))
								File.Delete (targetPath);
			
						//if(!(obj is GameObject) && !(obj is Texture2D) && !(obj is Material)) continue;
			
						//建立 AssetBundle
						#if ANDROID_BUNDLE_TEST
			if (BuildPipeline.BuildAssetBundle (obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies,BuildTarget.Android)) {
						#else
						if (BuildPipeline.BuildAssetBundle (obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies)) {
								#endif
								Debug.Log (obj.name + " succeeded");
				
						} else {
				
								Debug.Log (obj.name + " failed");
						}
				}
		}

		[MenuItem("Custom Editor/Create AssetBunldes Main")]
		static void CreateAssetBunldesMain ()
		{
				//获取在Project视图中选择的所有游戏对象
				Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
		
				//遍历所有的游戏对象
				foreach (Object obj in SelectedAsset) {
						string sourcePath = AssetDatabase.GetAssetPath (obj);
						//本地测试：建议最后将Assetbundle放在StreamingAssets文件夹下，如果没有就创建一个，因为移动平台下只能读取这个路径
						//StreamingAssets是只读路径，不能写入
						//服务器下载：就不需要放在这里，服务器上客户端用www类进行下载。
						string targetPath = Application.dataPath + "/StreamingAssets/" + obj.name + ".assetbundles";

						#if ANDROID_BUNDLE_TEST
			if (BuildPipeline.BuildAssetBundle (obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies,BuildTarget.Android)) {
#else
						if (BuildPipeline.BuildAssetBundle (obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies)) {
#endif
								Debug.Log (obj.name + "资源打包成功");
						} else {
								Debug.Log (obj.name + "资源打包失败");
						}
				}
				//刷新编辑器
				AssetDatabase.Refresh ();	
		
		}

}
