using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterControl : MonoBehaviour
{
    public Camera playerCamera;

    float m_CameraVerticalAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        

    }
    


    // Update is called once per frame
    void Update()
    {
        CameraRotateUpdate();
        FireUpdate();
    }

    void CameraRotateUpdate()
    {
        transform.Rotate(new Vector3(0f, Input.GetAxisRaw("Mouse X"), 0f), Space.Self);
        m_CameraVerticalAngle -= Input.GetAxisRaw("Mouse Y");

        // limit the camera's vertical angle to min/max
        m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);

        // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
        playerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);
    }

    void FireUpdate()
    {

    }
}
