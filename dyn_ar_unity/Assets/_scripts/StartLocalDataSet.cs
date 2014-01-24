

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This behaviour allows to automatically load and activate one or more DataSet on startup
/// LOADS FROM PERSISTENTDATAPATH; where previously downloaded stuff is kept
/// </summary>
public class StartLocalDataSet : MonoBehaviour
{
    private List<string> mDataSetsToActivate ;
    private List<string> mDataSetsToLoad ;
	
	void Awake(){
		Debug.Log("awake StartLocalDataSet");	
	}
	
    void Start()
    {
		Debug.Log("starting StartLocalDataSet");
		mDataSetsToLoad = new List<string>();
		//mDataSetsToLoad.Add("stahre");
		/*
		mDataSetsToLoad.Add("dnfront");
		mDataSetsToLoad.Add("dnromer");*/
		mDataSetsToLoad.Add("malmbergs");
		mDataSetsToLoad.Add("estrid");

		mDataSetsToActivate = new List<string>();
		//mDataSetsToActivate.Add("stahre");
		/*
		mDataSetsToActivate.Add("dnfront");
		mDataSetsToActivate.Add("dnromer");*/
		mDataSetsToActivate.Add ("malmbergs");
		mDataSetsToActivate.Add ("estrid");
        if (!QCARRuntimeUtilities.IsQCAREnabled())
        {
			Debug.Log("bailout");
            return;
        }

        if (QCARRuntimeUtilities.IsPlayMode())
        {
            // initialize QCAR 
            QCARUnity.CheckInitializationError();
        }
	


        foreach (string dataSetName in mDataSetsToLoad)
        {
			string fullname=  Application.persistentDataPath+"/"+dataSetName+".xml";
            if (!DataSet.Exists(fullname,DataSet.StorageType.STORAGE_ABSOLUTE))
            {
                Debug.LogError("Data set " + fullname + " does not exist.");
                continue;
            }else{
				  Debug.Log("Data set " + fullname + " DOES  exist.");
			}


			ImageTracker imageTracker =(ImageTracker)TrackerManager.Instance.GetTracker<ImageTracker>(); //(ImageTracker)TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER);
            DataSet dataSet = imageTracker.CreateDataSet();

           if (!dataSet.Load(fullname,DataSet.StorageType.STORAGE_ABSOLUTE))
	        {
	            Debug.LogError("Failed to load data set " + fullname + ".");
	            return;
	        }else{
				  Debug.Log("loaded data set " + fullname );
				imageTracker.ActivateDataSet(dataSet);
				AttachContentToTrackables(dataSet);
			}
		
  
        }
    }

	 private void AttachContentToTrackables(DataSet dataSet)
 {
     // get all current TrackableBehaviours
     IEnumerable<TrackableBehaviour> trackableBehaviours = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
 
     // Loop over all TrackableBehaviours.
     foreach (TrackableBehaviour trackableBehaviour in trackableBehaviours)
     {
         // check if the Trackable of the current Behaviour is part of this dataset
         if (dataSet.Contains(trackableBehaviour.Trackable))
         {
             GameObject go = trackableBehaviour.gameObject;
 			 go.name=trackableBehaviour.Trackable.Name;
             // Add a Trackable event handler to the Trackable.
             // This Behaviour handles Trackable lost/found callbacks.
             go.AddComponent<DefaultTrackableEventHandler>();
 
             // Create a cube object.
             GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
 
             // Attach the cube to the Trackable and make sure it has a proper size.
             cube.transform.parent = trackableBehaviour.transform;
             cube.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
             cube.transform.localPosition = new Vector3(0.0f, 0.35f, 0.0f);
             cube.transform.localRotation = Quaternion.identity;
             cube.active = true;
             trackableBehaviour.gameObject.active = true;
         }
     }
 }

}



