using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementController : MonoBehaviour
{
    float timeBetweenActions;
    float actionDuration;
    float actionCurrentDuration;
    private float actionRemainingTime;
    bool canDoAction;
    public bool jumping;
    public bool droppingDown;
    int remainingHorizontalMovementsAfterJump;
    public bool moving;
    public bool waiting;
    Vector3 startPos;
    Vector3 endPos;

    public Vector3 lastSafePos;

    public GameObject arrowLeft;
    public GameObject arrowRight;
    public GameObject arrowForward;
    public GameObject arrowBackward;
    public GameObject arrowUp;
    public GameObject arrowDown;

    public LayerMask groundLayer;

    public bool isPlayerControlled;
    public int participantID;
    public int playerRemainingMoves;

    public Text remainingTurnsText;
    GameObject gm;

    public int startStage;
    public bool firstMove;

    GameObject playerBody;

    public Material[] hatMaterials;
    public Material[] shirtMaterials;
    public Material[] pantsMaterials;

    public Material trapMaterial;

    [System.Serializable]
    public class Action
    {
        public string type;
        public float value;

        public Action(string type, float value)
        {
            this.type = type;
            this.value = value;
        }
    }

    public List<Action> actionList;

    //u -> Up
    //d -> Down
    //l -> Left
    //r -> Right
    //f -> Forward
    //b -> Backward
    //x -> Reverts
    //t -> Finishes Turn
    //g -> Finishes Game

    void Start()
    {
        //hatMaterials = Resources.LoadAll("PlayerMaterials\\Hats") as Material[];
        //shirtMaterials = Resources.LoadAll("PlayerMaterials\\Shirts") as Material[];
        //pantsMaterials = Resources.LoadAll("PlayerMaterials\\Pants") as Material[];
        playerRemainingMoves = 0;

        playerBody = this.transform.GetChild(0).gameObject;
        gm = GameObject.FindGameObjectWithTag("GameManager");
        timeBetweenActions = 1.0f;
        actionDuration = 0.5f;
        canDoAction = true;
        moving = false;
        waiting = false;
        remainingHorizontalMovementsAfterJump = 0;
        jumping = false;
        droppingDown = false;
        firstMove = true;
        if (!isPlayerControlled)
        {
            HideArrows();
        }
        remainingTurnsText = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).gameObject.GetComponent<Text>();

        playerBody.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = hatMaterials[Random.Range(0, hatMaterials.Length)];
        playerBody.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = shirtMaterials[Random.Range(0, shirtMaterials.Length)];
        playerBody.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material = pantsMaterials[Random.Range(0, pantsMaterials.Length)];

        int random = Random.Range(2, 12);
        playerRemainingMoves = random;
    }

    public void HideArrows()
    {
        arrowLeft.SetActive(false);
        arrowRight.SetActive(false);
        arrowForward.SetActive(false);
        arrowBackward.SetActive(false);
        arrowUp.SetActive(false);
        arrowDown.SetActive(false);
    }

    public void RevertPos()
    {
        if (moving && gm.GetComponent<GameData>().currentPlayerPlayingId == participantID)
        {
            gm.GetComponent<GameData>().playerActions.RemoveAt(gm.GetComponent<GameData>().playerActions.Count - 1);
        }
        this.transform.position = lastSafePos;
        playerRemainingMoves = 0;
        moving = false;
        jumping = false;
        droppingDown = false;
        firstMove = true;

        gm.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("x", 0));
        HideArrows();
        gm.GetComponent<GameData>().NextParticipant();
    }

    public void RefreshArrows()
    {
        HideArrows();
        if (gm.GetComponent<GameData>().currentPlayerPlayingId == 0)
        {
            RaycastHit hit;
            // Left
            if (!Physics.Raycast(transform.position, -Vector3.forward, out hit, 1.0f, groundLayer))
            {
                arrowLeft.SetActive(true);
            }
            // Right
            if (!Physics.Raycast(transform.position, Vector3.forward, out hit, 1.0f, groundLayer))
            {
                arrowRight.SetActive(true);
            }
            // Forward
            if (!Physics.Raycast(transform.position, -Vector3.right, out hit, 1.0f, groundLayer))
            {
                arrowForward.SetActive(true);
            }
            // Backward
            if (!Physics.Raycast(transform.position, Vector3.right, out hit, 1.0f, groundLayer))
            {
                arrowBackward.SetActive(true);
            }
            // Up
            if (!Physics.Raycast(transform.position, Vector3.up, out hit, 1.0f, groundLayer) && !jumping)
            {
                arrowUp.SetActive(true);
            }
            // Down
            if (!Physics.Raycast(transform.position, -Vector3.up, out hit, 1.0f, groundLayer))
            {
                arrowDown.SetActive(true);
            }
        }
    }

    public void Wait()
    {
        RaycastHit hit;

        if (isPlayerControlled && jumping && remainingHorizontalMovementsAfterJump > 0)
        {
            remainingHorizontalMovementsAfterJump -= 1;
        }
        moving = true;
        waiting = true;
        startPos = this.transform.position;
        endPos = this.transform.position;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;

        Physics.Raycast(transform.position, -Vector3.up, out hit, 1.0f, groundLayer);
        if (isPlayerControlled && hit.collider != null && !hit.collider.gameObject.tag.Equals("Boat") && !hit.collider.gameObject.tag.Equals("Platform"))
        {
            gm.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("w", 0));
        }
        
        gm.GetComponent<GameData>().PlayStage();
    }

    public void MoveLeft(float value)
    {
        if (isPlayerControlled && jumping && remainingHorizontalMovementsAfterJump > 0)
        {
            remainingHorizontalMovementsAfterJump -= 1;
        }
        moving = true;
        startPos = this.transform.position;
        endPos = this.transform.position - Vector3.forward * value;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;
        playerBody.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void MoveRight(float value)
    {
        if (isPlayerControlled && jumping && remainingHorizontalMovementsAfterJump > 0)
        {
            remainingHorizontalMovementsAfterJump -= 1;
        }
        moving = true;
        startPos = this.transform.position;
        endPos = this.transform.position + Vector3.forward * value;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;
        playerBody.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
    }
    public void MoveUp(float value)
    {
        moving = true;
        jumping = true;
        droppingDown = false;
        remainingHorizontalMovementsAfterJump = 2;
        startPos = this.transform.position;
        endPos = this.transform.position + Vector3.up * value;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;
    }
    public void MoveDown(float value)
    {
        moving = true;
        jumping = false;
        droppingDown = true;
        startPos = this.transform.position;
        endPos = this.transform.position - Vector3.up * value;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;
    }
    public void MoveForward(float value)
    {
        if (isPlayerControlled && jumping && remainingHorizontalMovementsAfterJump > 0)
        {
            remainingHorizontalMovementsAfterJump -= 1;
        }
        moving = true;
        startPos = this.transform.position;
        endPos = this.transform.position - Vector3.right * value;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;
        playerBody.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
    }
    public void MoveBackward(float value)
    {
        if (isPlayerControlled && jumping && remainingHorizontalMovementsAfterJump > 0)
        {
            remainingHorizontalMovementsAfterJump -= 1;
        }
        moving = true;
        startPos = this.transform.position;
        endPos = this.transform.position + Vector3.right * value;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;
        playerBody.transform.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
    }

    void Update()
    {
        if (!gm.GetComponent<GameData>().rollingDices)
        {
            actionRemainingTime -= Time.deltaTime;
            if (isPlayerControlled)
            {
                //Update UI on remaingin moves
                remainingTurnsText.text = "Remaining Moves: " + playerRemainingMoves;
            }

            if (moving)
            {
                actionCurrentDuration += Time.deltaTime;
                RaycastHit hit;

                if (!waiting)
                {
                    this.transform.position = Vector3.Lerp(startPos, endPos, actionCurrentDuration / actionDuration);
                }

                if (actionCurrentDuration >= actionDuration)
                {
                    waiting = false;
                    startPos = this.transform.position;
                    endPos = this.transform.position;
                    moving = false;
                    this.transform.position = endPos;
                    //if it was player moving
                    if (gm.GetComponent<GameData>().currentPlayerPlayingId == 0)
                    {
                        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1.0f, groundLayer))
                        {
                            droppingDown = false;
                            jumping = false;
                        }

                        if (!Physics.Raycast(transform.position, -Vector3.up, out hit, 1.0f, groundLayer) && !jumping)
                        {
                            droppingDown = true;
                            MoveDown(1);
                            gm.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("d", 1));
                            gm.GetComponent<GameData>().PlayStage();
                        }
                        else
                        {
                            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1.0f, groundLayer) && 
                                hit.collider.gameObject.tag.Equals("Trap"))
                            {
                                RevertPos();
                                hit.collider.gameObject.GetComponent<MeshRenderer>().material = trapMaterial;
                            }
                            else
                            {
                                if (!Physics.Raycast(transform.position, -Vector3.up, out hit, 1.0f, groundLayer) && droppingDown)
                                {
                                    //continues to more down if it didnt find the floor
                                    MoveDown(1);
                                    gm.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("d", 1));
                                    gm.GetComponent<GameData>().PlayStage();
                                }
                            

                            if (remainingHorizontalMovementsAfterJump == 0 && jumping)
                            {
                                //falls down
                                MoveDown(1);
                                gm.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("d", 1));
                                gm.GetComponent<GameData>().PlayStage();
                            }
                            else
                            {
                                //Decrement Player moves
                                playerRemainingMoves -= 1;

                                RefreshArrows();
                                if (playerRemainingMoves <= 0)
                                {
                                    if (jumping)
                                    {
                                        MoveDown(1);
                                        gm.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("d", 1));
                                        gm.GetComponent<GameData>().PlayStage();
                                    }
                                    else
                                    {
                                        HideArrows();
                                        gm.GetComponent<GameData>().NextParticipant();
                                    }
                                }
                            }
                            }
                        }
                    }
                }
            }
            else if (actionList.Count > 0 && !isPlayerControlled &&
                gm.GetComponent<GameData>().currentPlayerPlayingId == participantID)
            {
                if (actionRemainingTime <= 0.0f && canDoAction)
                {
                    if (actionList[0].type.Equals("t"))
                    {
                        //EndTurn
                        //Debug.Log("EndTurn");
                        startStage = (int)actionList[0].value;
                        gm.GetComponent<GameData>().NextParticipant();
                        firstMove = true;
                        actionList.RemoveAt(0);
                    }
                    else if (actionList[0].type.Equals("g"))
                    {
                        //EndTurn
                        //Debug.Log("EndsGame");
                    }
                    else
                    {
                        gm.GetComponent<GameData>().PlayStage();
                        if (firstMove)
                        {
                            if (startStage == gm.GetComponent<GameData>().currentStage)
                            {
                                switch (actionList[0].type)
                                {
                                    case "u":
                                        MoveUp(actionList[0].value);
                                        break;
                                    case "d":
                                        MoveDown(actionList[0].value);
                                        break;
                                    case "l":
                                        MoveLeft(actionList[0].value);
                                        break;
                                    case "r":
                                        MoveRight(actionList[0].value);
                                        break;
                                    case "f":
                                        MoveForward(actionList[0].value);
                                        break;
                                    case "b":
                                        MoveBackward(actionList[0].value);
                                        break;
                                    case "x":
                                        this.transform.position = lastSafePos;
                                        break;
                                    case "w":
                                        Wait();
                                        break;
                                }
                                actionList.RemoveAt(0);
                                firstMove = false;
                            }
                        }
                        else
                        {
                            switch (actionList[0].type)
                            {
                                case "u":
                                    MoveUp(actionList[0].value);
                                    break;
                                case "d":
                                    MoveDown(actionList[0].value);
                                    break;
                                case "l":
                                    MoveLeft(actionList[0].value);
                                    break;
                                case "r":
                                    MoveRight(actionList[0].value);
                                    break;
                                case "f":
                                    MoveForward(actionList[0].value);
                                    break;
                                case "b":
                                    MoveBackward(actionList[0].value);
                                    break;
                                case "x":
                                    this.transform.position = lastSafePos;
                                    break;
                                case "w":
                                    Wait();
                                    break;
                            }
                            actionList.RemoveAt(0);
                        }
                    }
                }
            }
        }
    }
}
