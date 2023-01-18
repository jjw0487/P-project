using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Movement;

public class EagleSpiral : MonoBehaviour
{
    public enum EagleMovement { Create, Forward, Spiral, Leave, Disappear }
    public EagleMovement eagleMovement = EagleMovement.Create;

    public float speed;
    public float rotateDuration;
    public Transform eagleExitPos;

    Transform myTarget;
    Vector3 myTargetDir;


    public void E_ChangeState(EagleMovement what)
    {
        //if (eagleMovement == what) return;
        eagleMovement = what;
        switch (eagleMovement)
        {
            case EagleMovement.Create:
                myTarget = SceneData.Inst.myPlayer.transform;
                E_ChangeState(EagleMovement.Forward);
                break;
            case EagleMovement.Forward:
                break;
            case EagleMovement.Spiral:
                StartCoroutine(RotateAround(rotateDuration));
                break;
            case EagleMovement.Leave:
                StartCoroutine(Rotating((eagleExitPos.position - transform.position).normalized));
                break;
            case EagleMovement.Disappear:
                Destroy(this.gameObject);
                break;
        }
    }

    IEnumerator Rotating(Vector3 dir)
    {
        Quaternion end = Quaternion.LookRotation(dir);
        Quaternion start = transform.rotation;
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }
        transform.rotation = end;
    }

    public void E_StateProcess()
    {
        switch (eagleMovement)
        {
            case EagleMovement.Create:
                break;
            case EagleMovement.Forward:
                myTargetDir = (myTarget.transform.position - transform.position).normalized;
                transform.position += myTargetDir * (speed * Time.deltaTime);
                if(Vector3.Distance(myTarget.transform.position, transform.position) < 1f)
                { E_ChangeState(EagleMovement.Spiral); }
                break;
            case EagleMovement.Spiral:
                break;
            case EagleMovement.Leave:
                //Quaternion rot = Quaternion.LookRotation((eagleExitPos.position - transform.position).normalized);
                //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, Time.deltaTime * 10f);
                myTargetDir = (eagleExitPos.transform.position - transform.position).normalized;
                transform.position += myTargetDir * (speed * Time.deltaTime);
                if(Vector3.Distance(eagleExitPos.transform.position, transform.position) < 1f)
                {
                    E_ChangeState(EagleMovement.Disappear);
                }
                break;
            case EagleMovement.Disappear:
                break;
        }
    }

    void Start()
    {
        E_ChangeState(EagleMovement.Create);
    }

    void Update()
    {
        E_StateProcess();
    }

    IEnumerator RotateAround(float duration)
    {
        while(duration > 0.0f)
        {
            duration -= Time.deltaTime;
            transform.RotateAround(myTarget.transform.position, Vector3.up, -120f * Time.deltaTime);
            yield return null;
        }
        E_ChangeState(EagleMovement.Leave);
    }
}
