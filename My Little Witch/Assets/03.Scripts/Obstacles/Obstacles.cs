using System.Linq;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public GameObject stonPile;
    public void DropStones()
    {
        GameObject obj = Instantiate(stonPile, this.transform.position, Quaternion.identity);

        SceneData.Inst.interactionManager.GetType(3f, 1, this.transform);

    }
}
