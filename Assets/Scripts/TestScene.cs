using UnityEngine;
using System.Collections;

public class TestScene : MonoBehaviour {

	string btnName = "Load GO";

	void OnGUI(){
		
		if(GUI.Button(new Rect(5,35,100,25) , btnName)){


#if UNITY_EDITOR && !UNITY_ANDROID
			StartCoroutine(LoadGameObject());
#elif UNITY_ANDROID && !UNITY_EDITOR 
			StartCoroutine(AndroidLoadGameObject());
#endif
		}
		GUI.Label(new Rect(100,100,500,50),"The data path is: " + Application.dataPath);
	}
	
	public static string PathURL;

	void Awake() {

		PathURL =
			#if UNITY_ANDROID
			"jar:file://" + Application.dataPath + "!/assets/";
		#elif UNITY_IPHONE
		Application.dataPath + "/Raw/";
		#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
		"file://" + Application.dataPath + "/StreamingAssets/";
		#else
		string.Empty;
#endif
	}

	private IEnumerator LoadGameObject(){
		
		// AssetBundle 檔案路徑
		string path = string.Format("file://{0}/../_AssetBunldes/{1}.assetBunldes" , Application.dataPath , "popText");
		
		//  載入 AssetBundle
		WWW bundle = new WWW(path);
		
		//等待載入完成
		yield return bundle;
		
		//實例化 GameObject 並等待實作完成
		yield return Instantiate(bundle.assetBundle.mainAsset);
		
		//卸載 AssetBundle
		bundle.assetBundle.Unload(false);

		string path2 = string.Format("file://{0}/../_AssetBunldes/{1}.assetBunldes" , Application.dataPath , "lumen");
		WWW bundle2 = new WWW (path2);
		yield return bundle2;
		GameObject TextGO = (GameObject) Instantiate(bundle2.assetBundle.mainAsset);
		bundle2.assetBundle.Unload(false);
		//btnName = TextGO.GetComponent<TASrc> ().test.text; 
	}

	private IEnumerator AndroidLoadGameObject () {

		Debug.Log ("AndroidLoadGameObject()called");
		string path = string.Format(PathURL+ "lumen.assetbundles");
		WWW bundle2 = WWW.LoadFromCacheOrDownload (path,5);
		yield return bundle2;
		GameObject TextGO = (GameObject) Instantiate(bundle2.assetBundle.mainAsset);
		bundle2.assetBundle.Unload(false);

	}
}
