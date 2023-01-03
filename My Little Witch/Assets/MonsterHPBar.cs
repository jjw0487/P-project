using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHPBar : MonoBehaviour
{
    [SerializeField] GameObject hpBarPrefab;

    List<Transform> m_ObjList = new List<Transform>();
    List<GameObject> m_hpBarList = new List<GameObject>();

    public Camera m_Cam;

    void Start()
    {

        GameObject[] m_tags = GameObject.FindGameObjectsWithTag("Monster");
        for (int i = 0; i < m_tags.Length; i++)
        {
            m_ObjList.Add(m_tags[i].transform);
            GameObject m_hpBar = Instantiate(hpBarPrefab, m_ObjList[i].transform.position, Quaternion.identity);
            m_hpBarList.Add(m_hpBar);

            print(m_hpBar.transform.position);
        }

    }

    void Update()
    {
        if (m_ObjList != null)
        {
            for (int i = 0; i < m_ObjList.Count; i++)
            {
                
                m_hpBarList[i].transform.position = m_Cam.WorldToScreenPoint(m_ObjList[i].transform.position + Vector3.up);

                if (m_ObjList == null)
                {
                    m_hpBarList.Clear();
                }
            }
        }
    }
}
