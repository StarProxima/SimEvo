using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class DebugScreen : MonoBehaviour {
    GUIStyle[] style = new GUIStyle[5];
    int accumulator = 0;
    int fpsCounter = 0, circlesCounter = 0, foodsCounter = 0;
    int ShapeAll = 0;
    int[] shapeNeuronCount;
    float fpsTimer = 0f, timer = 0;
 
    void Start() {
        QualitySettings.vSyncCount = 0;
        Application.runInBackground = true;
        Application.targetFrameRate = 1200;

        shapeNeuronCount = new int[32];
        for (int i = 0; i < shapeNeuronCount.Length; i++) shapeNeuronCount[i] = 0;

        for (int i = 0; i < 5; i++)
            style[i] = new GUIStyle();
        style[0].normal.textColor = Color.white;
        style[0].fontSize = 24;
        style[0].fontStyle = FontStyle.Bold;
        
        for(int i = 1; i <= 2; i++)
        {
            style[i].normal.textColor = Color.white;
            style[i].fontSize = 16;
            style[i].fontStyle = FontStyle.Normal;
        }
    }
 
    void OnGUI() {
        GUI.Label(new Rect(30, 10, 100, 34), "FPS: " + fpsCounter, style[0]);
        GUI.Label(new Rect(30, 40, 100, 34), "Circles: " + circlesCounter, style[1]);
        GUI.Label(new Rect(30, 60, 100, 34), "Foods: " + foodsCounter, style[1]);
        GUI.Label(new Rect(30, 90, 100, 34), "Shape1: " + shapeNeuronCount[1], style[1]);
        GUI.Label(new Rect(30, 110, 100, 34), "Shape2: " + shapeNeuronCount[2], style[1]);
        GUI.Label(new Rect(30, 130, 100, 34), "Shape3: " + shapeNeuronCount[3], style[1]);
        GUI.Label(new Rect(30, 150, 100, 34), "Shape4: " + shapeNeuronCount[4], style[1]);
        GUI.Label(new Rect(30, 170, 100, 34), "Shape5: " + shapeNeuronCount[5], style[1]);
        GUI.Label(new Rect(30, 190, 100, 34), "Shape6: " + shapeNeuronCount[6], style[1]);
        GUI.Label(new Rect(30, 210, 100, 34), "Shape7: " + shapeNeuronCount[7], style[1]);
        GUI.Label(new Rect(30, 230, 100, 34), "Shape8: " + shapeNeuronCount[8], style[1]);
        GUI.Label(new Rect(30, 250, 100, 34), "Shape9: " + shapeNeuronCount[9], style[1]);
        GUI.Label(new Rect(30, 270, 100, 34), "Shape10: " + shapeNeuronCount[10], style[1]);
        GUI.Label(new Rect(30, 290, 100, 34), "Shape11: " + shapeNeuronCount[11], style[1]);
        GUI.Label(new Rect(30, 310, 100, 34), "Shape12: " + shapeNeuronCount[12], style[1]);
        
    }
 
    void Update() {
        accumulator++;
        fpsTimer += Time.deltaTime;
        timer += Time.deltaTime;
        if (fpsTimer >= 1f) {
            
            fpsCounter = accumulator;
            accumulator=0;
        
            fpsTimer = 0;
            
        }

        if(timer >= 0.1f)
        {
            circlesCounter = GetComponent<Spawn>().circleCount;
            foodsCounter = GetComponent<Spawn>().foodCount;
            shapeNeuronCount = GetComponent<Spawn>().shapeNeuronCount;
            
            timer = 0;
        }
    }
}