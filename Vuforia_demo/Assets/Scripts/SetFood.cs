using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFood : MonoBehaviour
{
    // Start is called before the first frame update
    
    private GameObject cube;
    public Transform parent;
    public Transform createCube;
    void Start()
    {
        cube = Resources.Load("Prefabs/Cube") as GameObject;
    }
    public void OnSetFoodClick()
    {
        //GameObject ob = new GameObject("food");
        

        GameObject obj = Instantiate(cube);
        obj.name = "food";
        Vector3 loc = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
        obj.transform.position = createCube.transform.TransformPoint(loc);

        GameObject chicken = GameObject.Find("longChicken(Clone)");
        ChickenMove script = (ChickenMove)chicken.GetComponent("ChickenMove");
        script.SetMoveTarget(loc);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
