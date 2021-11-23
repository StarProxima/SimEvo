using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Circle: MonoBehaviour {

    [SerializeField] GameObject circle;
    NeuralNetwork neural;

    Genome genome;
    //Parametrs
    [SerializeField] float energy = 20;
    float maxEnergy = 100;
    float maxSpeed = 10;
    float maxRotateSpeed = 25;
    [SerializeField] bool energyDeath = true;

    //Draw parametrs
    float width = -1f;
    int vertexCountCircle = 16;
    int vertexCountEye = 24;
    int eyeLenght = 4;
    float radius = 0.5f;
    float theta = 0f;
    float time = 0;
    private LineRenderer lineCircle;
    private LineRenderer lineEye;


    //Rotate parametrs
    public float targetRotateAngle = 0;
    
    float diffRotateAngle;

    //Move parametrs
    public Vector2 targetPos;
    Vector2 diffMovePos;
    Vector2 lastDiffMovePos;
    Vector2 lastTargetPos;
    Vector2 speed;
    Collider2D[] nearestColliders;

    Rigidbody2D rb;

    public void Initialization(NeuralNetwork neural = null)
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        targetPos = transform.position;
        DrawCircle();
        if(neural != null)
            this.neural = new NeuralNetwork(neural, true);
        else
            this.neural = new NeuralNetwork(2,4,2);
            
        //Задержка перед началом движения/поворота объекта.
        //this.time -= time;  
    }
    void DrawCircle()
    {
        lineCircle = GetComponent<LineRenderer>();
        lineEye = gameObject.transform.GetChild(0).GetComponent<LineRenderer>();
        lineEye.positionCount = eyeLenght+1;
        lineEye.startWidth = width/3;
        lineEye.endWidth = width/3;

        lineCircle.startWidth = width;
        lineCircle.endWidth = width;
        lineCircle.positionCount = vertexCountCircle;
        
        theta = 0f;
        for (int i = 0; i < vertexCountCircle; i++) {
            
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            theta += 2.0f * Mathf.PI / (vertexCountCircle);
            lineCircle.SetPosition(i, new Vector3(x, y, 0));
        }

        theta = -2.0f * Mathf.PI / (vertexCountEye) * eyeLenght/2;
        for (int i = 0; i <= eyeLenght; i++)
        {
            float x = (radius*2-(-lineEye.startWidth/2*0.5f)) * Mathf.Cos(theta);
            float y = (radius*2-(-lineEye.startWidth/2*0.5f)) * Mathf.Sin(theta);
            theta += 2.0f * Mathf.PI / (vertexCountEye);
            lineEye.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        EnergyChange(5);
        Camera.main.GetComponent<Spawn>().foodCount--;
        Destroy(col.gameObject);
    }

    void RotateCircle(float angle)
    {
        targetRotateAngle = angle;
    }

    void RotateCircleProcess()
    {
        diffRotateAngle = targetRotateAngle - gameObject.transform.rotation.eulerAngles.z;

        if(Time.deltaTime < 1)
            rb.angularVelocity*= 1 - Time.deltaTime;
        
        if(Mathf.Abs(diffRotateAngle) > 1f)
        {
            targetRotateAngle = targetRotateAngle % 360;
            if(targetRotateAngle < 0) targetRotateAngle += 360;

            if(targetRotateAngle > gameObject.transform.rotation.eulerAngles.z)
            {
                
                if(Mathf.Abs(diffRotateAngle) > Mathf.Abs(Mathf.Abs(targetRotateAngle - gameObject.transform.rotation.eulerAngles.z)-360))
                    diffRotateAngle = Mathf.Abs(targetRotateAngle - gameObject.transform.rotation.eulerAngles.z )-360;
            }
            else 
            {
                if(Mathf.Abs(diffRotateAngle) > Mathf.Abs(Mathf.Abs(targetRotateAngle - gameObject.transform.rotation.eulerAngles.z)-360))
                    diffRotateAngle = Mathf.Abs(Mathf.Abs(targetRotateAngle - gameObject.transform.rotation.eulerAngles.z)-360);
            }
            float t = diffRotateAngle;
            if(Mathf.Abs(t) > 50) diffRotateAngle = Mathf.Sign(diffRotateAngle) * 50;
            // if(Mathf.Abs(t) < 10) diffRotateAngle = Mathf.Sign(diffRotateAngle) * 10;
            // if(Mathf.Abs(t) < 20) diffRotateAngle = Mathf.Sign(diffRotateAngle) * 20;
            diffRotateAngle = Mathf.Sign(diffRotateAngle)*10 + diffRotateAngle;
            diffRotateAngle = Time.deltaTime*50 < maxRotateSpeed ? diffRotateAngle/maxRotateSpeed*Time.deltaTime*50 : diffRotateAngle/maxRotateSpeed;
            gameObject.transform.Rotate(new Vector3(0,0,  diffRotateAngle));
        }    
    }
    
    void MoveCircle(Vector2 target)
    {
        targetPos = target;
    }
    
    void MoveCircleProcess()
    {
        diffMovePos = (targetPos-(Vector2)transform.position);
        if(diffMovePos.sqrMagnitude > 0.05f*0.05f)
        {
            if(rb.velocity.magnitude < maxSpeed)
            rb.velocity += diffMovePos.normalized*Time.deltaTime*5;
            float t = 1 - 1.4f/(diffMovePos.magnitude+0.4f)*Time.deltaTime*5;
            rb.velocity = rb.velocity * t;
        }
        else
        {
            if(rb.velocity != Vector2.zero)
                rb.velocity = Vector2.zero;
        } 
    }
    
    Vector2 DirFromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle* Mathf.Deg2Rad));
    }

    void Reproduction()
    {
        GameObject t = (GameObject)Instantiate(Resources.Load("Circle", typeof(GameObject)), new Vector3(transform.position.x, transform.position.y, 100), Quaternion.identity);
        t.GetComponent<Circle>().Initialization(neural);
        Camera.main.GetComponent<Spawn>().circleCount++;
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
                EnergyChange(-40);
            }
        }
        else
        {
            if(energyDeath)
            {
                Camera.main.GetComponent<Spawn>().circleCount--;
                Destroy(gameObject);
            }
        }
    }



    Vector2 СlosestFood(Collider2D[] colliders)
    {
        Vector2 result = new Vector2(100, 100);
        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].name == "Food(Clone)" && ((Vector2)colliders[i].transform.position-(Vector2)transform.position).magnitude < result.magnitude)
            {
                result = (Vector2)colliders[i].transform.position-(Vector2)transform.position;
            }
        }
        if(result == new Vector2(100, 100)) result = Vector2.zero;
        return result;
    }
    void Update()
    {
        time += Time.deltaTime;
        if(time > 0)
        {
            RotateCircleProcess();
            MoveCircleProcess();

            if(time > 0.2f)
            {
                
                
                Vector2 t = new Vector2(Random.Range(-10f,10f), Random.Range(-10f,10f));
                nearestColliders = Physics2D.OverlapCircleAll(transform.position, 10f);
                if(nearestColliders.Length > 0)
                {
                    t = СlosestFood(nearestColliders);
                }
                float[] p = neural.FeedForward(t.x/10f, t.y/10f);
                targetPos.x = transform.position.x + p[0]*25f;
                targetPos.y = transform.position.y + p[1]*25f;
                time = 0;
            }
        }
    
        EnergyControl();
    }
}