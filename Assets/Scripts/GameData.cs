using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public int currentPlayerPlayingId;
    public int maxPlayers;
    GameObject player;

    void Start () {
        //Randomly choose who starts playing
        currentPlayerPlayingId = Random.Range(0, maxPlayers);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void NextParticipant()
    {
        currentPlayerPlayingId += 1;

        if (currentPlayerPlayingId == maxPlayers)
        {
            currentPlayerPlayingId = 0;

            //Since it is the player playing again we roll the dice to decide how many plays he is going to do.
            player.GetComponent<PlayerMovementController>().playerRemainingMoves = Random.Range(1, 4);
            Debug.Log("You Have: " + player.GetComponent<PlayerMovementController>().playerRemainingMoves + "moves to make!!!");
        }
    }
}
