using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public EnemyHealth FullHealth;       		// Reference to the player's heatlh.
	public GameObject enemy ;               	// The enemy prefab to be spawned.
	public float spawnTime = 7f;           		// How long between each spawn.
	public Transform[] spawnPoints;         	// An array of the spawn points this enemy can spawn from.
	public float enemyPower;
	public float difficulty = 60f; 				//lower numbers are harder
	public float lastSpawn;
	public float nextSpawn;


	void Start ()
	{
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		//InvokeRepeating ("Spawn", spawnTime, spawnTime);
		lastSpawn = Time.time;
		nextSpawn = lastSpawn + spawnTime;
	}


	void Spawn (GameObject spawnMe)
	{
		lastSpawn = Time.time;
		nextSpawn = lastSpawn + spawnTime;
		GameObject nb = (GameObject)GameObject.Instantiate (spawnMe);
		nb.SetActive (true);

		// Find a random index between zero and one less than the number of spawn points.
		//int spawnPointIndex = Random.Range (0, spawnPoints.Length);

		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		//Instantiate (spawnMe, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
	}

	void Update()
	{
		if(Time.time > nextSpawn){
			enemyPower = (Time.time + 60f)/difficulty;
			GameObject upgraded = EnemyPowerAdjustment ();
			do {
				Spawn (upgraded);
				enemyPower--;
			} while(enemyPower >= 1);
		}


	}

	GameObject EnemyPowerAdjustment(){
		GameObject upgraded = enemy;
		EnemyAStar eStar = (EnemyAStar)upgraded.GetComponent (typeof(EnemyAStar));
		EnemyHealth eHealth = (EnemyHealth)upgraded.GetComponent (typeof(EnemyHealth));
		eStar.moveSpeed = (float) 1.00f * (1.00f + (enemyPower / 3));
		eHealth.UnitHealth = 45 * (1.00f + (enemyPower / 5));
		eHealth.FullHealth = 45 * (1.00f + (enemyPower / 5));

		return upgraded;
	}
}