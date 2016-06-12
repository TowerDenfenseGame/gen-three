using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretBehavior : MonoBehaviour {

    public List<GameObject> enemiesInRange;
    public GameObject[] enemyArray;
    float timeLeft = 1.00f;
    public List<Vector2> killZone = new List<Vector2>(); 


	// Use this for initialization
	void Start () {
        enemiesInRange = new List<GameObject>();
        //define killZone
        SetupKillZone();

    }

    void SetupKillZone() {
        string[] splitter = this.gameObject.name.Split(',');
        int x = int.Parse(splitter[0]);
        int y = int.Parse(splitter[1]);
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
	void Update () {
        
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            this.fire();
            timeLeft = 1.0f;
        }

    }

    void OnEnemyDestroyed(GameObject enemy) {
        enemiesInRange.Remove(enemy);
    }

    void fire() {
        bool quota = false;
        enemyArray = GameObject.FindGameObjectsWithTag("enemy");
        EnemyAStar enStar;
        foreach (GameObject enemy in enemyArray)
        {
            enStar = (EnemyAStar)enemy.GetComponent(typeof(EnemyAStar));
            Vector2 enPos = enStar.fetchGridPosition();
            if (!quota) { 
                foreach (Vector2 fireGrid in killZone) {
                    if (fireGrid == enPos) {
                        Destroy(enemy);
                        quota = true;
                    }
                }
            }
        }
        
    }
}
