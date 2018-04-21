using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersLoader : MonoBehaviour {

    public GameObject[] players;

    public List<Transform> spawnPositions;
    
    void Start () {
        players = new GameObject[4];
        int playerPosIndex = Random.Range(0, spawnPositions.Count);
        int enemyID = 1;

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if (i == playerPosIndex)
            {
                GameObject player = Instantiate(Resources.Load("Player")) as GameObject;
                player.transform.position = spawnPositions[i].position;
            }
            else
            {
                GameObject enemy = Instantiate(Resources.Load("Enemy")) as GameObject;
                enemy.transform.position = spawnPositions[i].position;
                enemy.GetComponent<PlayerMovementController>().participantID = enemyID;
                //Creates all actions for the enemy
                enemy.GetComponent<PlayerMovementController>().GenerateActions();
                enemyID += 1;
            }
        }
    }
	
	void Update () {
		
	}
}
