using UnityEngine;

public class AreaChecker : MonoBehaviour
{ // �� ����Ʈ����
    public LayerMask enemyMask;
    public Obstacles myStones;
    BoxCollider myBox;

    private void OnTriggerEnter(Collider other)
    {
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            myStones.DropStones();
            myBox = this.GetComponent<BoxCollider>();
            myBox.enabled = false;
        }

    }

}
