using UnityEngine;
using System.Collections;
using System.IO;

public class ui : MonoBehaviour {

	private string[] values;

	// Use this for initialization
	IEnumerator Start () {

		guiText.text = "test";
		//values = File.ReadAllText("Assets/Resources/Localization.tsv").Split('	');
		guiText.text = values [4275];

		//StartCoroutine ("LoopingTheStr");

		yield return null;
	}
	
    IEnumerator LoopingTheStr () {

		for (int i = 0; i < values.Length; i++) {

			guiText.text = values[i];

			yield return new WaitForSeconds(.2f);

		}

		yield return null;
	}

	void OnGUI () {

		GUI.Label(new Rect(10, 10, 300, 40), "values[] length is: " + values.Length.ToString());
	}
}
