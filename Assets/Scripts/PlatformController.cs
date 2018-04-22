using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    public List<GameObject> playersOnBoat;
    GameObject gm;

    private void Start()
    {
        playersOnBoat = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("Enemy"))
        {
            playersOnBoat.Add(other.gameObject);
            Debug.Log("TriggeringEnter!!!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("TriggeringExit!!!");
            playersOnBoat.Remove(other.gameObject);
        }
    }


}
