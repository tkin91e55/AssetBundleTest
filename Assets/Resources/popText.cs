using UnityEngine;
using System.Collections;

public class popText : MonoBehaviour {

	void OnGUI () {

		GUI.Label (new Rect (500, 200, 500, 200), "hello world!");
	}
}
