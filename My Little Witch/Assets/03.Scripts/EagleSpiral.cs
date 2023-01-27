using System.Collections;
using UnityEngine;

public class EagleSpiral : MonoBehaviour
{
    public enum EagleMovement { Create, Forward, Spiral, Leave, Disappear }
    public EagleMovement eagleMovement = EagleMovement.Create;

    public float speed;
    public float rotateDuration;

    public GameObject[] dropItems;

    // 시작하고 저장해서 위치값 바뀌지 않도록
    Vector3 myTargetPos;
    Vector3 myExitPos;
    Vector3 myTargetDir;

    void Start()
    {
        E_ChangeState(EagleMovement.Create);
    }

    void Update()
    {
        E_StateProcess();
    }


    public void E_ChangeState(EagleMovement what)
    {
        //if (eagleMovement == what) return;
        eagleMovement = what;
        switch (eagleMovement)
        {
            case EagleMovement.Create:
                myTargetPos = SceneData.Inst.myPlayer.transform.position;
                myExitPos = SceneData.Inst.EagleExitPos.transform.position;
                E_ChangeState(EagleMovement.Forward);
                break;
            case EagleMovement.Forward:
                break;
            case EagleMovement.Spiral:
                StartCoroutine(RotateAround(rotateDuration));
                break;
            case EagleMovement.Leave:
                StartCoroutine(Rotating((myExitPos - transform.position).normalized));
                break;
            case EagleMovement.Disappear:
                Destroy(this.gameObject);
                break;
        }
    }

    IEnumerator Rotating(Vector3 dir) // 독수리가 위로 향해 올라갈 때 자연스러움을 위해
    {
        Quaternion end = Quaternion.LookRotation(dir);
        Quaternion start = transform.rotation;
        float t = 0.0f;
        while (t < 1.0f)
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
                myTargetDir = (myTargetPos - transform.position).normalized;
                transform.position += myTargetDir * (speed * Time.deltaTime);
                if (Vector3.Distance(myTargetPos, transform.position) < 1f)
                { E_ChangeState(EagleMovement.Spiral); }
                break;
            case EagleMovement.Spiral:
                break;
            case EagleMovement.Leave:
                myTargetDir = (myExitPos - transform.position).normalized;
                transform.position += myTargetDir * (speed * Time.deltaTime);
                if (Vector3.Distance(myExitPos, transform.position) < 1f)
                {
                    E_ChangeState(EagleMovement.Disappear);
                }
                break;
            case EagleMovement.Disappear:
                break;
        }
    }



    IEnumerator RotateAround(float duration)
    {
        while (duration > 0.0f)
        {
            duration -= Time.deltaTime;
            transform.RotateAround(myTargetPos, Vector3.up, -120f * Time.deltaTime);
            yield return null;
        }
        E_ChangeState(EagleMovement.Leave);
    }
}
