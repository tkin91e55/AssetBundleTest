using UnityEngine;
using System.Collections;

public class TestScene : MonoBehaviour {

	void OnGUI(){
		
		if(GUI.Button(new Rect(5,35,100,25) , "Load GameObject")){

#if UNITY_EDITOR || !UNITY_ANDROID
			StartCoroutine(LoadGameObject());
#else

#endif
		}
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
	}
}
