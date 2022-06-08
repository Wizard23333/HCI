using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMove : MonoBehaviour
{
    private bool isTargetSet = false;
    private Vector3 targetPos; // 目标位置
    private int moveMod = 0; 

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {

        animator = this.GetComponent<Animator>();
       
    }
    private void FixedUpdate()
    {
        if(isTargetSet)
        {
            if (moveMod == 1 && transform.position.y < targetPos.y)
            {
                this.GetComponent<Rigidbody>().AddForce(-Physics.gravity * 1.1f);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if(isTargetSet)
        {
            
            // 做走路运动
            if (moveMod == 0)
            {
                // 动画相关控制
                animator.SetTrigger("walk");
                MoveToTarget(targetPos);
                // 是否到达目的地
                if ((transform.position - targetPos).magnitude < 0.1)
                {
                    animator.ResetTrigger("walk");
                    animator.SetTrigger("arrive");
                    isTargetSet = false;
                    Invoke(nameof(EatFood), 4);
                }
            }
            // 做飞翔运动
            else if (moveMod == 1)
            {
                animator.SetBool("fly", true);
                MoveToTarget(targetPos);

                if ((transform.position - targetPos).magnitude < 0.01)
                {
                    animator.SetBool("fly", false);
                    animator.SetBool("stopFly", true);
                    isTargetSet = false;

                    moveMod = 0;
                }

            }
            
        }  
         
    }
    private void MoveToTarget(Vector3 targetPos)
    {
        

        transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.02f);
    }
    private void EatFood()
    {
        Debug.Log("enter record value");
        // 记录鸡的数值
        if (!PlayerPrefs.HasKey("chicken_value"))
        {
            PlayerPrefs.SetInt("chicken_value", 100);
        } else
        {
            PlayerPrefs.SetInt("chicken_value", PlayerPrefs.GetInt("chicken_value") + 20);
        }

        transform.localScale += (0.2f) * new Vector3(1, 1, 1);
        GameObject food = GameObject.Find("food");
        Destroy(food);

        if(PlayerPrefs.GetInt("chicken_value") == 200)
        {
            GameObject.Find("ImageTarget");
            MyDefaultTrackableEventHandler script = (MyDefaultTrackableEventHandler)GameObject.Find("ImageTarget").GetComponent("MyDefaultTrackableEventHandler");
            script.ReloadModel();
        }

    }

    // 调整传入的目标食物的位置
    private Vector3 EatTargetPositionAdjuster(Vector3 targetPosition)
    {
        targetPosition.y = 0;
        Vector3 adjustedPos = new Vector3();
        // 修正的参数值
        Vector3 deltaVector3 = (targetPosition - transform.position).normalized * 0.20f / ((float)PlayerPrefs.GetInt("chicken_value") / 100);
        adjustedPos = targetPosition - deltaVector3;

        return adjustedPos;
    }


    // 供外部调用函数，设置目标位置
    public void MoveAndEatTarget(Vector3 targetPos)
    {
        this.targetPos = EatTargetPositionAdjuster(targetPos);
        this.isTargetSet = true;
        this.moveMod = 0;

        // 旋转鸡的方向

        

        transform.LookAt(this.targetPos);
       
    }

    // 供外部调用，仅当大鸡时使用
    public void FlyToTarget(Vector3 targetPos)
    {
        // 还是小鸡的时候只能走过去
        if (PlayerPrefs.GetInt("chicken_value") < 200)
        {
            MoveAndEatTarget(targetPos);
            return;
        }
        Vector3 lookAtPos = targetPos;
        lookAtPos.y = 0;
        transform.LookAt(lookAtPos);

        this.targetPos = targetPos;
        this.isTargetSet = true;
        this.moveMod = 1;

        
    }


}
