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

    void Start () {
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
    }
    private void Update()
    {
        if (playingStage)
        {
            currentPlayDuration += Time.deltaTime;
            this.transform.localPosition = Vector3.Lerp(startPos, endPos, currentPlayDuration / playDuration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            //Pushes player back to start position
            other.gameObject.GetComponent<PlayerMovementController>().RevertPos();
            Debug.Log("RevertingPos");
        }
    }
}
