using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisibility : MonoBehaviour {

    public List<GameObject> objectsToSetinvisible;

    public Color color;

    void Start () {
		
	}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("TriggerInvisibility");
            foreach (GameObject obj in objectsToSetinvisible)
            {
                obj.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, 40);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            foreach (GameObject obj in objectsToSetinvisible)
            {
                obj.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, 255);
            }
        }
    }
}
