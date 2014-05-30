using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LumenWorks.Framework.IO.Csv;

//reference: http://www.codeproject.com/Articles/9258/A-Fast-CSV-Reader


public class lumen : MonoBehaviour
{

		private CsvReader csv;
		private static Dictionary<string,LocalizeData> m_TextDictionary;
		public TextAsset localizationText;

		public Stream GenerateStreamFromString (string s)
		{
				MemoryStream stream = new MemoryStream ();
				StreamWriter writer = new StreamWriter (stream);
				writer.Write (s);
				writer.Flush ();
				stream.Position = 0;
				return stream;
		}

		// Use this for initialization
		void Start ()
		{	
				StartCoroutine ("ReadCsv");
		}

		IEnumerator ReadCsv () //this is intialization
		{
				// open the file "data.csv" which is a CSV file with headers

				using (Stream s = GenerateStreamFromString(localizationText.text)) {

						TextReader tr = new StreamReader (s);
						CsvReader csv;
						csv = new CsvReader (tr, true);
						//csv = new CsvReader (new StreamReader ("Assets/Resources/Localization.csv"), true);
						m_TextDictionary = new Dictionary<string,LocalizeData> ();
						int fieldCount = csv.FieldCount;
						//Debug.Log (fieldCount);
						//string[] headers = csv.GetFieldHeaders ();

						while (csv.ReadNextRecord()) {
						
								if (csv [0] == "1")
										m_TextDictionary.Add (csv [1], new LocalizeData (true, csv [2], csv [fieldCount - 1]));
								else
										m_TextDictionary.Add (csv [1], new LocalizeData (false, csv [2], csv [fieldCount - 1]));

								guiText.text = csv [fieldCount - 1];
								Debug.Log (csv [fieldCount - 1]);
								yield return new WaitForSeconds (.5f);
						}
				}

				yield return null;
#if false
			while (csv.ReadNextRecord())
			{
				for (int i = 0; i < fieldCount; i++)
					Debug.Log(string.Format("Item no.: {0}, {1} = {2};",counter, headers[i], csv[i]));
				counter++;
					//Debug.Log(headers[i]);
			}
#endif
		}

		void OnGUI () {

		GUI.Label (new Rect (200, 200, 500, 100), guiText.text);
	}

		public static string SearchCsvKey (string key)
		{

				if (m_TextDictionary.ContainsKey (key))
						return m_TextDictionary [key].LangSelection (LocalizeData.Lang.ZH);
				else {
						Debug.LogError ("a wrong key!!?");
						return "";
				}
		}

		public static string SearchCsvKey (string key, LocalizeData.Lang Lang)
		{

				if (m_TextDictionary.ContainsKey (key))
						return m_TextDictionary [key].LangSelection (Lang);
				else {
						Debug.LogError ("a wrong key!!?");
						return "";
				}
		}
}

public class LocalizeData
{

		private bool m_Enabled;
		private string m_JPText;
		private string m_ZHText;

		public enum Lang
		{
				JP = 0,
				ZH = 1
		}

		public LocalizeData (bool a_switch, string a_JPText, string a_ChiText)
		{
				m_Enabled = a_switch;
				m_JPText = a_JPText;
				m_ZHText = a_ChiText;	
		}

		public void SwitchTrue ()
		{
				m_Enabled = true;
		}

		public void SwitchFalse ()
		{
				m_Enabled = false;
		}

		public string LangSelection (Lang aLang)
		{
				switch (aLang) {
				case Lang.JP:
						return m_JPText;
				case Lang.ZH:
						return m_ZHText;
				default:
						return m_JPText;
				}
		}
}

