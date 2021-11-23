using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class SpawnCircles : MonoBehaviour
{
    public GameObject circle;
    public GameObject food;
    Rigidbody2D[,] rigidBodys;

    public float randomRange = 0.5f;
    public int x;
    public int y;
    float t = 1000;
    void Start()
    {
        //Time.timeScale = 0.1f;
        rigidBodys = new Rigidbody2D[150,150];
        for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
            {
                GameObject t = Instantiate(circle, new Vector3(4*i, 4*j, 0), Quaternion.identity);
                rigidBodys[i,j] = t.GetComponent<Rigidbody2D>();
            }
    }

    // void Update()
    // {
    //     t += Time.deltaTime;
    //     if(t > 30)
    //     {
    //         for (int i =0; i < x; i++)
    //             for (int j =0; j < y; j++)
    //             {
    //                 rigidBodys[i,j].gameObject.GetComponent<Circle>().targetPos = (new Vector2(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange)));
                    
    //             }
    //         t = 0;
    //     }
        
    // }
}
