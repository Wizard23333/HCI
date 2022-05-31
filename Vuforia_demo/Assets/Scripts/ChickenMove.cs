using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        
    }
    private float startTime;
    private bool isTargetSet = false;
    private Vector3 targetPos;

    // Update is called once per frame
    void Update()
    {
        //Vector3 targetPos = new Vector3(-2, 0, -1);

        if(isTargetSet)
        {
            MoveToFood(targetPos);
        }
        
        
    }
    void MoveToFood(Vector3 targetPos)
    {
        if (transform.position.y > 0.1f)
        {
            return;
        }

        transform.position = Vector3.Slerp(transform.position, targetPos, 0.03f);
    }

    public void SetMoveTarget(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        this.isTargetSet = true;
    }
}
