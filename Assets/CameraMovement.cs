using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    public Camera thisCamera;
    public float maxX, maxY;
    public float minSize, maxSize, scrollSpeed;
    Vector3 Origin, Difference;
    bool drag;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Center();
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && thisCamera.orthographicSize > minSize)
            thisCamera.orthographicSize -= (3f + thisCamera.orthographicSize) * scrollSpeed * Time.deltaTime;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && thisCamera.orthographicSize < maxSize)
            thisCamera.orthographicSize += (3f + thisCamera.orthographicSize) * scrollSpeed * Time.deltaTime;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            Difference = (thisCamera.ScreenToWorldPoint(Input.mousePosition)) - thisCamera.transform.position;
            if (!drag)
            {
                drag = true;
                Origin = thisCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else drag = false;

        if (drag)
        {
            thisCamera.transform.position = Origin - Difference;
            thisCamera.transform.position = new Vector3(
            Mathf.Clamp(thisCamera.transform.position.x, -maxX, maxX),
            Mathf.Clamp(thisCamera.transform.position.y, -maxY, maxY), transform.position.z);
            //Mathf.Clamp(Camera.main.transform.position.x, (2.5f + maxX) / (1f + thisCamera.orthographicSize), maxXP / (1f + thisCamera.orthographicSize)),
            //Mathf.Clamp(Camera.main.transform.position.y, maxYM / (1f + thisCamera.orthographicSize), maxYP / (1f + thisCamera.orthographicSize)), transform.position.z);
        }
    }

    public void Center()
    {
        transform.position = new Vector3(0f, 0f, -10f);
    }
}
