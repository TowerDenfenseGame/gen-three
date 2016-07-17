using UnityEngine;
using System.Collections;

public class AlertScript : MonoBehaviour {

	float timeUp = 0.0f;
	string alert = " ";
	float startTime = 0.0f;
	public GUIStyle styleSheet;


	// Use this for initialization
	void Start () {
		styleSheet.normal.textColor = Color.red;
		styleSheet.fontSize = 12;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		float EndTime = startTime + timeUp;
		if (Time.time < EndTime) {
			GUI.Label (new Rect (185f, 10f, 100f, 100f), alert, styleSheet);
		}


	}

	public void AlertCall(string type){
		//types are hardcoded
		switch (type){
		case "NoFunds":				//no funds, up for 1 second
			alert = "Not Enough Funds";
			startTime = Time.time;
			timeUp = 1.0f;
			print ("Alert was called properly");
			break;
		case "":
			
		default:
			break;
		}

	}

}
