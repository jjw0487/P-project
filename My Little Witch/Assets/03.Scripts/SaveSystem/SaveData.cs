using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class SaveData
{
    //player
    public int level;
    public float hp;
    public float[] position; //�÷��̾� ������ ��ġ

    //inventory
    public List<Item> items = new List<Item>();

    public SaveData(Player player)
    {
        #region �����ڸ� �����ε� �� ����
        //�����ڴ� Ŭ������ ��ü�� �����ϴ� ������ �Ѵ�.
        //Ŭ������ ������ �� �����ڸ� �������� �ʾƵ� �����Ϸ��� �ڵ����� ���������� �׷����� �����ڸ� ���� �����ϴ� ������
        //��ü�� �ʵ带 ���ϴ� ������ �ʱ�ȭ�Ϸ��� �� �� ������ ��Ұ� �������̱� ����
        #endregion

        level = player.Level;
        hp = player.CurHP;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        //items = 




    }
}
