using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Circle: MonoBehaviour {

    [SerializeField] GameObject manager;
    NeuralNetwork neural;
    NeuralNetwork neuralAuxiliary;


    //Parametrs
    [SerializeField] float energy = 20;
    float maxEnergy = 100;
    float maxSpeed = 10;
    float maxRotateSpeed = 50;
    [SerializeField] bool energyDeath = true;

    //Stats
    public int generation = 0;
    public int reproductionCount = 0;
    public float lifetime = 0;
    public int neuronCount = 0;
    
    public int foodEaten = 0;
    

    //Draw
    int vertexCount = 16;
    int vertexCountEye = 24;
    int eyeLenght = 4;
    float radius = 0.5f;
    float width = -1f;

    
    Collider2D[] nearestColliders;

    Collider2D[][] сolliders;
    int colIndex = 0; 

    Rigidbody2D rb;
    GameObject circle;
    
    Spawn spawn;
    float time = 0;
    Shape shape;
    public void Initialization(NeuralNetwork neural = null, NeuralNetwork neuralAuxiliary = null, float time = 0)
    {
        shape = new Shape(gameObject, maxSpeed, maxRotateSpeed);
        shape.DrawFigure(vertexCount, vertexCountEye, eyeLenght, radius, width);
        
        circle = (GameObject)Resources.Load("Circle", typeof(GameObject));
        manager = Camera.main.GetComponent<CamControl>().manager;
        spawn = manager.GetComponent<Spawn>();

        сolliders = new Collider2D[5][];
        for(int i = 0; i < 5; i++)
            сolliders[i] = new Collider2D[0];
        spawn.circleCount++;
        //DrawCircle();
        if(neural != null)
        {
            neuronCount = neural.layers[1].neurons.Length;
            spawn.shapeNeuronCount[neuronCount]++;
            this.neural = new NeuralNetwork(neural, 0.5f);
            this.neuralAuxiliary = new NeuralNetwork(neuralAuxiliary, 0.2f);
        }  
        else
        {
            neuronCount = Random.Range(1,13);
            spawn.shapeNeuronCount[neuronCount]++;
            this.neural = new NeuralNetwork(0, 4, neuronCount, 2);
            this.neuralAuxiliary = new NeuralNetwork(0, 4, neuronCount, 1);
        }
            
            
        //Задержка перед началом движением.
        this.time -= time;  
    }
    

    void OnTriggerEnter2D(Collider2D col)
    {
        shape.rb.velocity*=0.5f;
        float foodValue = col.gameObject.GetComponent<Food>().Eat();
        if(foodValue != 0)
        {
            foodEaten++;
            spawn.foodCount--;
            EnergyChange(foodValue*0.35f);
        }
    }

    void Reproduction()
    {
        if(spawn.shapeReprodaction)
        {
            GameObject t = Instantiate(circle, new Vector3(transform.position.x, transform.position.y, 100), Quaternion.identity);
            t.GetComponent<Circle>().Initialization(neural, neuralAuxiliary);
            t.GetComponent<Circle>().generation = generation + 1;
            reproductionCount++;
        }
        
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
                this.energy = 0;  
            else
                this.energy += energy;   
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
                EnergyChange(-50);
            }
        }
        else
        {
            if(energyDeath)
            {
                spawn.circleCount--;
                spawn.shapeNeuronCount[neuronCount]--; 
                Destroy(gameObject);
            }
        }
    }



    Vector2[] СlosestAndCenterMassFood(Collider2D[] colliders)
    {   
        Vector2[] result = new Vector2[2];
        result[0] = new Vector2(100, 100);
        result[1]= Vector2.zero;
        
        if(colliders != null)
        {
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].tag == "Food")
                {
                    
                    Vector2 t = (Vector2)colliders[i].transform.position-(Vector2)transform.position;
                    if( t.magnitude < result[0].magnitude)
                    {
                        result[0] = t;
                    }
                    result[1] += t.normalized;
                }  
            }
            if(result[0] == new Vector2(100, 100)) result[0] = Vector2.zero;
        }
        else result[0] = Vector2.zero;
        
        result[0] = result[0].normalized * (-Mathf.Log(result[0].magnitude+4f, 2.71f)+4f);
        result[1]= result[1].normalized * (Mathf.Log10(result[1].magnitude+1)+0.5f);

        return result;
    }

    Vector2 CenterMassFood(Collider2D[] colliders)
    {
        

        Vector2 result = Vector2.zero;

        if (colliders != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Food")
                {

                    Vector2 t = (Vector2)colliders[i].transform.position - (Vector2)transform.position;

                    result += t.normalized;
                }
            }
            
        }

        result = result.normalized * (Mathf.Log10(result.magnitude + 1) + 0.5f);

        return result;
    }


    void Update()
    {
        
        time += Time.deltaTime;
        if(time > 0)
        {
            shape.RotateProcess();
            shape.MoveProcess();

            if(time > 0.5f)
            {


                Vector2[] t;
                Vector2 t2;

                //nearestColliders = Physics2D.OverlapCircleAll((Vector2)transform.position, 10f);
                nearestColliders = Physics2D.OverlapCircleAll((Vector2)transform.position + shape.DirFromAngle(transform.rotation.eulerAngles.z)*7.5f, 15f);
                //Debug.DrawRay(transform.position, shape.DirFromAngle(transform.rotation.eulerAngles.z)*5f, Color.cyan, 1f);


                // сolliders[colIndex++%5] = Physics2D.OverlapCircleAll(transform.position, 10f);

                // int size = 0;
                // for(int i =0; i < 5; i++)
                //     size+=сolliders[i].Length;
                // Collider2D[] h = new Collider2D[size];

                // size = 0;
                // for(int i =0; i < 5; i++)
                // {
                //     сolliders[i].CopyTo(h, size);
                //     size+= сolliders[i].Length;
                // }

                // HashSet<Collider2D> set = new HashSet<Collider2D>(h);
                // set.CopyTo(h, 0);

                t = СlosestAndCenterMassFood(nearestColliders);
                nearestColliders = Physics2D.OverlapCircleAll(transform.position, 20f);
                t2 = CenterMassFood(nearestColliders);
                float[] p = neural.FeedForward(t[0].x, t[0].y, t[1].x, t[1].y);
                shape.Move(new Vector2(p[0]*25f, p[1]*25f));

                float[] p2 = neuralAuxiliary.FeedForward(p[0], p[1], t2.x, t2.y);
                shape.RotateTo(transform.rotation.eulerAngles.z + p2[0]*10f);
                //Debug.Log(Mathf.Acos(temp.x)/ Mathf.Deg2Rad);

                lifetime += time;
                time = 0; 
            }
        }
    
        EnergyControl();
    }
}