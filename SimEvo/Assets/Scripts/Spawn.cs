using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject circle;
    public bool circleSpawn = true;
    public int maxCircleCount = 10000;
    public int circleCount = 0;
    public float circleAreaWidth = 1000f;
    public float circleAreaHeight = 600f;
    public float circleDeltaTime = 1f;

    public GameObject food;
    public bool foodSpawn = true;
    public int maxFoodCount = 10000;
    public int foodCount = 0;
    public float foodAreaWidth = 1000f;
    public float foodAreaHeight = 600f;
    public float foodDeltaTime = 0.1f;

    [SerializeField] float foodGroupSize = 10;
    float circleTime = 0;
    float foodTime = 0;
    void Start()
    {
        //StartCoroutine(SpawnFoodProcess());
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
        if(circleCount < maxCircleCount && circleSpawn)
        {
            circleTime += Time.deltaTime;
            if(circleTime > circleDeltaTime)
            {
                GameObject t = Instantiate(circle, new Vector3(Random.Range(-foodAreaWidth/2, foodAreaWidth/2), Random.Range(-foodAreaHeight/2, foodAreaHeight/2), 100), Quaternion.identity);
                t.GetComponent<Circle>().Initialization();
                circleCount++;
                circleTime = 0;
            }
        } 

        if(foodCount < maxFoodCount && foodSpawn)
        {
            foodTime += Time.deltaTime;
            if(foodTime > foodDeltaTime)
            {
                for(int i = 0; i < foodGroupSize; i++)
                {
                    Instantiate(food, new Vector3(Random.Range(-foodAreaWidth/2, foodAreaWidth/2), Random.Range(-foodAreaHeight/2, foodAreaHeight/2), 100), Quaternion.Euler(0,0,Random.Range(0,360)));
                    foodCount++;
                }
                foodTime = 0;
            }
        }
    }
}
