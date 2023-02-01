using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public GameObject stonPile;
    public void DropStones()
    {
        GameObject obj = Instantiate(stonPile, this.transform.position, Quaternion.identity);

        SceneData.Inst.myPlayer.transform.LookAt(this.transform);

        //Vector3 dir = this.transform.position - SceneData.Inst.myPlayer.transform.position;
        //SceneData.Inst.myPlayer.transform.rotation = Quaternion.LookRotation(dir.normalized);

        Camera.main.transform.parent.GetComponent<Animator>()?.SetTrigger("Interaction");
    }
}
