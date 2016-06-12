using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretBehavior : MonoBehaviour {

    public List<GameObject> enemiesInRange;

	// Use this for initialization
	void Start () {
        enemiesInRange = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnEnemyDestroyed(GameObject enemy) {
        enemiesInRange.Remove(enemy);
    }
}
