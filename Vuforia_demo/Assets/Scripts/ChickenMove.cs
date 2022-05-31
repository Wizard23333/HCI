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
            if ((transform.position - targetPos).magnitude < 0.1)
            {
                animator.ResetTrigger("walk");
                animator.SetTrigger("arrive");
                isTargetSet = false;
               
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

    // 供外部调用函数，设置目标位置
    public void SetMoveTarget(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        this.isTargetSet = true;

        // 旋转方向

        transform.LookAt(targetPos);
       
    }
}
