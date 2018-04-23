using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameTextController : MonoBehaviour {

    GameData gd;

    private void Start()
    {
        gd = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameData>();
        this.GetComponent<TextMesh>().text = transform.parent.gameObject.name;
        this.GetComponent<TextMesh>().color = new Color(Random.value, Random.value, Random.value, 1.0f);
    }

    void Update () {
        if (!gd.rollingDices)
        {
            this.GetComponent<TextMesh>().text = transform.parent.gameObject.name;
            this.transform.LookAt(this.transform.position + (this.transform.position - Camera.main.transform.position));
        }
    }
}
