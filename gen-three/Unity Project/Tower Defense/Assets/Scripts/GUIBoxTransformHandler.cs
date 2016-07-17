using UnityEngine;
using System.Collections;

public class GUIBoxTransformHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
		this.transform.position = worldPoint;
		Vector3 Unadjusted = this.transform.position;
		Vector3 Adjusted = new Vector3(Unadjusted.x, (Unadjusted.y * -1.0f ), 0);
		this.transform.position = Adjusted;
	}

	// Update is called once per frame
	void Update () {
	

	}
}
