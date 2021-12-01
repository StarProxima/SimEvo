using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class Layer
{
    public int size;
    public float[] neurons;
    public float[,] weights;

    public bool bias;

    public Layer(int size, int nextSize, bool bias = false)
    {
        
        if(bias)
        {
            this.size = size+1;
            neurons = new float[size+1];
            weights = new float[size+1, nextSize];
        }
        else
        {
            this.size = size;
            neurons = new float[size];
            weights = new float[size, nextSize];
        }   
    }
    
}

public class NeuralNetwork
{
    public Layer[] layers;
    public int biasCount;
    public NeuralNetwork(int biasCount, params int[] sizes)
    {
        if(biasCount >= sizes.Length)
            biasCount = sizes.Length - 1;

        this.biasCount = biasCount;
        layers = new Layer[sizes.Length];

            for (int i = 0; i < sizes.Length; i++)
            {
                int nextSize = 0;
                int isBiasLayer = 0;
                if(i < sizes.Length - 1)
                    nextSize = sizes[i + 1];

                if(i < biasCount)
                {
                    layers[i] = new Layer(sizes[i], nextSize, true);
                    isBiasLayer = 1;
                }  
                else
                {
                    layers[i] = new Layer(sizes[i], nextSize);

                       
                }
                    
                for (int j = 0; j < sizes[i] + isBiasLayer; j++)
                {
                    for (int k = 0; k < nextSize; k++)
                    {
                        layers[i].weights[j, k] = UnityEngine.Random.Range(-1f, 1f);
                    }
                }
                
            }
    }

    public NeuralNetwork(NeuralNetwork neural, float mutationWeights = 0, float mutationNeurons = 0)
    {

        int[] sizes = new int[neural.layers.Length];
        for(int i = 0; i < neural.layers.Length; i++)
        {
            sizes[i] = neural.layers[i].size;
        }
        float rand = UnityEngine.Random.value;
        
        int randNeuron;
        if (rand > mutationWeights)
        {
            mutationWeights = 0;
        }
        


        layers = new Layer[neural.layers.Length];
        this.biasCount = neural.biasCount;
        for (int i = 0; i < neural.layers.Length; i++)
        {
            randNeuron = 0;
            int nextSize = 0;
            int isBiasLayer = 0;

            if (i < neural.layers.Length - 2 && mutationWeights != 0)
            {
                float randNeurons = UnityEngine.Random.value;
                if (randNeurons < mutationNeurons)
                {
                    randNeuron = Random.Range(-1, 2);
                    sizes[i+1] += randNeuron;
                }

            }


            if (i < neural.layers.Length-1)
            {
                if(i+1 < biasCount)
                    nextSize = sizes[i+1] - 1;
                else
                    nextSize = sizes[i+1];
            }
            

            if(i < neural.biasCount)
            {
                isBiasLayer = 1;
                layers[i] = new Layer(sizes[i] - 1, nextSize, true);
            }  
            else
            {
                layers[i] = new Layer(sizes[i], nextSize, false);
            }
                 

            for (int j = 0; j < sizes[i] - 1 + isBiasLayer; j++)
            {
                for (int k = 0; k < nextSize; k++)
                {
                    if(randNeuron != 1 && j < sizes[i] - 1 + isBiasLayer -1)
                        layers[i].weights[j, k] = neural.layers[i].weights[j,k];
                    else layers[i].weights[j, k] = UnityEngine.Random.Range(-1f, 1f);




                    rand = UnityEngine.Random.value;
                        
                    if(rand < 1f * mutationWeights)
                        layers[i].weights[j, k] += UnityEngine.Random.Range(-0.25f * mutationWeights, 0.25f * mutationWeights);
                        //layers[i].weights[j, k] = w + UnityEngine.Random.Range(-Mathf.Min(0.05f, 1 + w), Mathf.Min(0.05f, 1 - w));
                    if(rand < 0.2f * mutationWeights)
                        layers[i].weights[j, k] += UnityEngine.Random.Range(-2f * mutationWeights, 2f * mutationWeights);
                        //layers[i].weights[j, k] = w + UnityEngine.Random.Range(-Mathf.Min(0.5f, 1 + w), Mathf.Min(0.5f, 1 - w));
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
        if(biasCount > 0) 
            layers[0].neurons[inputs.Length] = 0.1f;

        for (int i = 1; i < layers.Length; i++) 
        {
            // float min = 0f;
            // if(i == layers.Length - 1) min = -1f;
            int isBiasLayer = 0;
                if(i < biasCount)
                {
                    isBiasLayer = 1;
                }

            for (int j = 0; j < layers[i].size; j++)
            {
                

                if(j < layers[i].size - isBiasLayer)
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