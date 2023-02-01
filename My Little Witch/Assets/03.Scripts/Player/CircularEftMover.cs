using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularEftMover : MonoBehaviour
{
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;

    public SkillData skillData;
    private Vector3 offset; 

    void Start()
    {
        offset = transform.position - SceneData.Inst.myPlayer.transform.position;

        rb = GetComponent<Rigidbody>();
        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
    }

    private void FixedUpdate()
    {

        transform.position = SceneData.Inst.myPlayer.transform.position + offset;
        transform.RotateAround(SceneData.Inst.myPlayer.transform.position, Vector3.up, -120f * Time.deltaTime);
        offset = transform.position - SceneData.Inst.myPlayer.transform.position;

    }



    void OnCollisionEnter(Collision collision)
    {
        //Lock all axes movement and rotation
        rb.constraints = RigidbodyConstraints.FreezeAll;
        /////////////
        if(collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, skillData.overlapRadius);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    Monster mon = col.GetComponentInParent<Monster>();
                    if (mon == null) continue;   //나중에 nullref 나오면 예외처리 해줘야함.
                    if (!col.GetComponentInParent<Monster>().isDead)
                    {
                        col.GetComponentInParent<Monster>().OnDamage(skillData.dmg[skillData.level]);
                    }

                    ContactPoint contact = collision.contacts[0];
                    Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                    Vector3 pos = contact.point + contact.normal * hitOffset;

                    if (hit != null)
                    {
                        var hitInstance = Instantiate(hit, pos, rot);
                        if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
                        else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
                        else { hitInstance.transform.LookAt(contact.point + contact.normal); }

                        var hitPs = hitInstance.GetComponent<ParticleSystem>();
                        if (hitPs != null)
                        {
                            Destroy(hitInstance, hitPs.main.duration);
                        }
                        else
                        {
                            var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                            Destroy(hitInstance, hitPsParts.main.duration);
                        }
                    }
                    foreach (var detachedPrefab in Detached)
                    {
                        if (detachedPrefab != null)
                        {
                            detachedPrefab.transform.parent = null;
                        }
                    }
                    Destroy(gameObject);

                }
            }
        }
    }



}

