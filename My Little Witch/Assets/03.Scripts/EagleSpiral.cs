using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class EagleSpiral : MonoBehaviour
{
    private enum EagleMovement { Create, Forward, Spiral, Leave, Disappear }
    private EagleMovement eagleMovement = EagleMovement.Create;

    [SerializeField] private float speed;
    [SerializeField] private float rotateDuration;
    [SerializeField] private GameObject[] dropItems;

    // 시작하고 저장해서 위치값 바뀌지 않도록
    private Vector3 myTargetPos;
    private Vector3 myExitPos;
    private Vector3 myTargetDir;
    private int playerLV;
    private Quaternion dir;

    void Start()
    {
        E_ChangeState(EagleMovement.Create);
        dir = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }

    void Update()
    {
        E_StateProcess();
    }

    public void GetLevel(int playerLevel)
    {
        playerLV = playerLevel;
    }

    private void E_ChangeState(EagleMovement what)
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
                GameObject obj = Instantiate(dropItems[playerLV - 2], this.transform.position + new Vector3(0f,2f,0f), Quaternion.identity);
                obj.transform.SetParent(SceneData.Inst.ItemPool);
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

    private void E_StateProcess()
    {
        switch (eagleMovement)
        {
            case EagleMovement.Create:
                break;
            case EagleMovement.Forward:
                
                if (Vector3.Distance(myTargetPos, transform.position) > 5f)
                {
                    transform.rotation = Quaternion.LookRotation((myTargetPos - transform.position).normalized);
                    myTargetDir = (myTargetPos - transform.position).normalized;
                    transform.position += myTargetDir * (speed * Time.deltaTime);
                }
                else if(Vector3.Distance(myTargetPos, transform.position) < 5f && 
                    Vector3.Distance(myTargetPos, transform.position) > 1.5f)
                {
                    dir = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
                    transform.rotation = Quaternion.Lerp(transform.rotation, dir, 0.15f);
                    myTargetDir = (myTargetPos - transform.position).normalized;
                    transform.position += myTargetDir * (speed * Time.deltaTime);
                }
                else if(Vector3.Distance(myTargetPos, transform.position) < 1.5f)
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
