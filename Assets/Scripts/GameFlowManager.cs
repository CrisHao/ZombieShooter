using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    static public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
