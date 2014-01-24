	// C# Example
	// Builds an asset bundle from the selected objects in the project view.
	// Once compiled go to "Menu" -> "Assets" and select one of the choices
	// to build the Asset Bundle
	
	using UnityEngine;
	using UnityEditor;
	public class ExportAssetBundles {

		[MenuItem("Assets/Build Android AssetBundle From Selection - Track dependencies")]
		static void ExportResourceAndroid () {
			// Bring up save panel
			string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
		Debug.Log("path is "+path);
			if (path.Length != 0) {
				// Build the resource file from the active selection.
				Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			//both android and ios here
			
				BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, 
                                  BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.Android
				);
			//
				Selection.objects = selection;
			}
		}
	[MenuItem("Assets/Build iOS AssetBundle From Selection - Track dependencies")]
	static void ExportResourceiOS () {
		// Bring up save panel
		string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
		if (path.Length != 0) {
			// Build the resource file from the active selection.
			Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			//both android and ios here
			
			BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, 
			                               BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.iPhone
			                               );
			//
			Selection.objects = selection;
		}
	}


		[MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking")]
		static void ExportResourceNoTrack () {
			// Bring up save panel
			string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
			if (path.Length != 0) {
				// Build the resource file from the active selection.
				BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path);
			}
		}
	}