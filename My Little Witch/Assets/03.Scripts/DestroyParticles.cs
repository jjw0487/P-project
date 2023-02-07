using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    [SerializeField] private float destroyTime = 1f;
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

}
