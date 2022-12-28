using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapSphereSizeChecker : MonoBehaviour
{
    public float radius = 0.1f;
    public Material detectedMat;

    void changeMaterial(GameObject go, Material changeMat)
    {
        Renderer rd = go.GetComponent<MeshRenderer>();
        Material[] mat = rd.sharedMaterials;
        mat[0] = changeMat;
        rd.materials = mat;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, radius);
    }

    void Update()
    {
        Collider[] colliders =
                    Physics.OverlapSphere(this.transform.position, radius);

        foreach (Collider col in colliders)
        {
            if (col.name == "Sphere" /* 자기 자신은 제외 */) continue;

            changeMaterial(col.gameObject, detectedMat);
        }
    }
}
