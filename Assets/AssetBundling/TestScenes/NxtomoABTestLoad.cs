using UnityEngine;
using System.Collections;

public class NxtomoABTestLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	string prompt = "";
	string prompt2 = "";

	void OnGUI () {

		if(GUI.Button(new Rect(0,15,150,150), "loadBundle")){
			StartCoroutine(LoadAB());
		}
		GUI.TextArea(new Rect(0,165,150,150), Application.dataPath);
#if !UNITY_EDITOR && UNITY_IPHONE
		GUI.TextArea(new Rect(0,315,150,150), Application.persistentDataPath + " ,, " + prompt);
		GUI.TextArea(new Rect(0,465,150,150), Application.persistentDataPath + " ,, " + prompt2);
#endif
	}

	IEnumerator LoadAB() {

#if UNITY_EDITOR
		AssetBundle resource = AssetBundle.CreateFromFile(Application.dataPath+ "/../AssetBundles/" + "docm_Resources");


		WWW res = new WWW(Application.dataPath+ "/../AssetBundles/" + "docm_Resources");

		#elif !UNITY_EDITOR && UNITY_IPHONE

		AssetBundle resource = AssetBundle.CreateFromFile(Application.persistentDataPath  + "/docm_Resources");
		
		
		WWW res = new WWW(Application.dataPath+ "/docm_Resources");
#endif

		yield return res;

		yield return resource;

		if (resource != null) {
						Debug.Log ("the resource is not null");
			prompt = "the resource is not null";
				} else {
						Debug.Log ("resource is null");
			prompt = "the resource is null";
				}

		if(res.error!=null)
			Debug.Log("res.error: " + res.error);
		else
			Debug.Log("no error");


		//Object ticket = resource.Load("Item/info.mIconTexName:TICKET_LE_OYA");

		//Object ticket = (Object)resource.Load("Item/info.mIconTexName: TICKET_LE_OYA",typeof(Object));

		Object ticket = (Object)resource.Load("Item/TICKET_LE_OYA",typeof(Object));

		Object ticket2 = Resources.Load("Item/TICKET_LE_OYA");

		if (ticket == null) {
						Debug.Log ("ticket is null");
						prompt2 = "ticket is null";
				} else {
						Debug.Log ("ticket is not null");
			prompt2 = "ticket is not null";
				}

		if (ticket2 == null)
			Debug.Log("ticket2 is null");
		else 
			Debug.Log("ticket2 is not null");

		guiTexture.texture = (Texture) ticket;
	}
}
