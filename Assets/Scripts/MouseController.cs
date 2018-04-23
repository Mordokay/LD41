using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    public LayerMask arrowLayerMask;

	void Update () {
        if (Input.GetMouseButtonDown(0) && !this.GetComponent<GameData>().rollingDices)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, arrowLayerMask))
            {
                switch (hit.collider.gameObject.name)
                {
                    case "ArrowLeft":
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().MoveLeft(1.0f);
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().HideArrows();
                        this.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("l", 1));
                        this.GetComponent<GameData>().PlayStage();
                        break;
                    case "ArrowRight":
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().MoveRight(1.0f);
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().HideArrows();
                        this.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("r", 1));
                        this.GetComponent<GameData>().PlayStage();
                        break;
                    case "ArrowForward":
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().MoveForward(1.0f);
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().HideArrows();
                        this.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("f", 1));
                        this.GetComponent<GameData>().PlayStage();
                        break;
                    case "ArrowBackward":
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().MoveBackward(1.0f);
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().HideArrows();
                        this.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("b", 1));
                        this.GetComponent<GameData>().PlayStage();
                        break;
                    case "ArrowUp":
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().MoveUp(1.0f);
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().HideArrows();
                        this.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("u", 1));
                        this.GetComponent<GameData>().PlayStage();
                        break;
                    case "ArrowDown":
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().MoveDown(1.0f);
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovementController>().HideArrows();
                        this.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("d", 1));
                        this.GetComponent<GameData>().PlayStage();
                        break;
                }
            }
        }
    }
}
