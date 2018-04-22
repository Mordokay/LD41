using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    float speedRotation = 4.0f;
    float speedZoom = 4.0f;

    public float ZoomMin;
    public float ZoomMax;
    public float currentZoom;

    public GameObject targetPlayer;
    GameObject player;

    public float totalCameraRotation = 0.0f;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        targetPlayer = player;
        this.transform.position = targetPlayer.transform.position + Vector3.one * currentZoom;
        transform.LookAt(targetPlayer.transform.position);
    }

    void Update()
    {
        this.transform.position = player.transform.position + Vector3.one * currentZoom;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            totalCameraRotation += 20 * Time.deltaTime * speedRotation;
            //transform.RotateAround(targetPos, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * speedRotation);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            totalCameraRotation += 20 * Time.deltaTime * -speedRotation;
            //transform.RotateAround(targetPos, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * -speedRotation);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.LeftArrow))
        {
            if(Vector3.Distance(Camera.main.transform.position, player.transform.position) > ZoomMin)
            {
                currentZoom -= Time.deltaTime * speedZoom;
                //Camera.main.transform.position = Camera.main.transform.position + Camera.main.transform.forward.normalized * Time.deltaTime * speedZoom;
            }
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Vector3.Distance(Camera.main.transform.position, player.transform.position) < ZoomMax)
            {
                currentZoom += Time.deltaTime * speedZoom;
                //Camera.main.transform.position = Camera.main.transform.position - Camera.main.transform.forward.normalized * Time.deltaTime * speedZoom;
            }
        }
        transform.RotateAround(targetPlayer.transform.position, new Vector3(0.0f, 1.0f, 0.0f), totalCameraRotation);
        transform.LookAt(targetPlayer.transform.position);
    }
}
