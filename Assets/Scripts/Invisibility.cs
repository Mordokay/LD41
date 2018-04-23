using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisibility : MonoBehaviour {

    public List<GameObject> objectsToSetinvisible;

    public Material mazeRockInvisible;
    public Material mazeRockNormal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("TriggerInvisibility");
            foreach (GameObject obj in objectsToSetinvisible)
            {
                obj.GetComponent<MeshRenderer>().material = mazeRockInvisible;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            foreach (GameObject obj in objectsToSetinvisible)
            {
                obj.GetComponent<MeshRenderer>().material = mazeRockNormal;
            }
        }
    }
}
