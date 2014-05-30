using UnityEngine;
using System.Collections;
using System.IO;
using LumenWorks.Framework.IO.Csv;


//to be used for streaming scene only
public class StreamTest : MonoBehaviour {

	public TextAsset localization;

	public Stream GenerateStreamFromString(string s)
	{
		MemoryStream stream = new MemoryStream();
		StreamWriter writer = new StreamWriter(stream);
		writer.Write(s);
		writer.Flush();
		stream.Position = 0;
		return stream;
	}

	// Use this for initialization
	void Start () {

		using (Stream s = GenerateStreamFromString(localization.text))
		{
			TextReader tr = new StreamReader(s);
			CsvReader csv;
			csv = new CsvReader (tr, true);
			int fieldCount = csv.FieldCount;
			while (csv.ReadNextRecord()) {
				
				Debug.Log(csv[3]);
			}
		}
	
	}

}
