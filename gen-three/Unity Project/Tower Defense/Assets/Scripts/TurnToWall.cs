using UnityEngine;

public class TurnToWall : MonoBehaviour
{
    public GameManager Game;
    bool isWall;
	private GameObject temp;
	private GameManager gm;

	public float bombPrice = 50f;

    void Start()
    {
		temp = GameObject.FindGameObjectsWithTag("GameMgr")[0];
		gm = (GameManager)temp.GetComponent (typeof(GameManager));
    }

    void OnMouseDown()
	{
        switch (GameManager.buildType) //determines what will be built on mouse click
        {
		case "wall": //Costing 10 to build wall
			BuildWall ();
            break;
        case "turret": //costing 25 to build turret
            BuildTurret();
            break;
        case "bomb": // costing 50 to build bomb
            BuildBomb();
            break;
        }
    }

    private void BuildBomb()
    {
		GameObject tempAlert = GameObject.FindGameObjectsWithTag ("AlertSys") [0];
		AlertScript flag = (AlertScript)tempAlert.GetComponent (typeof(AlertScript));

		if (gm.credits - 50f >= 0) {
			string[] splitter = this.gameObject.name.Split (',');
			if (!isWall) {
				this.gameObject.AddComponent<BombBehavior> ();
				isWall = true;
				this.GetComponent<Renderer> ().material.color = Color.yellow;
				gm.credits = gm.credits - 50f;
			}
		} else {
			flag.AlertCall ("NoFunds");
			print ("No Funds Tried To Call");
		}
    }

    void BuildWall()
    {
		GameObject tempAlert = GameObject.FindGameObjectsWithTag ("AlertSys") [0];
		AlertScript flag = (AlertScript)tempAlert.GetComponent (typeof(AlertScript));

		if (gm.credits - 10f >= 0) {
			string[] splitter = this.gameObject.name.Split (',');
			if (!isWall) {
				Game.addWall (int.Parse (splitter [0]), int.Parse (splitter [1]));
				isWall = true;
				this.GetComponent<Renderer> ().material.color = Color.red;
				gm.credits = gm.credits - 10f;
			}
		}else {
			flag.AlertCall ("NoFunds");
			print ("No Funds Tried To Call");
		}
    }

    void BuildTurret()
    {
		GameObject tempAlert = GameObject.FindGameObjectsWithTag ("AlertSys") [0];
		AlertScript flag = (AlertScript)tempAlert.GetComponent (typeof(AlertScript));

		if (gm.credits - 25f >= 0) {
			string[] splitter = this.gameObject.name.Split (',');
			if (!isWall) {
				Game.addWall (int.Parse (splitter [0]), int.Parse (splitter [1]));
				isWall = true;
				this.GetComponent<Renderer> ().material.color = Color.green;
				this.gameObject.AddComponent<TurretBehavior> ();
				gm.credits = gm.credits - 25f;
			}
		}else {
			flag.AlertCall ("NoFunds");
			print ("No Funds Tried To Call");
		}
    }
    void removeTile()
    {
        string[] splitter = this.gameObject.name.Split(',');
        Game.removeWall(int.Parse(splitter[0]), int.Parse(splitter[1]));
        isWall = false;
        this.GetComponent<Renderer>().material.color = Color.white;
        Destroy(this.gameObject.GetComponent<TurretBehavior>());
        Destroy(this.gameObject.GetComponent<BombBehavior>());
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            removeTile();
        }
    }

    void Update()
    {
		//set frame timer

		//countdown frames up for warning

		//

    }
}
