using UnityEngine;

public class TurnToWall : MonoBehaviour
{
    public GameManager Game;
    bool isWall;

    void Start()
    {
    }

    void OnMouseDown()
    {
        switch (GameManager.buildType) //determines what will be built on mouse click
        {
            case "wall":
                BuildWall();
                break;
            case "turret":
                BuildTurret();
                break;
            case "bomb":
                BuildBomb();
                break;
        }
    }

    private void BuildBomb()
    {
        string[] splitter = this.gameObject.name.Split(',');
        if (!isWall)
        {
            this.gameObject.AddComponent<BombBehavior>();
            isWall = true;
            this.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    void BuildWall()
    {
        string[] splitter = this.gameObject.name.Split(',');
        if (!isWall)
        {
            Game.addWall(int.Parse(splitter[0]), int.Parse(splitter[1]));
            isWall = true;
            this.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    void BuildTurret()
    {
        string[] splitter = this.gameObject.name.Split(',');
        if (!isWall)
        {
            Game.addWall(int.Parse(splitter[0]), int.Parse(splitter[1]));
            isWall = true;
            this.GetComponent<Renderer>().material.color = Color.green;
            this.gameObject.AddComponent<TurretBehavior>();
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
    }
}
