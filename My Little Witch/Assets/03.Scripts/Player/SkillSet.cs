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
            img[0].sprite = myData.sprite; // ���߿� ����â ����� ��������� �Լ� ���� ȣ��ǵ��� �ؾ���.
        }

    }

    /// ////////////////////////          UI            //////////////////////////////
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (myData.Type != SkillData.SkillType.NormalAttack) // �Ϲݰ����� �ƴ϶�� ���콺�� ��ų���
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
            if (myData.Type != SkillData.SkillType.NormalAttack) // �Ϲݰ��� ������ �׻� ����Ʈ�� �־�� �Ѵ�.
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
        SkillData temp = myData; // �������� ���� ������ �����

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
            if (myData.Type == SkillData.SkillType.Buff) // ����
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
            else if (myData.Type == SkillData.SkillType.Attck) // ����
            {
                if (myData.Action == SkillData.ActionType.WaitBeforeAction)
                {
                    StartCoroutine(WaitForThePerform(5f));
                }
                else
                {
                    //SkillAttackWithoutWaitMotion(); // �ִ��̺�Ʈ���� ����
                    StartCoroutine(fromSkill.Chill(myData.remainTime));
                    fromSkill.myPlayer.curAnim[0].SetTrigger(myData.triggerName);
                    StartCoroutine(CoolTime(myData.coolTime[myData.level - 1]));
                }
            }
            else if (myData.Type == SkillData.SkillType.Debuff) // �����
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
            else //���� N �����
            {
                if (myData.Action == SkillData.ActionType.WaitBeforeAction)
                {
                    StartCoroutine(WaitForThePerform_AND(5f));
                }
                else
                {
                    AND(); // ���߿� ��ų ���� AnimEvent�� �ٲܰŸ� ������
                    StartCoroutine(CoolTime(myData.coolTime[myData.level]));
                }
            }
        }
        else
        {
            // ������ �����մϴ� �ؽ�Ʈ
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

    public void SkillAttackWithoutWaitMotion() // �Ϲ� ��ų���� <- �ִ��̺�Ʈ���� ���� OnPlayerAtkWithoutWaitMotion()
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
            //�������� ����Ʈ �ȿ��� ȣ��
        }
    }

    public void SkillAttack() // ���� with Waiting Motion <- �ִ��̺�Ʈ���� ���� OnPlayerAtkSkill()
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

    public void AND() // ���� N ����� with Waiting Motion <- �ִ��̺�Ʈ���� ���� OnPlayerANDSkill()
    {
        fromSkill.myPlayer.HandleMP(myData.consumeMP, 0f);
        if (fromSkill.myPlayer.handleSlider == null) { fromSkill.myPlayer.handleSlider = StartCoroutine(fromSkill.myPlayer.SliderValue()); }
        GameObject obj = Instantiate(myData.Effect, SkillHitPoint, Quaternion.identity);
        StartCoroutine(fromSkill.Chill(myData.remainTime));
        SkillOverlapCol_AND();
    }

    public void SkillOverlapColWithoutWaitMotion() // ��� ��� ���� 
    {
        Collider[] hitColliders = Physics.OverlapSphere(myCharacter.transform.position, myData.overlapRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                /*Monster mon = col.GetComponentInParent<Monster>();
                if (mon == null) continue;*/   //���߿� nullref ������ ����ó�� �������.
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
                if (mon == null) continue;*/   //���߿� nullref ������ ����ó�� �������.
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

        fromSkill.myPlayer.curAnim[0].SetTrigger("WaitForPos"); // �����
        fromSkill.rangeOfSkills.SetActive(true); // ��ų �����Ÿ� ǥ�� 
        fromSkill.rangeOfSkills.transform.localScale = new Vector3(myData.rangeOfSkill, 0.01f, myData.rangeOfSkill); // ��ų �����Ÿ�

        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;

            if (fromSkill.myPlayer.stun) // ��� ������
            {
                StartCoroutine(CoolTime(myData.coolTime[myData.level - 1]));
                fromSkill.rangeOfSkills.SetActive(false);
                yield break; //�ٷ� ����
            }

            if (Input.GetMouseButtonDown(0)) // ��� �ð� ���� ���콺 �Է�
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("SkillLimit")))
                {
                    fromSkill.myPlayer.curAnim[0].SetTrigger(myData.triggerName); //�ִϸ��̼�
                    SkillHitPoint = myData.performPos + hitData.point; // ��ų������Ʈ ������ �����
                    myCharacter.transform.rotation = Quaternion.LookRotation((hitData.point - myCharacter.transform.position).normalized); // ���� �������� �Ĵٺ���
                    //SkillAttack(); // �ִ��̺�Ʈ���� �۵�
                    fromSkill.rangeOfSkills.SetActive(false); // �����Ÿ� ����
                    StartCoroutine(CoolTime(myData.coolTime[myData.level - 1])); //��Ÿ��
                    yield break;
                }
            }

            if (Input.GetMouseButtonDown(1)) // ���콺 ������ ��ư���� ���
            {
                //���
                fromSkill.rangeOfSkills.SetActive(false);
                fromSkill.canMove = true;
                fromSkill.canSkill = true;
                fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
                yield break; //��Ÿ�� ���� ����
            }

            yield return null;
        }
        fromSkill.rangeOfSkills.SetActive(false);
        fromSkill.canMove = true; //�ð��� ������
        fromSkill.canSkill = true;
        fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
    }

    public IEnumerator WaitForThePerform_AND(float cool) // ���ݰ� �����
    {
        fromSkill.canMove = false;
        fromSkill.canSkill = false;
        fromSkill.myPlayer.curAnim[0].SetTrigger("WaitForPos"); // �����
        fromSkill.rangeOfSkills.SetActive(true); // ��ų �����Ÿ�    
        fromSkill.rangeOfSkills.transform.localScale = new Vector3(myData.rangeOfSkill, 0.01f, myData.rangeOfSkill); // ��ų �����Ÿ�

        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;

            if (fromSkill.myPlayer.stun) // ��� ������
            {
                StartCoroutine(CoolTime(myData.coolTime[myData.level - 1]));
                fromSkill.rangeOfSkills.SetActive(false);
                yield break; // �ٷ� ����
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("SkillLimit"))) // ��ų ������ ���� �ݶ��̴�
                {
                    fromSkill.myPlayer.curAnim[0].SetTrigger(myData.triggerName); // �ִϸ��̼�
                    SkillHitPoint = myData.performPos + hitData.point; // ��ų�� ���� ������ ���� �޾Ƶΰ�
                    myCharacter.transform.rotation = Quaternion.LookRotation((hitData.point - myCharacter.transform.position).normalized); // ��ų �� �������� �Ĵٺ���
                    //AND(); //�ִ��̺�Ʈ���� �۵�
                    fromSkill.rangeOfSkills.SetActive(false); //�����Ÿ�
                    StartCoroutine(CoolTime(myData.coolTime[myData.level - 1])); //��Ÿ��
                    yield break;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                //���
                fromSkill.rangeOfSkills.SetActive(false);
                fromSkill.canMove = true;
                fromSkill.canSkill = true;
                fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
                yield break;
            }
            yield return null;
        }
        fromSkill.rangeOfSkills.SetActive(false);
        fromSkill.canMove = true; //�ð��� ������
        fromSkill.canSkill = true;
        fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
    }

    public IEnumerator CoolTime(float cool)
    {
        float coolTime = cool;
        inCoolTime = true; //Ʈ�縦 �ְ�
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            img[1].fillAmount = 1f * (cool / coolTime);
            yield return null;
        }
        inCoolTime = false; //�ð��� ������
    }


}
