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
    public float[] position; //플레이어 마지막 위치

    //inventory
    public List<Item> items = new List<Item>();

    public SaveData(Player player)
    {
        #region 생성자를 오버로딩 한 이유
        //생성자는 클래스의 객체를 생성하는 역할을 한다.
        //클래스를 선언할 때 생성자를 구현하지 않아도 컴파일러가 자동으로 생성하지만 그럼에도 생성자를 직접 구현하는 이유는
        //객체의 필드를 원하는 값으로 초기화하려고 할 때 적합한 장소가 생성자이기 때문
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
