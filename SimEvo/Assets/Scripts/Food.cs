using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    bool notEaten = true;

    float foodValue = 10;
    public float Eat()
    {
        if(notEaten)
        {
            notEaten = false;
            Destroy(gameObject);
            return foodValue;
        }
        return 0;
    }

}
