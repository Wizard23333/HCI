using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTap : MonoBehaviour
{
    private float touchTime;
    private bool newTouch = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 从摄像机发出射线将接触到的物体消除
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // 判断是不是双击
                //if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
                //{

                //    if (Input.GetTouch(0).tapCount == 2)
                //    {
                //        Destroy(hit.collider.gameObject);
                //    }
                //}
                // 判断是不是长按
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        newTouch = true;
                        touchTime = Time.time;
                    }
                    else if (touch.phase == TouchPhase.Stationary)
                    {
                        if (newTouch == true && Time.time - touchTime > 1f)
                        {
                            newTouch = false;
                            Destroy(hit.collider.gameObject);
                        }
                    }
                    else
                    {
                        newTouch = false;
                    }
                }
            }
        }

    }
}
