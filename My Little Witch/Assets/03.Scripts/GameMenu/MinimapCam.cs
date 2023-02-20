using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    private Transform myTarget = null;
    private void Start()
    {
        if (myTarget == null) { 
        myTarget = FindObjectOfType<Player>().transform;
        }
    }
    private void Update()
    {
        transform.position = myTarget.transform.position;
    }
}
