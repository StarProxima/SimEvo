using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    GameObject circle;
    public bool shapeSpawn = false;
    public int maxShapeCount = 1000;
    public int circleCount = 0;
    public float circleAreaWidth = 1000f;
    public float circleAreaHeight = 600f;
    
    public float shapePerSec = 10f; 

    GameObject food;
    public bool foodSpawn = true;
    public int maxFoodCount = 10000;
    public int foodCount = 0;
    public float foodAreaWidth = 1000f;
    public float foodAreaHeight = 600f;
    public float foodPerSec = 10;

    public float ratioFood = 0f;

    [SerializeField] bool foodGroupSpawn = false;
    public int foodGroupSize = 10;
    float shapeTime = 0;
    float foodTime = 0;
    Vector3 randPos;
    void Start()
    {
        circle = (GameObject)Resources.Load("Circle", typeof(GameObject));
        food = (GameObject)Resources.Load("Food", typeof(GameObject));
    }

    // IEnumerator SpawnFoodProcess()
    // {
    //     while(true)
    //     {
    //         if(count < max)
    //         {
    //             for(int i = 0; i < k; i++)
    //             {
    //                 Instantiate(food, new Vector3(Random.Range(-width/2, width/2), Random.Range(-height/2, height/2), 100), Quaternion.Euler(0,0,Random.Range(0,360)));
    //             }
                
    //             count+=k;
    //         }
            
    //         yield return new WaitForSeconds(0.01f);
    //     }
    // }
    
    
    void Update()
    {
        if(circleCount < maxShapeCount && shapeSpawn)
        {
            shapeTime += Time.deltaTime;
            if(shapeTime > 1f/shapePerSec)
            {
                GameObject t = Instantiate(circle, new Vector3(Random.Range(-foodAreaWidth/2, foodAreaWidth/2), Random.Range(-foodAreaHeight/2, foodAreaHeight/2), 100), Quaternion.identity);
                t.GetComponent<Circle>().Initialization();
                circleCount++;
                shapeTime = 0;
            }
        } 

        if(foodCount < maxFoodCount && foodSpawn)
        {
            foodTime += Time.deltaTime;
            if(foodTime > foodGroupSize/foodPerSec)
            {   
                if(Random.value > ratioFood)
                {
                    for(int i = 0; i < foodGroupSize; i++)
                    {
                        Instantiate(food, new Vector3(Random.Range(-foodAreaWidth/2, foodAreaWidth/2), Random.Range(-foodAreaHeight/2, foodAreaHeight/2), 100), Quaternion.Euler(0,0,Random.Range(0,360)));
                    }
                }
                else
                {
                    randPos = new Vector3(Random.Range(-foodAreaWidth/2, foodAreaWidth/2), Random.Range(-foodAreaHeight/2, foodAreaHeight/2), 100);
                    for(int i = 0; i < foodGroupSize; i++)
                    {
                        randPos = randPos + new Vector3(Random.Range(-Mathf.Log(foodGroupSize,2), Mathf.Log(foodGroupSize,2)),Random.Range(-Mathf.Log(foodGroupSize,2), Mathf.Log(foodGroupSize,2)),0);
                        Instantiate(food, randPos, Quaternion.Euler(0,0,Random.Range(0,360)));
                    }
                } 
                //if(foodGroupSpawn) randPos = new Vector3(Random.Range(-foodAreaWidth/2, foodAreaWidth/2), Random.Range(-foodAreaHeight/2, foodAreaHeight/2), 100);
                    
                foodCount+=foodGroupSize;
                foodTime = 0;
            }
        }
    }
}
