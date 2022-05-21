using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enlarge : MonoBehaviour
{
    Vector2 oldPos1;
    Vector2 oldPos2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 2)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                Vector2 temPos1 = Input.GetTouch(0).position;
                Vector2 temPos2 = Input.GetTouch(1).position;

                if (isEnlarge(oldPos1, oldPos2, temPos1, temPos2))
                {
                    float newScale = transform.localScale.x * 1.025f;
                    transform.localScale = new Vector3(newScale, newScale, newScale);
                } else
                {
                    float newScale = transform.localScale.x / 1.025f;
                    transform.localScale = new Vector3(newScale, newScale, newScale);
                }
                oldPos1 = temPos1;
                oldPos2 = temPos2;
            }
            
        }
    }

    // 判断两个点之间的距离是变大还是变小
    private bool isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        float length1 = Mathf.Sqrt(Mathf.Pow(oP1.x - oP2.x, 2) + Mathf.Pow(oP1.y - oP2.y, 2));
        float length2 = Mathf.Sqrt(Mathf.Pow(nP1.x - nP2.x, 2) + Mathf.Pow(nP1.y - nP2.y, 2));

        return length1 < length2;
    }
}
