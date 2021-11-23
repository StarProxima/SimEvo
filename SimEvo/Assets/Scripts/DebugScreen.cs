using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class DebugScreen : MonoBehaviour {
    GUIStyle[] style = new GUIStyle[5];
    int accumulator = 0;
    int fpsCounter = 0, circlesCounter = 0, foodsCounter = 0;
    float fpsTimer = 0f, timer = 0;
 
    void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 1200;

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
        GUI.Label(new Rect(30, 58, 100, 34), "Foods: " + foodsCounter, style[2]);
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
            circlesCounter = Camera.main.GetComponent<Spawn>().circleCount;
            foodsCounter = Camera.main.GetComponent<Spawn>().foodCount;
            timer = 0;
        }
    }
}