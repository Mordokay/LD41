using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject waitButton;

    public Text remainingTurnsText;
    public LayerMask groundLayer;

    public bool rollingDices;
    public GameObject diceBox;

    public GameObject DiceCamera;
    public GameObject MainCamera;

    UIManager ui;

    public bool routineSave;
    public int posActionSave;
    public string nicknameForSave;

    string actionData;

    void Start () {
        routineSave = false;
        posActionSave = 0;

        rollingDices = false;
        actionData = "";
        ui = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        stillAtStart = true;
        //Time.timeScale = 0.2f;

        playingStage = false;
        //Randomly choose who starts playing
        currentPlayerPlayingId = Random.Range(0, maxPlayers);
        while(currentPlayerPlayingId == 0)
        {
            currentPlayerPlayingId = Random.Range(0, maxPlayers);
        }

        if (currentPlayerPlayingId == 0)
        {
            endTurnButton.SetActive(false);
            waitButton.SetActive(false);
            remainingTurnsText.enabled = true;

            rollingDices = true;
            diceBox.GetComponent<DiceManager>().CanRollDices();
            DiceCamera.SetActive(true);
            MainCamera.SetActive(false);
        }
        player = GameObject.FindGameObjectWithTag("Player");
        playerActions = new List<PlayerMovementController.Action>();

        currentStage = 0;
    }

    public void PlayStage()
    {
        if (!playingStage && ui.gameStarted)
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

    public void ReturnToPlayer(int diceCount)
    {
        DiceCamera.SetActive(false);
        MainCamera.SetActive(true);

        rollingDices = false;

        endTurnButton.SetActive(true);
        waitButton.SetActive(true);
        remainingTurnsText.enabled = true;

        currentPlayerPlayingId = 0;
        player.GetComponent<PlayerMovementController>().RefreshArrows();
        //Since it is the player playing again we roll the dice to decide how many plays he is going to do.

        player.GetComponent<PlayerMovementController>().playerRemainingMoves = diceCount;
        remainingTurnsText.text = "Remaining Moves: " + diceCount;

        //Add the turn stage in the begining 
        playerActions.Add(new PlayerMovementController.Action("t", currentStage));

        RaycastHit hit;
        
        if (Physics.Raycast(this.GetComponent<PlayersLoader>().players[currentPlayerPlayingId].transform.position, Vector3.down, out hit, 1.0f, groundLayer))
        {
            if(!hit.collider.tag.Equals("Boat") && !hit.collider.tag.Equals("Platform"))
            {
                this.GetComponent<PlayersLoader>().players[currentPlayerPlayingId].GetComponent<PlayerMovementController>().lastSafePos =
            this.GetComponent<PlayersLoader>().players[currentPlayerPlayingId].gameObject.transform.position;
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
            endTurnButton.SetActive(false);
            waitButton.SetActive(false);

            rollingDices = true;
            diceBox.GetComponent<DiceManager>().CanRollDices();
            DiceCamera.SetActive(true);
            MainCamera.SetActive(false);
        }
        else
        {
            remainingTurnsText.enabled = false;
            endTurnButton.SetActive(false);
            waitButton.SetActive(false);
            player.GetComponent<PlayerMovementController>().HideArrows();
            RaycastHit hit;

            if (Physics.Raycast(this.GetComponent<PlayersLoader>().players[currentPlayerPlayingId].transform.position, Vector3.down, out hit, 1.0f, groundLayer))
            {
                if (!hit.collider.tag.Equals("Boat") && !hit.collider.tag.Equals("Platform"))
                {
                    this.GetComponent<PlayersLoader>().players[currentPlayerPlayingId].GetComponent<PlayerMovementController>().lastSafePos =
                this.GetComponent<PlayersLoader>().players[currentPlayerPlayingId].gameObject.transform.position;
                }
            }
        }
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if(currentPlayerPlayingId == 0)
            {
                player.GetComponent<PlayerMovementController>().playerRemainingMoves = Random.Range(2, 12);
            }
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
        if (routineSave)
        {
            //Debug.Log("playerActions.count: " + playerActions.Count);
            Debug.Log("playerActions.count: " + actionData.Split('|').Length);
            while (playerActions.Count > 0)
            {
                actionData += playerActions[0].type + ' ' + playerActions[0].value.ToString() + '|';
                playerActions.RemoveAt(0);
            }
           // actionData = actionData.Substring(0, actionData.Length - 1);
            //Debug.Log(actionData);
            //StartCoroutine(SaveData(nicknameForSave));
            StartCoroutine(CreateTable(nicknameForSave));
            StartCoroutine(this.GetComponent<GameRecorder>().PostData(nicknameForSave, actionData));
            routineSave = false;
        }
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(this.GetComponent<GameRecorder>().CreateTable(nicknameForSave));
        }
        */
        
    }

    public IEnumerator SaveData(string playerName)
    {
        yield return StartCoroutine(this.GetComponent<GameRecorder>().PostData(playerName, actionData));
    }

    public IEnumerator CreateTable (string playerName)
    {
        yield return StartCoroutine(this.GetComponent<GameRecorder>().CreateTable(playerName));
    }
}
