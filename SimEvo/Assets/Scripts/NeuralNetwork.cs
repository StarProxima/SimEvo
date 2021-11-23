using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Layer
{
    public int size;
    public float[] neurons;
    public float[,] weights;

    public Layer(int size, int nextSize, bool bias)
    {
        int t = 0;
        if(bias) t = 1;
        this.size = size+t;
        neurons = new float[size+t];
        weights = new float[size+t, nextSize];
    }
}

public class NeuralNetwork
{
    public Layer[] layers;

    public NeuralNetwork(params int[] sizes)
    {
        layers = new Layer[sizes.Length];
        for (int i = 0; i < sizes.Length; i++)
        {
            int nextSize = 0;
            if(i < sizes.Length - 1)
            {
                nextSize = sizes[i + 1];
                layers[i] = new Layer(sizes[i], nextSize, true);
            }
            else
            {
                layers[i] = new Layer(sizes[i], nextSize, false);  
            } 
            
            for (int j = 0; j <= sizes[i]; j++)
            {
                
                for (int k = 0; k < nextSize; k++)
                {
                    layers[i].weights[j, k] = UnityEngine.Random.Range(-1f, 1f);
                }
            }
        }
    }

    public NeuralNetwork(NeuralNetwork neural, bool mutation = false)
    {
        float rand1 = UnityEngine.Random.value;
        layers = new Layer[neural.layers.Length];
        for (int i = 0; i < neural.layers.Length; i++)
        {
            int nextSize = 0;
            int t = 1;
            if(i < neural.layers.Length - 1)
            {
                if(i < neural.layers.Length - 2)
                    nextSize = neural.layers[i+1].size -1;
                else
                    nextSize = neural.layers[i+1].size;  
                layers[i] = new Layer(neural.layers[i].size - 1, nextSize, true);
            }
            else
            {
                layers[i] = new Layer(neural.layers[i].size, nextSize, false);
                t = 0;
            } 
           
            for (int j = 0; j < neural.layers[i].size -1 + t; j++)
            {
                for (int k = 0; k < nextSize; k++)
                {
                    float w = neural.layers[i].weights[j,k];
                    if(mutation && rand1 < 0.5f)
                    {
                        float rand2 = UnityEngine.Random.value;
                        
                        if(rand2 < 0.25)
                            layers[i].weights[j, k] = w + UnityEngine.Random.Range(-0.05f, 0.05f);
                            //layers[i].weights[j, k] = w + UnityEngine.Random.Range(-Mathf.Min(0.05f, 1 + w), Mathf.Min(0.05f, 1 - w));
                        if(rand2 < 0.1)
                            layers[i].weights[j, k] = w + UnityEngine.Random.Range(-0.5f, 0.5f);
                            //layers[i].weights[j, k] = w + UnityEngine.Random.Range(-Mathf.Min(0.5f, 1 + w), Mathf.Min(0.5f, 1 - w));
                    }
                    else
                    {
                        layers[i].weights[j, k] = w;
                    } 
                }
            }
        }
    }

    public float[] FeedForward(params float[] inputs)
    {
        //Array.Copy(inputs, 0, layers[0].neurons, 0, inputs.Length);
        for(int i =0; i < inputs.Length; i++)
        {
            layers[0].neurons[i] = inputs[i];
        } 
        layers[0].neurons[inputs.Length] = 0.1f;

        for (int i = 1; i < layers.Length; i++) 
        {
            // float min = 0f;
            // if(i == layers.Length - 1) min = -1f;
            
            for (int j = 0; j < layers[i].size; j++)
            {
                
                if(j < layers[i].size - 1 || i == layers.Length - 1)
                {
                    layers[i].neurons[j] = 0;
                    for (int k = 0; k < layers[i-1].size; k++)
                    {
                        layers[i].neurons[j] += layers[i-1].neurons[k] * layers[i-1].weights[k, j];
                    }
                    layers[i].neurons[j] = Mathf.Min(1f, Mathf.Max(-1f, layers[i].neurons[j]));
                }
                else
                {
                   layers[i].neurons[j] = 0.1f; 
                }
                
            }
        }
        return layers[layers.Length - 1].neurons;
    }

}