using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    public List<GameObject> playersOnPlatform;

    private void Start()
    {
        playersOnPlatform = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("Enemy"))
        {
            playersOnPlatform.Add(other.gameObject);
            Debug.Log("TriggeringEnter!!!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("TriggeringExit!!!");
            playersOnPlatform.Remove(other.gameObject);
        }
    }


}
