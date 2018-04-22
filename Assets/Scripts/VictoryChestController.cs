using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryChestController : MonoBehaviour {

    UIManager ui;
    GameObject gm;

    private void Start()
    {
        ui = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        gm = GameObject.FindGameObjectWithTag("GameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            gm.GetComponent<GameData>().savingPlayerData = true;

            Debug.Log("Player wins!!!");
            ui.ShowPlayerWin(other.gameObject.name);
        }
        else if(other.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("Enemy Wins wins!!!");
            ui.ShowEnemyWin(other.gameObject.name);
        }
    }
}
