using UnityEngine;
using UnityEngine.Events; // 유니티 이벤트와 액션 기능을 사용 



public class PracticeDelegate : MonoBehaviour
{

    public UnityEvent onInputSpace;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.Space))
        {
            onInputSpace.Invoke();
        }*/
    }
}
