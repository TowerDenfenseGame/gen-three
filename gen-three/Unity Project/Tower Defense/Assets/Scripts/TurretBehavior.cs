using UnityEngine;
using System.Collections.Generic;

public class TurretBehavior : MonoBehaviour
{
    public List<GameObject> enemiesInRange;
    public GameObject[] enemyArray;
    float timeLeft = 1.00f;
    public List<Vector2> killZone = new List<Vector2>();
    public GameObject target;
    public bool hasTarget;
    public LineRenderer line;
    public Vector3[] positions = new Vector3[2];

    // Use this for initialization
    void Start()
    {
        enemiesInRange = new List<GameObject>();
        //define killZone
        SetupKillZone();
        hasTarget = false;
        this.gameObject.AddComponent<LineRenderer>();
        line = this.gameObject.GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Particles/Additive"));
        line.SetColors(new Color(256f, 256f, 256f), new Color(256f, 256f, 256f));
        positions[0] = this.transform.position;
        line.SetWidth(0.25f, 0.25f);
    }

    void SetupKillZone()
    {
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

    void Update()
    {
        //check for enemies in range.
        //find Target
        if (!hasTarget)
        {
			
            positions[1] = positions[0];
            line.SetPositions(positions);
            //enumerate enemies
            enemyArray = GameObject.FindGameObjectsWithTag("enemy");
            //find a target
            EnemyAStar enStar;
            foreach (GameObject enemy in enemyArray) //check everybody in array
            {
                enStar = (EnemyAStar)enemy.GetComponent(typeof(EnemyAStar));
                Vector2 enPos = enStar.fetchGridPosition();
                foreach (Vector2 fireGrid in killZone)
                { //are they within range
                    if (fireGrid == enPos && !hasTarget)
                    { //seek for one that is within firegrid
                        target = enemy; //set as enemy
                        hasTarget = true; // we have a target now
                    }
                }
            }
            //fire until out of range or dead
            //find new target
        }
        else
        {
            //render targeting line
            EnemyAStar enstar;
            bool inRange = false;
            bool isAlive = false;
            if (target != null)
            {
                isAlive = true;
            }

            foreach (Vector2 fireGrid in killZone) //check to make sure target is still in range
            {
                if (target != null)
                {
                    enstar = (EnemyAStar)target.GetComponent(typeof(EnemyAStar));
                    if (enstar.fetchGridPosition().Equals(fireGrid))
                    {
                        positions[1] = target.transform.position;
                        line.SetPositions(positions);
                        inRange = true;
                    }
                }
            }

            if (!inRange || !isAlive)
            {
                hasTarget = false;
            }
        }

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            this.fire();
            timeLeft = 1.0f;
        }

		if (hasTarget == true) {
			Vector3 direction = gameObject.transform.position - target.transform.position;
			gameObject.transform.rotation = Quaternion.AngleAxis ((Mathf.Atan2 (direction.y, direction.x) * 180 / Mathf.PI), new Vector3 (0, 0, 1));
		} else {
			this.GetComponent<SpriteRenderer> ().transform.localRotation = Quaternion.identity;
		}
    }

    void OnEnemyDestroyed(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    void fire()
    {
        bool quota = false;
        enemyArray = GameObject.FindGameObjectsWithTag("enemy");
        EnemyAStar enStar;
        foreach (GameObject enemy in enemyArray)
        {
            enStar = (EnemyAStar)enemy.GetComponent(typeof(EnemyAStar));
            Vector2 enPos = enStar.fetchGridPosition();
            if (!quota)
            {
                foreach (Vector2 fireGrid in killZone)
                {
					if (fireGrid == enPos)
					{
						quota = true;
						//temporarily we will just reset the hasTarget bool when fire occurs, but later we need to do checks agains enemy health
						EnemyHealth targetHP = (EnemyHealth)enemy.GetComponent(typeof(EnemyHealth));
						float currentHP = targetHP.blast(10);
						if (currentHP <= 0)
						{
							hasTarget = false;
							Destroy(enemy);
							GameObject temp = GameObject.FindGameObjectsWithTag("GameMgr")[0];
							GameManager gm = (GameManager)temp.GetComponent (typeof(GameManager));
							gm.credits += 25;
                            SumScore.Add(5);
						}
					}
                }
            }
        }
    }
}