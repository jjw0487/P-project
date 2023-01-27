using UnityEngine;
using UnityEngine.AI;

public class CharacterProperty : MonoBehaviour
{
    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
            }
            return _anim;
        }
    }

    Rigidbody _rigid = null;
    protected Rigidbody myRigid
    {
        get
        {
            if (_rigid == null)
            {
                _rigid = GetComponent<Rigidbody>();
            }
            return _rigid;
        }
    }

    NavMeshAgent _NavAgent = null;
    protected NavMeshAgent myAgent
    {
        get
        {
            if (_NavAgent == null)
            {
                _NavAgent = GetComponent<NavMeshAgent>();
            }
            return _NavAgent;
        }
    }

}
