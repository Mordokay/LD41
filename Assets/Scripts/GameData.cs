using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public int currentPlayerPlayingId;
    public int maxPlayers;
    public GameObject player;
    public bool playerWon;

    private float stageRemainingTime;
    bool playingStage;

    public bool stillAtStart;

    public List<PlayerMovementController.Action> playerActions;

    public int currentStage;

    public List<GameObject> patrolObjects;

    public GameObject endTurnButton;

    void Start () {
        stillAtStart = true;

        //Time.timeScale = 0.2f;

        playingStage = false;
        //Randomly choose who starts playing
        currentPlayerPlayingId = Random.Range(0, maxPlayers);
        //currentPlayerPlayingId = 0;

        player = GameObject.FindGameObjectWithTag("Player");
        playerActions = new List<PlayerMovementController.Action>();

        currentStage = 0;
    }

    public void PlayStage()
    {
        if (!playingStage)
        {
            playingStage = true;
            //Plays one stage of every dynamic object
            stageRemainingTime = 0.5f;
            foreach (GameObject obj in patrolObjects)
            {
                obj.GetComponent<Patrol>().PlayNext(currentStage);
            }
        }
    }

    public void NextParticipant()
    {
        currentPlayerPlayingId += 1;
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (currentPlayerPlayingId == maxPlayers)
        {
            player.GetComponent<PlayerMovementController>().remainingTurnsText.enabled = true;
            currentPlayerPlayingId = 0;
            player.GetComponent<PlayerMovementController>().RefreshArrows();
            //Since it is the player playing again we roll the dice to decide how many plays he is going to do.
            int random = Random.Range(2, 12);
            player.GetComponent<PlayerMovementController>().playerRemainingMoves = random;
            player.GetComponent<PlayerMovementController>().remainingTurnsText.text = "Remaining Text: " + random;

            player.GetComponent<PlayerMovementController>().lastSafePos = player.transform.position;
            endTurnButton.SetActive(true);
            
            //Add the turn stage in the begining 
            playerActions.Add(new PlayerMovementController.Action("t", currentStage));
        }
        else
        {
            player.GetComponent<PlayerMovementController>().remainingTurnsText.enabled = false;
            endTurnButton.SetActive(false);
            player.GetComponent<PlayerMovementController>().HideArrows();
        }
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else if(stillAtStart)
        {
            if (currentPlayerPlayingId == 0 && player != null)
            {
                player.GetComponent<PlayerMovementController>().RefreshArrows();

                //Add the turn stage in the begining 
                playerActions.Add(new PlayerMovementController.Action("t", currentStage));
            }
            else
            {
                player.GetComponent<PlayerMovementController>().HideArrows();
            }
            
            stillAtStart = false;
        }

        if (playingStage)
        {
            stageRemainingTime -= Time.deltaTime;
            if(stageRemainingTime <= 0.0f)
            {
                playingStage = false;
                currentStage += 1;
                if(currentStage > 3)
                {
                    currentStage = 0;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(runActions());
        }
    }

    IEnumerator runActions()
    {
        int random = Random.Range(0, 9999);
        string name = "Mordokay" + random;

        yield return StartCoroutine(this.GetComponent<GameRecorder>().CreateTable(name));

        foreach(PlayerMovementController.Action action in playerActions)
        {
            yield return StartCoroutine(this.GetComponent<GameRecorder>().PostScores(name, action.type, action.value.ToString()));
        }

        yield return StartCoroutine(this.GetComponent<GameRecorder>().GetTable(name));
    }
}
