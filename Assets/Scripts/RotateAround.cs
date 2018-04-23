using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

    public float rotationSpeed;

	void Update () {
        this.transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
	}
}
