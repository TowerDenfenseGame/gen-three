using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MyPathNode[,] grid;
    public GameObject enemy;
    public GameObject gridBox;
    public int gridWidth;
    public int gridHeight;
    public Sprite carUp;
    public Sprite carDown;
    public Sprite carFront;
    public Sprite carBack;
    public float gridSize;
    public GUIStyle lblStyle;
    static public String buildType;
    public static string distanceType;
	public float credits;

    //This is what you need to show in the inspector.
    public static int distance = 0;
    private Component test;
    public List<Vector2> enemyPostions = new List<Vector2>();
    GameObject pullPositionFromThisGameObject;

    void Start()
    {
        //Generate a grid - nodes according to the specified size
        grid = new MyPathNode[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //Boolean isWall = ((y % 2) != 0) && (rnd.Next (0, 10) != 8);
                Boolean isWall = false;
                grid[x, y] = new MyPathNode()
                {
                    IsWall = isWall,
                    X = x,
                    Y = y,
                };
            }
        }
        //instantiate grid gameobjects to display on the scene
        createGrid();
        //instantiate enemy object
        createEnemy();
		print ("Started");
		credits = 100f;


    }

    void OnGUI()
    {

      /*  if (GUI.Button(new Rect(0f, 0f, 200f, 50f), "Create Enemy"))
        {
            createEnemy();
        }*/

		//Reload should be replace with a menu
        if (GUI.Button(new Rect(25f, 10f, 75f, 25f), "Reload"))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Main");
        }
		//TODO: Add Hotkeys for the build types

		if (GUI.Button(new Rect(25f, 40f, 75f, 25f), "Turret (Q)")) //button to make basic turrets
		{
			buildType = "turret";
		}
		if (GUI.Button(new Rect(105f, 40f, 75f, 25f), "Wall (W)")) //button to make walls
        {//this needs to go where we actually build the wall
				buildType = "wall";

        }
		if (GUI.Button(new Rect(185f, 40f, 75f, 25f), "Bomb (E)")) //button to make basic turrets
        {
            buildType = "bomb";
        }

        //GUI.Label(new Rect(5f, 120f, 100f, 100f), "Enemy Position:");
        /*if (pullPositionFromThisGameObject != null)
        {
            EnemyAStar enStar = (EnemyAStar)pullPositionFromThisGameObject.GetComponent(typeof(EnemyAStar));
            Vector2 enPos = enStar.fetchGridPosition();
            GUI.Label(new Rect(5f, 130f, 100f, 100f), enPos.ToString());
        }
        */
		GUI.Label(new Rect(105f, 10f, 100f, 100f), "Credits:");
		GUI.Label(new Rect(155f, 10f, 100f, 100f), "$"+ credits.ToString());
    }

    void createGrid()
    {
        //Generate Gameobjects of GridBox to show on the Screen
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                GameObject nobj = (GameObject)GameObject.Instantiate(gridBox);
                nobj.transform.position = new Vector2(gridBox.transform.position.x + (gridSize * j), gridBox.transform.position.y + (0.87f * i));
                nobj.name = j + "," + i;
                nobj.gameObject.transform.parent = gridBox.transform.parent;
                nobj.SetActive(true);
            }
        }
    }

    void createEnemy()
    {
        GameObject nb = (GameObject)GameObject.Instantiate(enemy);
        nb.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        /*
        pullPositionFromThisGameObject = GameObject.FindGameObjectWithTag("enemy");
        if (pullPositionFromThisGameObject != null)
        {
            EnemyAStar enStar = (EnemyAStar)pullPositionFromThisGameObject.GetComponent(typeof(EnemyAStar));
            Vector2 enPos = enStar.fetchGridPosition();
        }
        */
    }

    public void addWall(int x, int y)
    {
        grid[x, y].IsWall = true;
    }

    public void removeWall(int x, int y)
    {
        grid[x, y].IsWall = false;
    }
}
