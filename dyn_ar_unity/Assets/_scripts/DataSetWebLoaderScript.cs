using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class DataSetWebLoaderScript : MonoBehaviour
{
	private string _webUrl = "http://www.fingerfunk.se/dynamic/StreamingAssets/";
	private string _webMarkerList="http://www.fingerfunk.se/dynamic/StreamingAssets/markerList.php";
	private bool _isDone;
	private WWW _currentwww;
	private bool _downloadStarted;


	private string _addchars;
	private int _numchars;
	private string _targetFilePath;

	private Dictionary<string,string> filemapDictionary;
	
	
	//TODO SETPREF ETC FOR FIGURING OUT WHAT WE HAVE ALREAD


	
	void Start ()
	{
		StartCoroutine(loadIndex());
	}
	

	#region helpers
		private void xx (string s)
	{
		Debug.Log (s);
	}
	
	public static bool fileIsDownloaded (string filename)
	{
		string fullpath = Path.Combine (Application.persistentDataPath, filename);
		if (File.Exists (fullpath))
			return true;
		else
			return false;
	}
	#endregion
	
	private IEnumerator checkAvailableMarkersOnServer(){

		WWW www = new WWW (_webMarkerList);
		yield return www;
		if (www.error != null) {
			Debug.Log ("wwww checkAvailableMarkersOnServer error " + www.error);
		} else {
			Debug.Log ("markerlist OK " + _webMarkerList + "  progress " + www.text);	
		}
	}
	
	//loading of all external files here
	private IEnumerator loadIndex ()
	{
		 	List<string> dList=new List<string>();
			//dList.Add("stahre");
		/*
			dList.Add("dnfront");
			dList.Add("dnromer");*/
			dList.Add("estrid");
			dList.Add("malmbergs");
		

		string[] extensions= {".xml",".dat"};
			
			foreach (string filename in dList) {
			foreach(string extension in extensions){
				string fullfilename=filename+extension;
				string uri = _webUrl + fullfilename;
				Debug.Log ("uri is "+uri);
				_targetFilePath = Path.Combine (Application.persistentDataPath, fullfilename);
				Debug.Log ("TRGETFILEPATH is " + _targetFilePath);
					
				if (!Directory.Exists (Path.GetDirectoryName (_targetFilePath))) {
					Debug.Log ("create directory " + Path.GetDirectoryName (_targetFilePath));
					Directory.CreateDirectory (Path.GetDirectoryName (_targetFilePath));
				} else {
					Debug.Log ("directory found " + Path.GetDirectoryName (_targetFilePath));
				}
			
				_currentwww = new WWW (uri);
				_downloadStarted=true;
				yield return _currentwww;
					
				if (_currentwww.error != null) {
					Debug.Log ("wwww error " + _currentwww.error+"  file "+filename);
					continue;
				} else {
					Debug.Log ("wwww OK " + uri + "  progress " + _currentwww.bytes.Length);	
					Debug.Log ("file exist " + File.Exists (_targetFilePath) + "  for " + _targetFilePath);
				}
					
		
				File.WriteAllBytes (_targetFilePath, _currentwww.bytes);
				while (!File.Exists(_targetFilePath)) {
					yield return new WaitForSeconds(0.1f);
				}
				Debug.Log ("file written " + _targetFilePath);
			}
			}
	
		
		Debug.Log ("End Load");
		Application.LoadLevel(1);
		
	}

	
	void OnDestroy ()
	{
		xx ("destroy");
	}
	#region debugguttons
	

	#endregion

	

}

