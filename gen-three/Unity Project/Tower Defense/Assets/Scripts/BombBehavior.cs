using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombBehavior : MonoBehaviour
{

    public List<GameObject> enemiesInRange;
    public GameObject[] enemyArray;
    public List<Vector2> killZone = new List<Vector2>();
    public Vector2 triggerZone;
    public GameObject target;
    bool steppedON;
    bool blewup;
    // Use this for initialization
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

    // Update is called once per frame
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
                steppedON = true;
            }
            if (steppedON == true && enPos != triggerZone)
            {
                foreach (Vector2 fireGrid in killZone)
                { //are they within range
                    if (fireGrid == enPos)
                    {
                        Destroy(enemy);
                        blewup = true;
                    }
                }
            }
        }
        if (blewup == true)
        {
            Destroy(this);
        }
    }
}