using UnityEngine;
using System.Collections.Generic;

public class BombBehavior : MonoBehaviour
{
    public List<GameObject> enemiesInRange;
    public GameObject[] enemyArray;
    public List<Vector2> killZone = new List<Vector2>();
    public Vector2 triggerZone;
    public GameObject target;
    bool blewup;

    void Start()
    {
        enemiesInRange = new List<GameObject>();
        SetupKillZone();
    }

    void SetupKillZone()
    {
        string[] splitter = this.gameObject.name.Split(',');
        int x = int.Parse(splitter[0]);
        int y = int.Parse(splitter[1]);
        triggerZone = new Vector2(x, y);
        killZone.Add(new Vector2(x, y));
        killZone.Add(new Vector2(x, y + 1));
        killZone.Add(new Vector2(x, y - 1));
        killZone.Add(new Vector2(x + 1, y));
        killZone.Add(new Vector2(x + 1, y + 1));
        killZone.Add(new Vector2(x + 1, y - 1));
        killZone.Add(new Vector2(x - 1, y));
        killZone.Add(new Vector2(x - 1, y + 1));
        killZone.Add(new Vector2(x - 1, y - 1));
    }

    void Update()
    {
        enemyArray = GameObject.FindGameObjectsWithTag("enemy");
        //find a target
        EnemyAStar enStar;
        foreach (GameObject enemy in enemyArray) //check everybody in array
        {
            enStar = (EnemyAStar)enemy.GetComponent(typeof(EnemyAStar));
            Vector2 enPos = enStar.fetchGridPosition();
            if (enPos == triggerZone)
            {
                foreach (Vector2 fireGrid in killZone)
                { //are they within range
                    if (fireGrid == enPos)
                    {
                        Destroy(enemy);
                        blewup = true;
                        SumScore.Add(5);
                    }
                }
            }
        }

        if (blewup == true)
        {
            //this.GetComponent<Renderer>().material.color = Color.white;
			this.GetComponent<TurnToWall> ().removeTile ();
            //Destroy(this);
        }
    }
}