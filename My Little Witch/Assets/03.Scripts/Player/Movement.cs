using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct PlayerStat
{
    public PlayerData orgData;
    public float curHP;
    public float curMP;

}
public class Movement : CharacterProperty
{
    [Header("Character")]
    [SerializeField] private ONWHAT onWhat = ONWHAT.Street;
    [SerializeField] private GameObject KK;
    [SerializeField] private GameObject BR;
    [SerializeField] private PlayerStat charStat;

    [SerializeField] private int level; // 인스펙터에서 변경해서 테스트 위해
    [SerializeField] private int curExp;
    [SerializeField] private GameObject eagle;

    [Header("Ray")]
    public Camera mainCamera; // 메인 카메라 
    private Vector3 movePoint; // 이동 위치 저장
    private Transform normAtkTarget;
    

    [Header("Movement")]
    public Animator[] curAnim;
    [SerializeField] private float C_speed = 4f;
    [SerializeField] private float C_dashSpeed = 7f;
    [SerializeField] private float C_rotSpeed = 10f;
    [SerializeField] private LayerMask layer;
    //private bool run;
    private bool canRun = true;
    public bool stun = false; // 외부참조 필요
    public bool ground = true; // 외부참조 필요

    [Header("BroomMovement")]
    private Vector3 dir = Vector3.zero;
    [SerializeField] private float B_Speed = 10f;
    [SerializeField] private float B_RotSpeed = 3f;
    [SerializeField] private float B_AddFloatPower = 0.2f;
    [SerializeField] private float B_restrictedHeight = 1000f;
    [SerializeField] private GameObject orgDashEffect;

    [Header("UI")]
    [SerializeField] private TMPro.TMP_Text[] state;
    [SerializeField] private StaminaBar myStamina;
    [SerializeField] private UnityEngine.UI.Slider myStaminaSlider;
    [SerializeField] private HPBar myHPBar;
    [SerializeField] private GameObject[] normAtkNums;

    [Header("Skill")]
    public Skill mySkill; // 외부참조 필요
    [SerializeField] private Transform myRightHand;
    [SerializeField] private SkillData normAtkData;
    private int coolStacks;

    [Header("Game Setting")]
    //public Transform AttackMark;
    public bool OnInteraction = false; // 외부참조 필요

    public enum ONWHAT { Street, Broom } // 외부참조 필요한지 연구하자

    public void ChangeState(ONWHAT what)
    {
        //if (onWhat == what) return;
        onWhat = what;
        switch (onWhat)
        {
            case ONWHAT.Street:
                myAgent.enabled = true;
                //myRigid.drag = 0f;
                //myRigid.constraints = RigidbodyConstraints.FreezeAll;
                KK.SetActive(true);
                BR.SetActive(false);
                break;
            case ONWHAT.Broom:
                myAgent.SetDestination(transform.position);
                myAgent.enabled = false;
                myRigid.drag = 6f;
                myRigid.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
                BR.SetActive(true);
                KK.SetActive(false);
                break;
        }
    }
    public void StateProcess()
    {
        switch (onWhat)
        {
            case ONWHAT.Street:
                if (mySkill.canMove && !stun && !OnInteraction)
                {
                    C_Ray();
                    Running();

                    if (Input.GetKeyDown(KeyCode.LeftControl))
                    {
                        StartCoroutine(C_Dash());
                    }
                }

                if (!mySkill.canMove || stun) //스킬이나 스턴이 걸리면 움직임 정지
                {
                    curAnim[0].SetBool("IsWalking", false);
                    myAgent.SetDestination(transform.position);
                }

                if (Input.GetKeyDown(KeyCode.S)) // S키를 눌러 정지
                {
                    curAnim[0].SetBool("IsWalking", false);
                    myAgent.SetDestination(transform.position);
                }
                break;
            case ONWHAT.Broom:
                B_Movement();
                B_DashnHeight(); //대시와 높이제한
                break;
        }
    }

    private void Awake()
    {
        ChangeState(ONWHAT.Street);
        mainCamera = Camera.main;
    }

    void Start()
    {
        coolStacks = 0;
        StackNumCheck(coolStacks, coolStacks + 1);
        StartCoroutine(StackingCoolStacks(5f));
        myAgent.updateRotation = false; // 네비메시 로테이션을 막자
        state[0].text = "Idle";
        canRun = true; //시작할 때 바로 뛸 수 있도록

        level = charStat.orgData.Level;
        curExp = charStat.orgData.EXP[level - 1];
    }

    void Update()
    {
        StateProcess();
        CheckGround();
        if (Input.GetKeyDown(KeyCode.L) && ground)
        {
            SwitchingCharacter();
        }
    }

    private void FixedUpdate()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        //totalDist = dir.magnitude;

        // 움직임 관련해서는 나중에 옮겨주자
        if (onWhat == ONWHAT.Broom)
        {
            myRigid.MovePosition(this.transform.position + dir * B_Speed * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, dir, B_RotSpeed * Time.deltaTime);
        }

    }
  
    ////////////////////////////      UI       ///////////////////////////////////////////
    public void GetInteraction(bool trueornot)
    {
        OnInteraction = trueornot;

        myAgent.SetDestination(transform.position);

        curAnim[0].SetBool("IsWalking", false);
        curAnim[0].SetBool("IsRunning", false);

        Quaternion camRot = mainCamera.transform.parent.rotation;
        Quaternion target = Quaternion.Euler(camRot.eulerAngles.x, this.transform.rotation.eulerAngles.y, camRot.eulerAngles.z);
        mainCamera.transform.parent.rotation = target;

    }

    public void GetItemValue(int i, int value)
    {
        if (i == 1)
        {
            myHPBar.HandleHP(value);
        }
    }

    public void GetEXP(int exp)
    {
        curExp -= exp;
        if (curExp < 0)
        {
            curExp *= -1; //음수를 양수로
            curExp *= -1; 
            LevelUp(+curExp);
        }
    }
    public void LevelUp(int rest)
    {
        ++level; // 일단 스크립터블에 영향이 안가도록 데이터는 건드리지 말고 이렇게 두자
        curExp = charStat.orgData.EXP[level - 1];
        curExp -= rest;
        if (curExp < 0)
        {
            curExp *= -1;
            LevelUp(+curExp);
        }
        Instantiate(Resources.Load("Effect/MagicAura"), this.transform.position + Vector3.up * 0.2f, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
        GameObject obj = Instantiate(eagle, Camera.main.transform.position, Quaternion.identity);
        obj.GetComponent<EagleSpiral>().GetLevel(level);
    }

    public void CheckGround() // 연속점프 방지, 점프를 땅에 있을 때만
    {
        if (onWhat == ONWHAT.Street)
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.localPosition + (Vector3.up * 0.2f), Vector3.down, out hit, 0.4f, layer))
            //발끝에서 0.2만큼 올려서 아랫방향으로 0.4만큼 쏜다
            {
                ground = true;
                state[1].text = "Ground";
            }
            else
            {
                ground = false;
                state[1].text = "InAir";
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.localPosition, Vector3.down, out hit, 0.3f, layer))
            {
                ground = true;
                state[1].text = "Ground";
            }
            else
            {
                ground = false;
                state[1].text = "InAir";
            }
        }

    }
    void SwitchingCharacter()
    {
        if (ONWHAT.Broom != onWhat)
        {
            onWhat = ONWHAT.Broom;
            ChangeState(ONWHAT.Broom);
        }
        else
        {
            onWhat = ONWHAT.Street;
            ChangeState(ONWHAT.Street);
        }
    }

    public void OnDmg(float dmg)
    {
        myHPBar.HandleHP(dmg);
        StartCoroutine(Stunned(0.7f));
        curAnim[0].SetTrigger("IsHit");
    }

    void StateNotice()
    {
        if (curAnim[0].GetBool("IsWalking"))
        {
            state[0].text = "Walk";
        }

        if (curAnim[0].GetBool("IsRunning"))
        {
            state[0].text = "Run";
        }
    }


    /////////////////////////////////Character/////////////////////////////////////////////////

    void C_Ray()
    {
        /*Ray monCheck = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitMonData;
        if (Physics.Raycast(monCheck, out hitMonData, 100f, 1 << LayerMask.NameToLayer("Monster")))
        {
            //if (hitMonData.transform.parent == transform)
            hitMonData.transform.GetComponentInParent<Monster>().OnMouseHover();
        }
        else
        {
            GetComponentInParent<Monster>().OnMouseHoverExit();
        }*/

        /*rigidbody.position = Vector3.MoveTowards(transform.position, movePoint, Speed * Time.deltaTime);
        rigidbody.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed);*/

        /*totalRayDist = Vector3.Distance(transform.position, movePoint);
        Vector3 dir = movePoint - transform.position;
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);
        Quaternion targetRot = Quaternion.LookRotation(dirXZ);*/


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);            
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("Monster")) && coolStacks > 0)
            {
                StopCoroutine(SteppingBeforeNormAtk(hitData));
                if ((hitData.transform.position - transform.position).magnitude > 5f)
                {
                    StartCoroutine(SteppingBeforeNormAtk(hitData));
                    // 코루틴 실행하고 다음번 피킹때는 그 코루틴을 스탑하자 
                }
                else
                {
                    C_normAtk(hitData);
                }
            }
            else if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("Monster")) && coolStacks == 0)
            {
                curAnim[0].SetBool("IsWalking", false);
                myAgent.SetDestination(transform.position);
            }
            else if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("Item")))
            {
                myAgent.SetDestination(transform.position);
            }
            else if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("Ground")))
            {
                movePoint = hitData.point;
                if (mySkill.canMove && !stun)
                {
                    myAgent.SetDestination(movePoint);
                }
            }
        }

        if (myAgent.velocity.sqrMagnitude >= 0.1f * 0.1f && myAgent.remainingDistance <= 0.2f)
        {
            curAnim[0].SetBool("IsWalking", false);
        }
        else if (myAgent.desiredVelocity.sqrMagnitude >= 0.1f * 0.1f)
        {
            curAnim[0].SetBool("IsWalking", true);
            Vector3 direction = myAgent.desiredVelocity; // 에이전트의 이동방향
            Quaternion targetAngle = Quaternion.LookRotation(direction); //회전각도
            transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime * C_rotSpeed);
        }
    }

    void StackNumCheck(int i, int n)
    {
        normAtkNums[i].SetActive(true);

        if (i > 0)
        {
            normAtkNums[n].SetActive(false);
        }

        if (coolStacks == 0) // 0일 때 예외처리
        {
            normAtkNums[1].SetActive(false);
        }
    }

    void C_normAtk(RaycastHit hitPoint)
    {
        coolStacks--;
        StackNumCheck(coolStacks, coolStacks + 1);
        if (coolStacks == 4)
        {
            StartCoroutine(StackingCoolStacks(5f));
        }
        Vector3 dir = hitPoint.point - transform.position;
        dir.y = 0;
        if (!Mathf.Approximately(dir.magnitude, 0.0f))
        {
            myAgent.SetDestination(transform.position);
            Quaternion target = Quaternion.LookRotation(dir.normalized);
            transform.rotation = target;
        }
        normAtkTarget = hitPoint.transform;
        //skillTarget = hitPoint.transform.position;
        //normAtkData.GetComponent<ProjectileMover>().SetTarget(hitPoint.point);
        curAnim[0].SetBool("IsWalking", false);
        curAnim[0].SetTrigger("NormAtk");
    }
    public void C_OnNormAtk()
    {
        GameObject obj = Instantiate(normAtkData.Effect, myRightHand.position, transform.rotation);
        obj.GetComponent<Target>().SetTarget(normAtkTarget, new Vector3(0f, 0.5f, 0f));
        //obj.GetComponent<TargetProjectile>().UpdateTarget(hitPoint.point, new Vector3(0f, 0.5f, 0f));
    }
    void Running()
    {
        if (Mathf.Approximately(myStaminaSlider.value, 0.0f))
        //스태미너 바의 밸류가 0에 근사치에 닿을 때
        {
            canRun = false;
            /*if (myAgent.remainingDistance > 0.1f)
            {
                curAnim[0].SetBool("IsWalking", true);
            }
            else
            {
                curAnim[0].SetBool("IsWalking", false);
            }*/

            if (myAgent.velocity.sqrMagnitude >= 0.1f * 0.1f && myAgent.remainingDistance <= 0.1f)
            {
                curAnim[0].SetBool("IsWalking", false);
            }
            else if (myAgent.desiredVelocity.sqrMagnitude >= 0.1f * 0.1f)
            {
                curAnim[0].SetBool("IsWalking", true);
                Vector3 direction = myAgent.desiredVelocity; // 에이전트의 이동방향
                Quaternion targetAngle = Quaternion.LookRotation(direction); //회전각도
                transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime * 8.0f);
            }
            myAgent.speed = C_speed;
            curAnim[0].SetBool("IsRunning", false);

            //run = false;
        }
        else
        {
            //canRun= true;
            if (Input.GetKey(KeyCode.LeftShift) && myAgent.remainingDistance > 0.1f && canRun)
            // 시프트를 눌렀고, 이동거리가 있으며 canRun 이 false가 아닐 때
            {
                // run = true;
                myAgent.speed = C_dashSpeed;
                curAnim[0].SetBool("IsRunning", true);
                //rigidbody.position = Vector3.MoveTowards(transform.position, movePoint, dashSpeed * Time.deltaTime);
            }
            else // 이동거리값이 0보다 작을 때 shift로 달리기 발동 안할 수 있도록
            {
                // run = false;
                myAgent.speed = C_speed;
                curAnim[0].SetBool("IsRunning", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && !canRun)
        // 시프트 키를 떼었고, canRun 이 false일 때
        {
            canRun = true;
            myAgent.speed = C_speed;
        }
    }


    ///////////////////////////////////////////Broom//////////////////////////////////////

    void B_Movement()
    {
        //Vector3 characterDir = transform.rotation * dir;
        if (dir.z < 0)
        {
            if (dir.x < 0)//left
            {
                curAnim[1].SetBool("IsTurningRight", true);
            }
            if (dir.x > 0) //right
            {
                curAnim[1].SetBool("IsTurningLeft", true);

            }
        }

        if (dir.z > 0)
        {

            if (dir.x < 0)//left
            {
                curAnim[1].SetBool("IsTurningLeft", true);
            }
            if (dir.x > 0) //right
            {
                curAnim[1].SetBool("IsTurningRight", true);
            }
        }

        if (dir.x == 0)//Idle
        {
            curAnim[1].SetBool("IsTurningLeft", false);
            curAnim[1].SetBool("IsTurningRight", false);
        }

    }
    void B_DashnHeight()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 dashPower = this.transform.forward * -Mathf.Log(1 / myRigid.drag) * 10f;
            // drag 공기저항값을 역수로 뒤집어서 로그로 바꾸고 - 를 넣어줘서 값을 구한 후 우리가 구한 대시양을 곱해준다 < 자연스러운 대시를 위해(무슨 소리인지 모르겠다.)
            myRigid.AddForce(dashPower, ForceMode.VelocityChange);
            Instantiate(orgDashEffect, this.transform.position, this.transform.rotation);
        }

        if (this.transform.position.y < B_restrictedHeight) // 높이제한
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Vector3 jumpPower = Vector3.up * B_AddFloatPower;
                myRigid.AddForce(jumpPower, ForceMode.VelocityChange);
            }
        }
    }

    /////////////////////////////////////Coroutine//////////////////////////////////////////////////////////
    IEnumerator SteppingBeforeNormAtk(RaycastHit hitData)
    {
        movePoint = hitData.point;
        myAgent.SetDestination(movePoint);

        while (myAgent.pathPending)
        {
            yield return null;
        }
        while (myAgent.remainingDistance > 0.1f)
        {
            if ((hitData.transform.position - transform.position).magnitude < 5.0f)
            {
                C_normAtk(hitData);
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator Stunned(float cool) // 못 움직이게 하는 스킬들
    {

        stun = true;
        while (cool > 0.0f)
        {
            state[0].text = "Stun";
            cool -= Time.deltaTime;
            yield return null;
        }
        stun = false;
        mySkill.canMove = true;
        mySkill.canSkill = true;
        state[0].text = "Idle";
    }

    IEnumerator C_Dash()
    {
        Vector3 dashPower = this.transform.forward * -Mathf.Log(1 / myRigid.drag) * 30.0f;
        myRigid.AddForce(dashPower, ForceMode.VelocityChange);
        Instantiate(orgDashEffect, this.transform.position + Vector3.forward, this.transform.rotation);
        yield return null;
    }

    IEnumerator StackingCoolStacks(float cool)
    {
        float cooltime = cool;
        while (coolStacks < 5)
        {
            cool -= Time.deltaTime;
            if (cool <= 0.0f && coolStacks <= 5)
            {
                coolStacks++;
                cool = cooltime;
                StackNumCheck(coolStacks, coolStacks - 1);
            }
            yield return null;
        }
    }
}
