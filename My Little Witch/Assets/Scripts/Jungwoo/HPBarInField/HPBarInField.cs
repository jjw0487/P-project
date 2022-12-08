//작성자 : 전정우
//설명 :

using UnityEngine;
using UnityEngine.UI;

public class HPBarInField : MonoBehaviour
{
    [Header("HP BARS")] public Slider KongHP;
    public Slider JinHP;
    public Slider EmberHP;

    public enum MYSTATE
    {
        Alive,
        Death
    }

    public MYSTATE state = MYSTATE.Alive;

    void ChangeState(MYSTATE myState)
    {
        if (state == myState) return;
        state = myState;
        switch (state)
        {
            case MYSTATE.Alive:
                if (KongHP.value <= 0.0f)
                {
                    ChangeState(MYSTATE.Death);
                }

                if (JinHP.value <= 0.0f)
                {
                    ChangeState(MYSTATE.Death);
                }

                if (EmberHP.value <= 0.0f)
                {
                    ChangeState(MYSTATE.Death);
                }

                break;
            case MYSTATE.Death:

                break;
        }
    }

    void StateProcess()
    {
        switch (state)
        {
            case MYSTATE.Alive:

                break;
            case MYSTATE.Death:
                break;
        }
    }
}