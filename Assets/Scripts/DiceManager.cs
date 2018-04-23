using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour {

    public Transform dicePosA;
    public Transform dicePosB;
    public GameObject DiceA;
    public GameObject DiceB;
    GameData gd;

    Vector3 torque;

    public bool canRoll;
    public bool rollingDices;

    public int diceAValue;
    public int diceBValue;

    void Start () {
        diceAValue = 0;
        diceBValue = 0;

        canRoll = false;
        rollingDices = false;
        gd = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameData>();
	}

    public void CanRollDices()
    {
        canRoll = true;
    }

    public void RollDices()
    {
        DiceA.transform.position = dicePosA.position;
        DiceB.transform.position = dicePosB.position;

        torque.x = Random.Range(-200, 200);
        torque.y = Random.Range(-200, 200);
        torque.z = Random.Range(-200, 200);
        DiceA.GetComponent<Rigidbody>().angularVelocity = torque;

        torque.x = Random.Range(-200, 200);
        torque.y = Random.Range(-200, 200);
        torque.z = Random.Range(-200, 200);
        DiceB.GetComponent<Rigidbody>().angularVelocity = torque;
    }

    public void UpdateDiceA(int value)
    {
        diceAValue = value;
    }

    public void UpdateDiceB(int value)
    {
        diceBValue = value;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && canRoll) {
            RollDices();
            canRoll = false;
            rollingDices = true;
        }

		if(rollingDices && DiceA.GetComponent<Rigidbody>().IsSleeping() && DiceA.GetComponent<Rigidbody>().IsSleeping())
        {
            gd.ReturnToPlayer(diceAValue + diceBValue);

            rollingDices = false;
        }
	}
}
