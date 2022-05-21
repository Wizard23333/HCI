using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickRotate : MonoBehaviour
{
    // x方向的速度
    private float speed_x = 150f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 实现旋转
        if (Input.GetMouseButton(0))
        {
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * -speed_x * Time.deltaTime, Space.World);
                }
            }
        }

    }
}
