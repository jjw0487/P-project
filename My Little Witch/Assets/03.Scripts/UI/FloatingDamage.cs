using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    public TMPro.TMP_Text dmg;
    public Transform myPos;
    // Start is called before the first frame update
    void Start()
    {
        if (myPos != null)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(myPos.position);
            transform.position = pos;
        }

        Destroy(this.gameObject, 3f);
    }

}
