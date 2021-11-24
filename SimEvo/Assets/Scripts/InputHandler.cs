using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    RuntimePlatform platform = Application.platform;

    [SerializeField] GameObject MobileUI;

    
    
     

    
    public void SpawnFood(bool b)
    {
        GetComponent<Spawn>().foodSpawn = b;
        
    }
    public void InputRatio(float f)
    {
        GetComponent<Spawn>().ratioFood = f;
        
    }
    public void InputMaxFood(string s)
    {
        GetComponent<Spawn>().maxFoodCount = Convert.ToInt32(s);
    }
    public void InputFoodPerSec(string s)
    {
        GetComponent<Spawn>().foodPerSec = Convert.ToInt32(s);
    }
    public void InputGroupSize(string s)
    {
        GetComponent<Spawn>().foodGroupSize = Convert.ToInt32(s);
    }

    void Start()
    {
        
    }
    public bool Pointer()
    {
        if (Input.GetMouseButton(1))
            return true;

        return false;
    }

    public bool PointerDown()
    {
        if (Input.GetMouseButtonDown(1))
                return true;
        return false;
    }

    
    public bool trakingButtonDown()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
            return true;
        
        return false;
    }

    public float GetDeltaY()
    {   
        
        return Input.mouseScrollDelta.y; 

    }
    public Vector2 GetPosForZoom()
    {

        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Vector2 GetPointerPos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition); 
    }
}
