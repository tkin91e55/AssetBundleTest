using UnityEngine;
using System.Collections;

public class RunScript : MonoBehaviour
{
	



	    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
	    public static readonly string PathURL =
#if UNITY_ANDROID
		"jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
		Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	"file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;
#endif


	public static readonly string PersistentPathURL =
		#if UNITY_ANDROID
		Application.persistentDataPath + "/";
	#elif UNITY_IPHONE
	Application.persistentDataPath + "/";
	#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	"file://" + Application.persistentDataPath + "/";
	#else
	string.Empty;
	#endif

	string androidExtPath;

	void Start() {

		AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment"); 
		androidExtPath = jc.CallStatic<AndroidJavaObject> ("getExternalStorageDirectory").Call<string> ("getPath");
		Debug.Log(androidExtPath);
		Debug.Log ("ajc called");

		}
	
	void OnGUI()
	{
		GUI.TextArea(new Rect(0,Screen.height/3,200,100),"persistent path: " + Application.persistentDataPath);

		if(GUILayout.Button("Main Assetbundle"))
		{
			//StartCoroutine(LoadMainGameObject(PathURL + "Prefab0.assetbundle"));
			//StartCoroutine(LoadMainGameObject(PathURL +  "Prefab1.assetbundle"));
		


			#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine(LoadMainCacheGameObject(PersistentPathURL + "Prefab0.assetbundle"));
			StartCoroutine(LoadMainCacheGameObject(PersistentPathURL +  "Prefab1.assetbundle"));
#else
			StartCoroutine(LoadMainCacheGameObject(PathURL + "Prefab0.assetbundle"));
			StartCoroutine(LoadMainCacheGameObject(PathURL + "Prefab1.assetbundle"));
			#endif
		}
		
		if(GUILayout.Button("ALL Assetbundle"))
		{
			StartCoroutine(LoadALLGameObject(PathURL + "ALL.assetbundle"));
		}
		
		if(GUILayout.Button("Open Scene"))
		{
			StartCoroutine(LoadScene());
		}
		if(GUILayout.Button("test to load"))
			#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine(TestLoading());
#else
		Debug.Log("");
			#endif
		
	}

	private IEnumerator TestLoading() {

		string path1 = Application.persistentDataPath + "/Prefab0.assetbundle" ;
		string path2 = "/storage/emulated/0/Android/" + "Prefab0.assetbundle";

		//Not-Working
		////////////////////////////////////////////////////////////////////////////

		WWW bundle = new WWW (path1);
		yield return bundle;
		if (bundle.error != null)
			Debug.Log ("Nxtomo WWW not working Error: " + bundle.error + ", Path: " + path1);

		//Not-Working
		////////////////////////////////////////////////////////////////////////////

		WWW bundle2 = new WWW (path2);
		yield return bundle2;
		if (bundle2.error != null)
			Debug.Log ("Nxtomo WWW (2) not working Error: " + bundle2.error + ", Path: " + path2);

		//Working
		////////////////////////////////////////////////////////////////////////////

		AssetBundle bundle3 = AssetBundle.CreateFromFile (path1);
		if (bundle3 != null)
			Debug.Log ("bundle3 is not null");
		else
			Debug.Log ("bundle3 is null");
		yield return Instantiate (bundle3.mainAsset);
		bundle3.Unload (false);

		//Working
		////////////////////////////////////////////////////////////////////////////

		AssetBundle bundle4 = AssetBundle.CreateFromFile (path2);
		if(bundle4 != null)
			Debug.Log ("bundle4 is not null");
		else
			Debug.Log ("bundle4 is null");
		yield return Instantiate (bundle4.mainAsset);
		bundle4.Unload (false);

		//Seems Working
		////////////////////////////////////////////////////////////////////////////

		WWW bundle5 = WWW.LoadFromCacheOrDownload ("file://"+path1,0);
		yield return bundle5;
		if (bundle5.error != null)
						Debug.Log ("Nxtomo WWW not working Error: " + bundle5.error + ", Path: " + path1);
				else if (bundle5.assetBundle != null) {
						Debug.Log ("www 5 seems ok");
						bundle5.assetBundle.Unload (false);
				}

		//Seems Working
		////////////////////////////////////////////////////////////////////////////

		WWW bundle6 = WWW.LoadFromCacheOrDownload (path2,0);
		yield return bundle6;
		if (bundle6.error != null)
			Debug.Log ("Nxtomo WWW not working Error: " + bundle6.error + ", Path: " + path2);
		else if (bundle6.assetBundle != null) {
			Debug.Log ("www 6 seems ok");
			bundle6.assetBundle.Unload (false);
		}
	

		}
	
	//读取一个资源
	
	private IEnumerator LoadMainGameObject(string path)
	{
		 WWW bundle = new WWW(path);
		 
		 yield return bundle;

		if (bundle.error != null)
						Debug.Log ("Nxtomo Error: " + bundle.error);
		 
		 //加载到游戏中
		 yield return Instantiate(bundle.assetBundle.mainAsset);
		 
		 bundle.assetBundle.Unload(false);
	}
	
	//读取全部资源
	
	private IEnumerator LoadALLGameObject(string path)
	{
		 WWW bundle = new WWW(path);
		 
		 yield return bundle;
		 
		 //通过Prefab的名称把他们都读取出来
		 Object  obj0 =  bundle.assetBundle.Load("Prefab0");
		 Object  obj1 =  bundle.assetBundle.Load("Prefab1");
		
		 //加载到游戏中	
		 yield return Instantiate(obj0);
		 yield return Instantiate(obj1);
		 bundle.assetBundle.Unload(false);
	}

	string prefab0 = "Prefab0.assetbundle";
	string prefab1 = "Prefab1.assetbundle";

	private IEnumerator LoadMainCacheGameObject(string path)
	{
		 WWW bundle = WWW.LoadFromCacheOrDownload(path,5);
		 
		 yield return bundle;

		if (bundle.error != null)
						Debug.Log ("Nxtomo WWW not working Error: " + bundle.error + ", Path: " + path);

		 
		yield return Instantiate(bundle.assetBundle.mainAsset);

		 
		 bundle.assetBundle.Unload(false);

	}
	
	
	private IEnumerator LoadScene()
	{
		 WWW download = WWW.LoadFromCacheOrDownload ("file://"+Application.dataPath + "/MyScene.unity3d", 1);
		  yield return download;
		  var bundle = download.assetBundle;
  		  Application.LoadLevel ("Level");
	}
	
}
