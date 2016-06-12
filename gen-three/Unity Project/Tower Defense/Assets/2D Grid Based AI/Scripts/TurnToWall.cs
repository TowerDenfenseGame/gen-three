using UnityEngine;
using System.Collections;

public class TurnToWall : MonoBehaviour
{

	public GameManager Game;
	// Use this for initialization
	void Start ()
	{
	

	}

	bool isWall;
    bool isTurret = false;

	void OnMouseDown ()
	{
		string[] splitter = this.gameObject.name.Split (',');
		if (!isWall) {
			Game.addWall (int.Parse (splitter [0]), int.Parse (splitter [1]));
			isWall = true;
			this.GetComponent<Renderer> ().material.color = Color.red;



		} else {
			Game.removeWall (int.Parse (splitter [0]), int.Parse (splitter [1]));
			isWall = false;
			this.GetComponent<Renderer> ().material.color = Color.white;
		}
		

	}

	void BuildTurret ()
	{
		string[] splitter = this.gameObject.name.Split (',');
		if (!isWall) {
			Game.addWall (int.Parse (splitter [0]), int.Parse (splitter [1]));
			isWall = true;
            isTurret = true;
			this.GetComponent<Renderer> ().material.color = Color.green;
            this.gameObject.AddComponent<TurretBehavior>();



		} else {
			Game.removeWall (int.Parse (splitter [0]), int.Parse (splitter [1]));
			isWall = false;
            isTurret = false;
            this.GetComponent<Renderer> ().material.color = Color.white;
            
		}
	}

	void OnMouseOver ()
	{
		if (Input.GetMouseButtonDown (1)) {
			BuildTurret ();
		}
	}


	// Update is called once per frame
	void Update ()
	{

	}

}
