using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Circle: MonoBehaviour {

    [SerializeField] GameObject manager;
    NeuralNetwork neural;

    //Parametrs
    [SerializeField] float energy = 20;
    float maxEnergy = 100;
    float maxSpeed = 10;
    float maxRotateSpeed = 25;
    [SerializeField] bool energyDeath = true;

    
    //Draw
    int vertexCount = 16;
    int vertexCountEye = 24;
    int eyeLenght = 4;
    float radius = 0.5f;
    float width = -1f;

    
    Collider2D[] nearestColliders;

    Rigidbody2D rb;
    GameObject circle;

    Spawn spawn;
    float time = 0;
    Shape shape;
    public void Initialization(NeuralNetwork neural = null, float time = 0)
    {
        shape = new Shape(gameObject, maxSpeed, maxRotateSpeed);
        shape.DrawFigure(vertexCount, vertexCountEye, eyeLenght, radius, width);
        
        circle = (GameObject)Resources.Load("Circle", typeof(GameObject));
        manager = Camera.main.GetComponent<CamControl>().manager;
        spawn = manager.GetComponent<Spawn>();
        
        
        //DrawCircle();
        if(neural != null)
            this.neural = new NeuralNetwork(neural, 0.5f);
        else
            this.neural = new NeuralNetwork(4, Random.Range(2,8), 2);
            
        //Задержка перед началом движением.
        this.time -= time;  
    }
    

    void OnTriggerEnter2D(Collider2D col)
    {
        
        shape.rb.velocity*=0.5f;
        float foodValue = col.gameObject.GetComponent<Food>().Eat();
        if(foodValue != 0)
            spawn.foodCount--;
        EnergyChange(foodValue*0.5f);
        
    }

    void Reproduction()
    {
        GameObject t = Instantiate(circle, new Vector3(transform.position.x, transform.position.y, 100), Quaternion.identity);
        t.GetComponent<Circle>().Initialization(neural, 1f);
        manager.GetComponent<Spawn>().circleCount++;
    }
    void EnergyChange(float energy)
    {
        if(this.energy + energy > maxEnergy)
        {
            this.energy = maxEnergy;
        } 
        else
        {
            if(this.energy + energy < 0)
            {
                this.energy = 0;
            }   
            else
            {
                this.energy += energy;
            }    
        }   
    }
    void EnergyControl()
    {
        if(energy > 0)
        {
            energy -= Time.deltaTime;
            if(energy > 70)
            {
                Reproduction();
                EnergyChange(-30);
            }
        }
        else
        {
            if(energyDeath)
            {
                spawn.circleCount--;
                Destroy(gameObject);
            }
        }
    }



    Vector2[] СlosestFood(Collider2D[] colliders)
    {   
        Vector2[] result = new Vector2[2];
        result[0] = new Vector2(100, 100);
        result[1]= Vector2.zero;
        
        if(nearestColliders != null)
        {
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].name == "Food(Clone)")
                {
                    Vector2 t = ((Vector2)colliders[i].transform.position-(Vector2)transform.position);
                    if( t.magnitude < result[0].magnitude)
                    {
                        result[0] = (Vector2)colliders[i].transform.position-(Vector2)transform.position;
                    }
                    result[1] += t.normalized;
                }
                
                
                
            }
            if(result[0] == new Vector2(100, 100)) result[0] = Vector2.zero;
        }
        else result[0] = Vector2.zero;
        result[0].Normalize();
        result[1].Normalize();
        return result;
    }


    void Update()
    {
        time += Time.deltaTime;
        if(time > 0)
        {
            shape.RotateProcess();
            shape.MoveProcess();

            if(time > 0.2f)
            {
                
                
                Vector2[] t;
                nearestColliders = Physics2D.OverlapCircleAll(transform.position, 10f);
                t = СlosestFood(nearestColliders);
                
                float[] p = neural.FeedForward(t[0].x, t[0].y, t[1].x, t[1].y);
                shape.Move(new Vector2(p[0]*25f, p[1]*25f));
                
                time = 0;
            }
        }
    
        EnergyControl();
    }
}