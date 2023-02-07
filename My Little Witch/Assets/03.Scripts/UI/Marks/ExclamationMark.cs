using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExclamationMark : MarkController
{  
    private void Start()
    {
        Destroy(this.gameObject, 2f);
    }
}
