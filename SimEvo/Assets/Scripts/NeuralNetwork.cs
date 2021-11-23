using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Genome
{
    float[,] weights;
    public Genome(params int[] sizes)
    {
        weights = new float[sizes[0], sizes[1]];
        for(int i = 0; i < sizes[0]; i++)
        {
            for(int j = 0; j < sizes[1]; j++)
            {
                weights[i, j] = UnityEngine.Random.Range(-1f, 1f);
            }
        }
    }

    public Genome(Genome genome)
    {
        weights = new float[genome.weights.GetUpperBound(0), genome.weights.GetUpperBound(1)];
        for(int i = 0; i < genome.weights.GetUpperBound(0); i++)
        {
            for(int j = 0; j < genome.weights.GetUpperBound(1); j++)
            {
                weights[i, j] = genome.weights[i,j];
            }
        }
    }

    public void Mutate()
    {
        for(int i = 0; i < weights.GetUpperBound(0); i++)
        {
            for(int j = 0; j < weights.GetUpperBound(1); j++)
            {
                float rand = UnityEngine.Random.value;
                if(rand < 0.25)
                    weights[i, j] += UnityEngine.Random.Range(-0.15f, 0.15f);
                if(rand < 0.1)
                    weights[i, j] += UnityEngine.Random.Range(-0.5f, 0.5f);
            }
        }
    }
}

public class Layer
{
    public int size;
    public float[] neurons;
    public float[,] weights;

    public Layer(int size, int nextSize)
    {
        this.size = size;
        neurons = new float[size];
        weights = new float[size, nextSize];
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
            if(i < sizes.Length - 1) nextSize = sizes[i + 1];
            layers[i] = new Layer(sizes[i], nextSize);
            for (int j = 0; j < sizes[i]; j++)
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
            if(i < neural.layers.Length - 1) nextSize = neural.layers[i+1].size;
            layers[i] = new Layer(neural.layers[i].size, nextSize);
            for (int j = 0; j < neural.layers[i].size; j++)
            {
                for (int k = 0; k < nextSize; k++)
                {
                    if(mutation && rand1 < 0.5f)
                    {
                        float rand2 = UnityEngine.Random.value;
                        if(rand2 < 0.25)
                            layers[i].weights[j, k] = neural.layers[i].weights[j,k] + UnityEngine.Random.Range(-0.05f, 0.05f);
                        if(rand2 < 0.1)
                            layers[i].weights[j, k] = neural.layers[i].weights[j,k] + UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else
                    {
                        layers[i].weights[j, k] = neural.layers[i].weights[j,k];
                    } 
                }
            }
        }
    }

    public float[] FeedForward(params float[] inputs)
    {
        Array.Copy(inputs, 0, layers[0].neurons, 0, inputs.Length);
        for (int i = 1; i < layers.Length; i++) 
        {
            float min = 0f;
            if(i == layers.Length - 1) min = -1f;
            // Layer l = layers[i-1];
            // Layer l1 = layers[i];
            for (int j = 0; j < layers[i].size; j++)
            {
                layers[i].neurons[j] = 0;
                for (int k = 0; k < layers[i-1].size; k++)
                {
                    layers[i].neurons[j] += layers[i-1].neurons[k] * layers[i-1].weights[k, j];
                }
                layers[i].neurons[j] = Mathf.Min(1f, Mathf.Max(min, layers[i].neurons[j]));
            }
        }
        return layers[layers.Length - 1].neurons;
    }

}