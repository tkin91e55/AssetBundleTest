using UnityEngine;
using System.Collections;

public class TASrc : MonoBehaviour {

	public TextAsset test;
	public TextAsset localization;

	void OnGUI () {

		GUI.Label (new Rect (100, 100, 500, 200), "unknown");
	}
}
