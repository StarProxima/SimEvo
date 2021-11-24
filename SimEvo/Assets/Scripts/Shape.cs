using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    GameObject obj;

    public Rigidbody2D rb;
    //Draw parametrs
    float width = -1f;
    int vertexCount = 16;
    int vertexCountEye = 24;
    int eyeLenght = 4;
    float radius = 0.5f;
    float theta = 0f;
    private LineRenderer line;
    private LineRenderer lineEye;

    //Rotate parametrs
    public float targetRotateAngle = 0;
    float maxRotateSpeed;
    float diffRotateAngle;

    //Move parametrs
    public Vector2 targetPos;
    Vector2 diffMovePos;
    Vector2 lastDiffMovePos;
    Vector2 lastTargetPos;
    Vector2 speed;
    float maxSpeed;

    public void DrawFigure(int vertexCount, int vertexCountEye, int eyeLenght, float radius, float width)
    {
        line = obj.GetComponent<LineRenderer>();
        lineEye = obj.transform.GetChild(0).GetComponent<LineRenderer>();
        lineEye.positionCount = eyeLenght+1;
        lineEye.startWidth = width/3;
        lineEye.endWidth = width/3;

        line.startWidth = width;
        line.endWidth = width;
        line.positionCount = vertexCount;
        
        theta = 0f;
        for (int i = 0; i < vertexCount; i++) {
            
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            theta += 2.0f * Mathf.PI / (vertexCount);
            line.SetPosition(i, new Vector3(x, y, 0));
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

    Vector2 DirFromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle* Mathf.Deg2Rad));
    }
    
    public void RotateTo(float angle)
    {
        targetRotateAngle = angle;
    }

    public void RotateProcess()
    {
        diffRotateAngle = targetRotateAngle - obj.transform.rotation.eulerAngles.z;

        if(Time.deltaTime < 1)
            rb.angularVelocity*= 1 - Time.deltaTime;
        
        if(Mathf.Abs(diffRotateAngle) > 1f)
        {
            targetRotateAngle = targetRotateAngle % 360;
            if(targetRotateAngle < 0) targetRotateAngle += 360;

            if(targetRotateAngle > obj.transform.rotation.eulerAngles.z)
            {
                
                if(Mathf.Abs(diffRotateAngle) > Mathf.Abs(Mathf.Abs(targetRotateAngle - obj.transform.rotation.eulerAngles.z)-360))
                    diffRotateAngle = Mathf.Abs(targetRotateAngle - obj.transform.rotation.eulerAngles.z )-360;
            }
            else 
            {
                if(Mathf.Abs(diffRotateAngle) > Mathf.Abs(Mathf.Abs(targetRotateAngle - obj.transform.rotation.eulerAngles.z)-360))
                    diffRotateAngle = Mathf.Abs(Mathf.Abs(targetRotateAngle - obj.transform.rotation.eulerAngles.z)-360);
            }
            float t = diffRotateAngle;
            if(Mathf.Abs(t) > 50) diffRotateAngle = Mathf.Sign(diffRotateAngle) * 50;
            // if(Mathf.Abs(t) < 10) diffRotateAngle = Mathf.Sign(diffRotateAngle) * 10;
            // if(Mathf.Abs(t) < 20) diffRotateAngle = Mathf.Sign(diffRotateAngle) * 20;
            diffRotateAngle = Mathf.Sign(diffRotateAngle)*10 + diffRotateAngle;
            diffRotateAngle = Time.deltaTime*50 < maxRotateSpeed ? diffRotateAngle/maxRotateSpeed*Time.deltaTime*50 : diffRotateAngle/maxRotateSpeed;
            obj.transform.Rotate(new Vector3(0,0,  diffRotateAngle));
        }    
    }
    
    public void Move(Vector2 target)
    {
        targetPos = (Vector2)obj.transform.position + target;
    }
    
    public void MoveProcess()
    {
        diffMovePos = (targetPos-(Vector2)obj.transform.position);
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

    public Shape(GameObject obj, float maxSpeed, float maxRotateSpeed)
    {
        this.obj = obj;
        this.maxSpeed = maxSpeed;
        this.maxRotateSpeed = maxRotateSpeed;
        rb = obj.GetComponent<Rigidbody2D>();
        targetPos = obj.transform.position;

    }
}
