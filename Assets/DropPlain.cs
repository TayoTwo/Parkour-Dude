using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPlain : MonoBehaviour
{

    public float timeTillDrop;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    IEnumerator Drop(){

        yield return new WaitForSeconds(timeTillDrop);
        rb.isKinematic = false;

    }

    void OnCollisionEnter(Collision col){

        if (col.gameObject.tag == "Player"){

            StartCoroutine(Drop());

        }

    }
}
