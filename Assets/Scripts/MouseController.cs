using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                switch (hit.collider.gameObject.name)
                {
                    case "ArrowLeft":
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().MoveLeft(1.0f);
                        break;
                    case "ArrowRight":
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().MoveRight(1.0f);
                        break;
                    case "ArrowForward":
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().MoveForward(1.0f);
                        break;
                    case "ArrowBackward":
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().MoveBackward(1.0f);
                        break;
                    case "ArrowUp":
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().MoveUp(1.0f);
                        break;
                    case "ArrowDown":
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().MoveDown(1.0f);
                        break;
                }
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }
}
