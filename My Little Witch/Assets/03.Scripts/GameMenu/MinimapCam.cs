using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    private Transform myTarget = null;
    private void Start()
    {
        if (myTarget == null)
        {
            myTarget = FindObjectOfType<Player>()?.transform;
        }
    }
    private void Update()
    {
        if (myTarget != null) transform.position = myTarget.transform.position;
    }
}
