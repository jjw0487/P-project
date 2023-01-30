using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public GameObject stonPile;
    public void DropStones()
    {
        GameObject obj = Instantiate(stonPile, this.transform.position, Quaternion.identity) as GameObject;

        Vector3 dir = this.transform.position - SceneData.Inst.myPlayer.transform.position;
        Quaternion target = Quaternion.LookRotation(dir.normalized);
        SceneData.Inst.myPlayer.transform.rotation = target;

        Camera.main.transform.parent.GetComponent<Animator>()?.SetTrigger("Interaction");
    }
}
