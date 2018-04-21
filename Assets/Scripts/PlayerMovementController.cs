using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    float timeBetweenActions;
    float actionDuration;
    float actionCurrentDuration;
    private float actionRemainingTime;
    bool canDoAction;
    int remainingHorizontalMovementsAfterJump;
    bool moving;
    Vector3 startPos;
    Vector3 endPos;

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
    //t -> Finishes Turn
    //g -> Finishes Game

    void Start()
    {
        timeBetweenActions = 1.5f;
        actionDuration = 0.5f;
        canDoAction = true;
        moving = false;
        remainingHorizontalMovementsAfterJump = 0;

        if (!isPlayerControlled)
        {
            HideArrows();
        }
    }

    void HideArrows()
    {
        arrowLeft.SetActive(false);
        arrowRight.SetActive(false);
        arrowForward.SetActive(false);
        arrowBackward.SetActive(false);
        arrowUp.SetActive(false);
        arrowDown.SetActive(false);
    }

    void RefreshArrows()
    {
        HideArrows();
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameData>().currentPlayerPlayingId == 0)
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
            if (!Physics.Raycast(transform.position, Vector3.up, out hit, 1.0f, groundLayer))
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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            remainingHorizontalMovementsAfterJump = 2;
        }
    }
    private void FixedUpdate()
    {
        if (isPlayerControlled)
        {
            RefreshArrows();
        }
    }

    public void MoveLeft(float value)
    {
        moving = true;
        startPos = this.transform.position;
        endPos = this.transform.position - Vector3.forward * value;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;
    }
    public void MoveRight(float value)
    {
        moving = true;
        startPos = this.transform.position;
        endPos = this.transform.position + Vector3.forward * value;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;
    }
    public void MoveUp(float value)
    {
        moving = true;
        startPos = this.transform.position;
        endPos = this.transform.position + Vector3.up * value;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;
    }
    public void MoveDown(float value)
    {
        moving = true;
        startPos = this.transform.position;
        endPos = this.transform.position - Vector3.up * value;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;
    }
    public void MoveForward(float value)
    {
        moving = true;
        startPos = this.transform.position;
        endPos = this.transform.position - Vector3.right * value;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;
    }
    public void MoveBackward(float value)
    {
        moving = true;
        startPos = this.transform.position;
        endPos = this.transform.position + Vector3.right * value;
        actionRemainingTime = timeBetweenActions;
        actionCurrentDuration = 0.0f;
    }

    //Creates Actions for the enemies based on mySQL table or local .txt file
    public void GenerateActions()
    {
        actionList = new List<Action>();
        actionList.Add(new Action("r", 1));
        actionList.Add(new Action("r", 1));
        actionList.Add(new Action("b", 1));
        actionList.Add(new Action("t", 0));
        actionList.Add(new Action("b", 1));
        actionList.Add(new Action("l", 1));
        actionList.Add(new Action("t", 0));
        actionList.Add(new Action("b", 1));
        actionList.Add(new Action("r", 1));
        actionList.Add(new Action("u", 1));
        actionList.Add(new Action("t", 0));
        actionList.Add(new Action("l", 1));
        actionList.Add(new Action("b", 1));
        actionList.Add(new Action("t", 0));
        actionList.Add(new Action("d", 1));
        actionList.Add(new Action("g", 0));
    }

    void Update () {
        actionRemainingTime -= Time.deltaTime;

        if (moving)
        {
            actionCurrentDuration += Time.deltaTime;
            this.transform.position = Vector3.Lerp(startPos, endPos, actionCurrentDuration / actionDuration);
            if(actionCurrentDuration >=  actionDuration)
            {
                moving = false;
                this.transform.position = endPos;

                //if it was player moving
                if(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameData>().currentPlayerPlayingId == 0)
                {
                    //Decrement Player moves
                    playerRemainingMoves -= 1;
                    if(playerRemainingMoves == 0)
                    {
                        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameData>().NextParticipant();
                    }
                }
            }
        }
        else if (actionList.Count > 0 && !isPlayerControlled &&
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameData>().currentPlayerPlayingId == participantID)
        {
            if (actionRemainingTime <= 0.0f && canDoAction)
            {
                if (actionList[0].type.Equals("t"))
                {
                    //EndTurn
                    Debug.Log("EndTurn");
                    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameData>().NextParticipant();
                }
                else if (actionList[0].type.Equals("g"))
                {
                    //EndTurn
                    Debug.Log("EndsGame");
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
                    }
                }
                actionList.RemoveAt(0);
            }
        }
	}
}
