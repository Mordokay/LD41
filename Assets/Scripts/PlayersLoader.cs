using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersLoader : MonoBehaviour {

    public GameObject[] players;
    public Transform spawnPositions;
    
    void Start () {
        players = new GameObject[4];
        int enemyID = 1;

        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                GameObject player = Instantiate(Resources.Load("Player")) as GameObject;
                player.transform.position = spawnPositions.position;
                player.GetComponent<PlayerMovementController>().isPlayerControlled = true;
                players[i] = player;
                player.name = PlayerPrefs.GetString("nickname");
                //player.GetComponent<PlayerMovementController>().HideArrows();
                //Since it is the player playing again we roll the dice to decide how many plays he is going to do.
                int random = Random.Range(2, 12);
                player.GetComponent<PlayerMovementController>().playerRemainingMoves = random;
            }
            else
            {
                GameObject enemy = Instantiate(Resources.Load("Player")) as GameObject;
                enemy.transform.position = spawnPositions.position;
                enemy.GetComponent<PlayerMovementController>().participantID = enemyID;
                enemy.GetComponent<PlayerMovementController>().isPlayerControlled = false;
                enemy.tag = "Enemy";
                players[i] = enemy;

                enemy.name = "Enemy#" + enemyID;
                enemyID += 1;
            }
        }

        StartCoroutine(addActionsToEnemies());
    }


    IEnumerator addActionsToEnemies()
    {
        yield return StartCoroutine(this.GetComponent<GameRecorder>().GetAllTables());

        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].GetComponent<PlayerMovementController>().isPlayerControlled)
            {
                int random = Random.Range(0, this.GetComponent<GameRecorder>().tableNames.Count);
                string tableName = this.GetComponent<GameRecorder>().tableNames[random];
                this.GetComponent<GameRecorder>().tableNames.RemoveAt(random);
                yield return StartCoroutine(this.GetComponent<GameRecorder>().GetTable(tableName));
                players[i].GetComponent<PlayerMovementController>().actionList = this.GetComponent<GameRecorder>().tableActions;
                this.GetComponent<GameRecorder>().tableActions = new List<PlayerMovementController.Action>();

                players[i].GetComponent<PlayerMovementController>().startStage = (int)players[i].GetComponent<PlayerMovementController>().actionList[0].value;
                players[i].GetComponent<PlayerMovementController>().actionList.RemoveAt(0);

                players[i].name = tableName;
            }
        }
    }
}
