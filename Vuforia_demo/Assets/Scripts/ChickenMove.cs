using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMove : MonoBehaviour
{
    private bool isTargetSet = false;
    private Vector3 targetPos; // 目标位置

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {

        animator = this.GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
       
        if(isTargetSet)
        {
            // 动画相关控制
            animator.SetTrigger("walk");
            MoveToFood(targetPos);
            // 是否到达目的地
            if ((transform.position - targetPos).magnitude < 0.01)
            {
                animator.ResetTrigger("walk");
                animator.SetTrigger("arrive");
                isTargetSet = false;
                Invoke(nameof(RecordValue), 4);                
            }
        }  
         
    }
    void MoveToFood(Vector3 targetPos)
    {
        if (transform.position.y > 0.1f)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.02f);
    }
    void RecordValue()
    {
        Debug.Log("enter record value");
        if (!PlayerPrefs.HasKey("chicken_value"))
        {
            PlayerPrefs.SetInt("chicken_value", 100);
        } else
        {
            PlayerPrefs.SetInt("chicken_value", PlayerPrefs.GetInt("chicken_value") + 20);
        }
        transform.localScale = (float)PlayerPrefs.GetInt("chicken_value") / 100 * new Vector3(1, 1, 1);
        GameObject food = GameObject.Find("food");
        Destroy(food);
    }

    // 供外部调用函数，设置目标位置
    public void SetMoveTarget(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        this.isTargetSet = true;

        // 旋转方向

        transform.LookAt(targetPos);
       
    }
}
