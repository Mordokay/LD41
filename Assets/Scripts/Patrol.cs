using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {

    public List<Vector3> patrolPositions;
    bool playingStage;
    float playDuration;
    float currentPlayDuration;

    Vector3 startPos;
    Vector3 endPos;

    GameData gd;
    List<GameObject> playersToMove;

    void Start () {
        playersToMove = new List<GameObject>();
        gd = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameData>();
        playingStage = false;
        playDuration = 0.5f;
        currentPlayDuration = 0.0f;
    }

    public void PlayNext(int stageToPlay)
    {
        currentPlayDuration = 0.0f;
        startPos = patrolPositions[stageToPlay];
        if (stageToPlay == 3) {
            if (this.gameObject.tag.Equals("Bullet"))
            {
                startPos = patrolPositions[0];
            }
            endPos = patrolPositions[0];
        }
        else
        {
            endPos = patrolPositions[stageToPlay + 1];
        }
        playingStage = true;

        if (this.gameObject.tag.Equals("Boat") || this.gameObject.tag.Equals("Platform"))
        {
            foreach (GameObject player in this.GetComponentInChildren<PlatformController>().playersOnPlatform)
            {
                if (player.GetComponent<PlayerMovementController>().waiting && 
                    player.GetComponent<PlayerMovementController>().participantID == 0)
                {
                    Vector3 dir = endPos - startPos;
                    if(dir.x > 0)
                    {
                        Debug.Log("Backward");
                        //player.GetComponent<PlayerMovementController>().MoveBackward(1);
                        gd.playerActions.Add(new PlayerMovementController.Action("b", 1));
                    }
                    else if(dir.x < 0)
                    {
                        Debug.Log("Forward");
                        //player.GetComponent<PlayerMovementController>().MoveForward(1);
                        gd.playerActions.Add(new PlayerMovementController.Action("f", 1));
                        //Forward
                    }
                    if (dir.z > 0)
                    {
                        Debug.Log("Right");
                        //player.GetComponent<PlayerMovementController>().MoveRight(1);
                        gd.playerActions.Add(new PlayerMovementController.Action("r", 1));
                        //Right
                    }
                    else if (dir.z < 0)
                    {
                        Debug.Log("Left");
                        //player.GetComponent<PlayerMovementController>().MoveLeft(1);
                        gd.playerActions.Add(new PlayerMovementController.Action("l", 1));
                        //Left
                    }
                }
            }
        }
    }

    private void Update()
    {
        
        if (this.gameObject.tag.Equals("Boat") || this.gameObject.tag.Equals("Platform"))
        {
            playersToMove.Clear();
            foreach (GameObject player in this.GetComponentInChildren<PlatformController>().playersOnPlatform)
            {
                playersToMove.Add(player);
            }
        }

        if (playingStage)
        {
            currentPlayDuration += Time.deltaTime;
            this.transform.localPosition = Vector3.Lerp(startPos, endPos, currentPlayDuration / playDuration);
            
            if(playersToMove.Count > 0)
            {
                foreach(GameObject p in playersToMove)
                {
                    p.transform.position = this.transform.localPosition + Vector3.up;
                }
            }
            
            if(this.transform.localPosition == endPos)
            {
                playingStage = false;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && (this.gameObject.tag.Equals("Spikes") || this.gameObject.tag.Equals("Bullet")))
        {
            //Pushes player back to start position
            other.gameObject.GetComponent<PlayerMovementController>().RevertPos();
            Debug.Log("RevertingPos");
        }
    }
}
