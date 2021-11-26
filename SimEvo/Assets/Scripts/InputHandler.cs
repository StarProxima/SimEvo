using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    RuntimePlatform platform = Application.platform;

    

    
    [SerializeField] GameObject foodUI, shapeUI, settingsUI; 
     

    public void InputDropdown(int x)
    {
        foodUI.SetActive(false);
        shapeUI.SetActive(false);
        settingsUI.SetActive(false);
        if(x == 0)
        {
            foodUI.SetActive(true);
        }
        else if(x == 1)
        {
            shapeUI.SetActive(true);
        }
        else if(x == 2)
        {
            settingsUI.SetActive(true);
        }
    }

    public void InputFoodDisplay(bool b)
    {
        if(b)
            Camera.main.cullingMask|= 1 << LayerMask.NameToLayer("Food");
        else Camera.main.cullingMask&= ~(1 << LayerMask.NameToLayer("Food"));
    }
    public void InputShapeDisplay(bool b)
    {
        if(b)
            Camera.main.cullingMask|= 1 << LayerMask.NameToLayer("Shape");
        else Camera.main.cullingMask&= ~(1 << LayerMask.NameToLayer("Shape")); 
    }

    public void InputReprodaction(bool b)
    {
        GetComponent<Spawn>().shapeReprodaction = b; 
    }
    public void InputShapePerSec(string s)
    {
        if(s != null && s != "")
            GetComponent<Spawn>().shapePerSec = Convert.ToInt32(s); 
    }
    public void InputSpawnShape(bool b)
    {
        GetComponent<Spawn>().shapeSpawn = b; 
    }

    public void InputMaxShape(string s)
    {
        if(s != null && s != "")
            GetComponent<Spawn>().maxShapeCount = Convert.ToInt32(s); 
    }

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
        if(s != null && s != "")
            GetComponent<Spawn>().maxFoodCount = Convert.ToInt32(s);
    }
    public void InputFoodPerSec(string s)
    {
        if(s != null && s != "")
            GetComponent<Spawn>().foodPerSec = Convert.ToInt32(s);
    }
    public void InputGroupSize(string s)
    {
        if(s != null && s != "")
            GetComponent<Spawn>().foodGroupSize = Convert.ToInt32(s);
    }

    void Start()
    {
        
    }
    // public bool Pointer()
    // {
    //     if (Input.GetMouseButton(1))
    //         return true;

    //     return false;
    // }

    // public bool PointerDown()
    // {
    //     if (Input.GetMouseButtonDown(1))
    //             return true;
    //     return false;
    // }

    
    // public bool trakingButtonDown()
    // {
        
    //     if (Input.GetKeyDown(KeyCode.T))
    //         return true;
        
    //     return false;
    // }

    // public float GetDeltaY()
    // {   
        
    //     return Input.mouseScrollDelta.y; 

    // }
    // public Vector2 GetPosForZoom()
    // {

    //     return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    // }

    // public Vector2 GetPointerPos()
    // {
    //     return Camera.main.ScreenToWorldPoint(Input.mousePosition); 
    // }
}
