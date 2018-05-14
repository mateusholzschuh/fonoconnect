using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Decrease the player life
        HUDController.health -= 10;  

        //Fix the bug of multiples hits in the same barrier
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
