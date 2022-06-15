using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFood : MonoBehaviour
{
    // Start is called before the first frame update
    
    private GameObject cube;
    public Transform parent;
    public Transform createCube;
    public string createFoodName;
    public int createFoodNumber;
    public class FoodName
    {
        public string foodname;
        public int foodnumber;
        public FoodName(string a,int b)
        {
            foodname = a;
            foodnumber = b;
        }
    };
    FoodName[] foodNames = {
            new FoodName("Banana",1), new FoodName("Cheese",2) , new FoodName("Cherry",3) , new FoodName("Hamburger",4) ,
        new FoodName("Hotdog",5),new FoodName("Olive",6),new FoodName("Watermelon",7) };
    void Start()
    {
        
    }
    public void OnSetFoodClick()
    {
        //GameObject ob = new GameObject("food");
        int tnumber = Random.Range(0, 7);
        createFoodName = foodNames[tnumber].foodname;
        createFoodNumber = foodNames[tnumber].foodnumber;
        cube = Resources.Load("Mg3D_Food/" + createFoodName) as GameObject;
        GameObject obj = Instantiate(cube);
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.name = "food";
        Vector3 loc = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
        obj.transform.position = createCube.transform.TransformPoint(loc);

        GameObject chicken = GameObject.Find("longChicken(Clone)");
        ChickenMove script = (ChickenMove)chicken.GetComponent("ChickenMove");
        script.MoveAndEatTarget(loc);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
