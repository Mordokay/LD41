using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryChestController : MonoBehaviour {

    UIManager ui;

    private void Start()
    {
        ui = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
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
