using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour {

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
