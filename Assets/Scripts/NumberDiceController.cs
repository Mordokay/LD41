using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberDiceController : MonoBehaviour {

    public int thisNumber;
    public bool isDiceA;
    public DiceManager dm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("DiceBase"))
        {
            if (isDiceA)
            {
                dm.UpdateDiceA(thisNumber);
            }
            else
            {
                dm.UpdateDiceB(thisNumber);
            }
        }
    }
}
