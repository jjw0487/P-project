using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public GameObject stonPile;
    public void DropStones()
    {
        GameObject obj = Instantiate(stonPile, this.transform.position, Quaternion.identity) as GameObject;
    }
}
