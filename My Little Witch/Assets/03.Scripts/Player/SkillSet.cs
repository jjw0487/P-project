using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SkillSet : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("Information")]
    public SkillData myData;
    public Image[] img;
    private Sprite orgSprite;

    [Header("Object")]
    private Skill fromSkill;
    private Transform myCharacter;
    private Camera mainCamera;
    private Vector3 SkillHitPoint;
    private bool inCoolTime;

    private void Start()
    {
        if (orgSprite == null) { orgSprite = img[0].sprite; }
        mainCamera = Camera.main;
        myCharacter = SceneData.Inst.myPlayer.transform;
        fromSkill = SceneData.Inst.mySkill;
        if (myData == null)
        {
            myData = null;
        }

        if (myData != null)
        {
            img[0].sprite = myData.sprite; // 나중에 스탯창 만들고 드랍때마다 함수 만들어서 호출되도록 해야함.
        }

    }

    /// ////////////////////////          UI            //////////////////////////////
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (myData.Type != SkillData.SkillType.NormalAttack) // 일반공격이 아니라면 마우스로 스킬사용
            {
                this.PerformSkill();
                if(this == fromSkill.skillSetArray[0]) { print("1"); fromSkill.myPlayer.curAnim[0].SetInteger("SkillNum", 0); }
                if(this == fromSkill.skillSetArray[1]) { fromSkill.myPlayer.curAnim[0].SetInteger("SkillNum", 0); }
                if(this == fromSkill.skillSetArray[2]) { fromSkill.myPlayer.curAnim[0].SetInteger("SkillNum", 0); }
                if(this == fromSkill.skillSetArray[3]) { fromSkill.myPlayer.curAnim[0].SetInteger("SkillNum", 0); }
            }  
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (myData.Type != SkillData.SkillType.NormalAttack) // 일반공격 슬롯은 항상 디폴트가 있어야 한다.
            {
                ClearSlot();
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (myData != null)
        {
            DragImage.Inst.dragSkillData = myData;
            DragImage.Inst.dragSkillSet = this;
            DragImage.Inst.DragSetSprite(myData.sprite);
            DragImage.Inst.transform.position = eventData.position;
            DragImage.Inst.fromBook = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (myData != null)
        {
            DragImage.Inst.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragImage.Inst.SetColor(0);
        DragImage.Inst.dragSkillSet = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //if (DragImage.Inst.dragSkillData != null) { ChangeSlot(); }
        if (DragImage.Inst.dragSkillData != null && DragImage.Inst.fromBook) { AddSkillData(DragImage.Inst.dragSkillData); }
        else if (DragImage.Inst.dragSkillData != null && !DragImage.Inst.fromBook) { ChangeSlot(); }
    }

    private void ChangeSlot()
    {
        SkillData temp = myData; // 아이템을 담을 공간을 만들고

        this.AddSkillData(DragImage.Inst.dragSkillData);

        if (temp != null)
        {
            DragImage.Inst.dragSkillSet.AddSkillData(temp);
        }
        else
        {
            DragImage.Inst.dragSkillSet.ClearSlot();
        }
    }

    private void ClearSlot()
    {
        myData = null;
        img[0].sprite = orgSprite;
    }

    ////////////////////////////          Skill          //////////////////////////////

    public void AddSkillData(SkillData data)
    {
        myData = data;
        img[0].sprite = myData.sprite;
    }

    public void PerformSkill()
    {
        if (!inCoolTime && fromSkill.myPlayer.CurMP > myData.consumeMP && !fromSkill.myPlayer.stun)
        {
            if (myData.Type == SkillData.SkillType.Buff) // 버프
            {
                if (myData.Action == SkillData.ActionType.WaitBeforeAction)
                {
                    StartCoroutine(WaitForThePerform(5f));
                }
                else
                {
                    Buff();
                    StartCoroutine(CoolTime(myData.coolTime[myData.level - 1]));
                }
            }
            else if (myData.Type == SkillData.SkillType.Attck) // 어택
            {
                if (myData.Action == SkillData.ActionType.WaitBeforeAction)
                {
                    StartCoroutine(WaitForThePerform(5f));
                }
                else
                {
                    //SkillAttackWithoutWaitMotion(); // 애님이벤트에서 실행
                    StartCoroutine(fromSkill.Chill(myData.remainTime));
                    fromSkill.myPlayer.curAnim[0].SetTrigger(myData.triggerName);
                    StartCoroutine(CoolTime(myData.coolTime[myData.level - 1]));
                }
            }
            else if (myData.Type == SkillData.SkillType.Debuff) // 디버프
            {
                if (myData.Action == SkillData.ActionType.WaitBeforeAction)
                {
                    StartCoroutine(WaitForThePerform(5f));
                }
                else
                {
                    StartCoroutine(CoolTime(myData.coolTime[myData.level - 1]));
                }

            }
            else //공격 N 디버프
            {
                if (myData.Action == SkillData.ActionType.WaitBeforeAction)
                {
                    StartCoroutine(WaitForThePerform_AND(5f));
                }
                else
                {
                    AND(); // 나중에 스킬 전부 AnimEvent로 바꿀거면 지우자
                    StartCoroutine(CoolTime(myData.coolTime[myData.level]));
                }
            }
        }
        else
        {
            // 마력이 부족합니다 텍스트
        }

    }

    public void Buff()
    {
        GameObject obj = Instantiate(myData.Effect, myCharacter.transform.position + myData.performPos, Quaternion.identity);
        fromSkill.myPlayer.curAnim[0].SetTrigger(myData.triggerName);
        fromSkill.myPlayer.HandleMP(myData.consumeMP,0f);
        if (fromSkill.myPlayer.handleSlider == null) { fromSkill.myPlayer.handleSlider = StartCoroutine(fromSkill.myPlayer.SliderValue()); }
        StartCoroutine(fromSkill.Chill(myData.remainTime));
    }

    public void SkillAttackWithoutWaitMotion() // 일반 스킬어택 <- 애님이벤트에서 실행 OnPlayerAtkWithoutWaitMotion()
    {
        if (myData.orientation == SkillData.Orientation.immediate)
        {
            fromSkill.myPlayer.HandleMP(myData.consumeMP, 0f);
            if (fromSkill.myPlayer.handleSlider == null) { fromSkill.myPlayer.handleSlider = StartCoroutine(fromSkill.myPlayer.SliderValue()); }
            GameObject obj = Instantiate(myData.Effect, myCharacter.transform.position + myData.performPos, Quaternion.identity);
            SkillOverlapColWithoutWaitMotion();
        }
        else if (myData.orientation == SkillData.Orientation.Remain)
        {
            fromSkill.myPlayer.HandleMP(myData.consumeMP, 0f);
            if (fromSkill.myPlayer.handleSlider == null) { fromSkill.myPlayer.handleSlider = StartCoroutine(fromSkill.myPlayer.SliderValue()); }
            GameObject obj = Instantiate(myData.Effect, myCharacter.transform.position + new Vector3(1, 0.5f, 0), Quaternion.identity);
            GameObject obj2 = Instantiate(myData.Effect, myCharacter.transform.position + new Vector3(-1, 0.5f, 0), Quaternion.identity);
            GameObject obj3 = Instantiate(myData.Effect, myCharacter.transform.position + new Vector3(0, 0.5f, 1), Quaternion.identity);
            GameObject obj4 = Instantiate(myData.Effect, myCharacter.transform.position + new Vector3(0, 0.5f, -1), Quaternion.identity);

            /*for (int i = 0; i < myData.level; ++i)
            {
                Vector3 performPos = myCharacter.transform.position + myData.performPos;
                GameObject obj = Instantiate(myData.Effect, performPos, Quaternion.identity);
            }*/

            StartCoroutine(fromSkill.Chill(myData.remainTime));
            //데미지는 이펙트 안에서 호출
        }
    }

    public void SkillAttack() // 어택 with Waiting Motion <- 애님이벤트에서 실행 OnPlayerAtkSkill()
    {
        if (myData.orientation == SkillData.Orientation.immediate)
        {
            fromSkill.myPlayer.HandleMP(myData.consumeMP, 0f);
            if (fromSkill.myPlayer.handleSlider == null) { fromSkill.myPlayer.handleSlider = StartCoroutine(fromSkill.myPlayer.SliderValue()); }
            GameObject obj = Instantiate(myData.Effect, SkillHitPoint, Quaternion.identity);
            StartCoroutine(fromSkill.Chill(myData.remainTime));
            SkillOverlapCol();
        }
        
            
    }

    public void AND() // 어택 N 디버프 with Waiting Motion <- 애님이벤트에서 실행 OnPlayerANDSkill()
    {
        fromSkill.myPlayer.HandleMP(myData.consumeMP, 0f);
        if (fromSkill.myPlayer.handleSlider == null) { fromSkill.myPlayer.handleSlider = StartCoroutine(fromSkill.myPlayer.SliderValue()); }
        GameObject obj = Instantiate(myData.Effect, SkillHitPoint, Quaternion.identity);
        StartCoroutine(fromSkill.Chill(myData.remainTime));
        SkillOverlapCol_AND();
    }

    public void SkillOverlapColWithoutWaitMotion() // 대기 모션 없는 
    {
        Collider[] hitColliders = Physics.OverlapSphere(myCharacter.transform.position, myData.overlapRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                /*Monster mon = col.GetComponentInParent<Monster>();
                if (mon == null) continue;*/   //나중에 nullref 나오면 예외처리 해줘야함.
                if (!col.GetComponentInParent<Monster>().isDead)
                {
                    col.GetComponentInParent<Monster>().OnDamage(myData.dmg[myData.level - 1]);
                }
            }
        }
    }

    public void SkillOverlapCol()
    {
        Collider[] hitColliders = Physics.OverlapSphere(SkillHitPoint, myData.overlapRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                /*Monster mon = col.GetComponentInParent<Monster>();
                if (mon == null) continue;*/   //나중에 nullref 나오면 예외처리 해줘야함.
                if (!col.GetComponentInParent<Monster>().isDead)
                {
                    col.GetComponentInParent<Monster>().OnDamage(myData.dmg[myData.level - 1]);
                }
            }
        }
    }

    public void SkillOverlapCol_AND()
    {
        Collider[] hitColliders = Physics.OverlapSphere(SkillHitPoint, myData.overlapRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                if (!col.GetComponentInParent<Monster>().isDead)
                {
                    col.GetComponentInParent<Monster>().OnDamage(myData.dmg[myData.level - 1]);
                    col.GetComponentInParent<Monster>().OnDebuff(myData.debuffTime[myData.level - 1], myData.percentage[myData.level - 1]);
                }
            }
        }
    }

    public IEnumerator WaitForThePerform(float cool)
    {
        fromSkill.canMove = false;
        fromSkill.canSkill = false;

        fromSkill.myPlayer.curAnim[0].SetTrigger("WaitForPos"); // 대기모션
        fromSkill.rangeOfSkills.SetActive(true); // 스킬 사정거리 표시 
        fromSkill.rangeOfSkills.transform.localScale = new Vector3(myData.rangeOfSkill, 0.01f, myData.rangeOfSkill); // 스킬 사정거리

        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;

            if (fromSkill.myPlayer.stun) // 얻어 맞으면
            {
                StartCoroutine(CoolTime(myData.coolTime[myData.level - 1]));
                fromSkill.rangeOfSkills.SetActive(false);
                yield break; //바로 종료
            }

            if (Input.GetMouseButtonDown(0)) // 대기 시간 내에 마우스 입력
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("SkillLimit")))
                {
                    fromSkill.myPlayer.curAnim[0].SetTrigger(myData.triggerName); //애니메이션
                    SkillHitPoint = myData.performPos + hitData.point; // 스킬힛포인트 변수를 만들고
                    myCharacter.transform.rotation = Quaternion.LookRotation((hitData.point - myCharacter.transform.position).normalized); // 시전 방향으로 쳐다보고
                    //SkillAttack(); // 애님이벤트에서 작동
                    fromSkill.rangeOfSkills.SetActive(false); // 사정거리 끄고
                    StartCoroutine(CoolTime(myData.coolTime[myData.level - 1])); //쿨타임
                    yield break;
                }
            }

            if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼으로 취소
            {
                //취소
                fromSkill.rangeOfSkills.SetActive(false);
                fromSkill.canMove = true;
                fromSkill.canSkill = true;
                fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
                yield break; //쿨타임 없이 종료
            }

            yield return null;
        }
        fromSkill.rangeOfSkills.SetActive(false);
        fromSkill.canMove = true; //시간이 끝나면
        fromSkill.canSkill = true;
        fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
    }

    public IEnumerator WaitForThePerform_AND(float cool) // 공격과 디버프
    {
        fromSkill.canMove = false;
        fromSkill.canSkill = false;
        fromSkill.myPlayer.curAnim[0].SetTrigger("WaitForPos"); // 대기모션
        fromSkill.rangeOfSkills.SetActive(true); // 스킬 사정거리    
        fromSkill.rangeOfSkills.transform.localScale = new Vector3(myData.rangeOfSkill, 0.01f, myData.rangeOfSkill); // 스킬 사정거리

        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;

            if (fromSkill.myPlayer.stun) // 얻어 맞으면
            {
                StartCoroutine(CoolTime(myData.coolTime[myData.level - 1]));
                fromSkill.rangeOfSkills.SetActive(false);
                yield break; // 바로 종료
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("SkillLimit"))) // 스킬 범위에 생긴 콜라이더
                {
                    fromSkill.myPlayer.curAnim[0].SetTrigger(myData.triggerName); // 애니메이션
                    SkillHitPoint = myData.performPos + hitData.point; // 스킬이 나갈 포지션 값을 받아두고
                    myCharacter.transform.rotation = Quaternion.LookRotation((hitData.point - myCharacter.transform.position).normalized); // 스킬 쏜 방향으로 쳐다보고
                    //AND(); //애님이벤트에서 작동
                    fromSkill.rangeOfSkills.SetActive(false); //사정거리
                    StartCoroutine(CoolTime(myData.coolTime[myData.level - 1])); //쿨타임
                    yield break;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                //취소
                fromSkill.rangeOfSkills.SetActive(false);
                fromSkill.canMove = true;
                fromSkill.canSkill = true;
                fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
                yield break;
            }
            yield return null;
        }
        fromSkill.rangeOfSkills.SetActive(false);
        fromSkill.canMove = true; //시간이 끝나면
        fromSkill.canSkill = true;
        fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
    }

    public IEnumerator CoolTime(float cool)
    {
        float coolTime = cool;
        inCoolTime = true; //트루를 주고
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            img[1].fillAmount = 1f * (cool / coolTime);
            yield return null;
        }
        inCoolTime = false; //시간이 끝나면
    }


}
