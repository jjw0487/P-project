//작성자 : 전정우
//설명 : 

using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterDescription : MonoBehaviour
{
    public GameObject myDescription;
    public GameObject myCharacter;

    public enum DESCRIPTION
    {
        On,
        Off
    }

    public DESCRIPTION description = DESCRIPTION.Off;

    void ChangeState(DESCRIPTION state)
    {
        if (description == state) return;
        description = state;
        switch (description)
        {
            case DESCRIPTION.On:
                break;
            case DESCRIPTION.Off:

                break;
        }
    }

    void StateProcess()
    {
        switch (description)
        {
            case DESCRIPTION.On:
                break;
            case DESCRIPTION.Off:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(DESCRIPTION.Off);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();

        Debug.Log(EventSystem.current.IsPointerOverGameObject());

        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            ChangeState(DESCRIPTION.On);
        }
        else
        {
            ChangeState(DESCRIPTION.Off);
        }
    }
}