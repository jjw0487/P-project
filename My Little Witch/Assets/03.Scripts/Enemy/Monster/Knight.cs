using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Knight : Monster
{
    public void Hit()
    {
        myEnemy.OnDmg(10f);
    }
}
