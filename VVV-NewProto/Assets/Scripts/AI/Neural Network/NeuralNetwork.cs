using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork 
{

    public int[] layers; //layers
    private float[][] neurons; //neuron matrix
    private float[][][] weights; //weight matrix

    
    public NeuralNetwork(int[] layers)
    {
        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }

        InitNeurons();
        InitWeights();
    }

    private void InitNeurons()
    {
        //Neuron initialization
        List<float[]> neuronsList = new List<float[]>();

        for (int i = 0;i < layers.Length;i++) //run through layers
        {
            neuronsList.Add(new float[layers[i]]); //add layer to neuron list
        }

        neurons = neuronsList.ToArray(); //convert list to array
    }

    private void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>(); // weights list which will later be converted into a weights 3d array

        for (int i = 0; i < layers.Length; i++)
        {
            List<float[]> layerWeightList = new List<float[]>(); //layer weight list for this current layer

            int neuronsInPreviousLayer = layers[i - 1];

            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer]; //neurons weights

                //setting weights randomly between 1 and - 1
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronWeights[k] = (float)Random.Range(1, -1) - 0.5f;
                }

                layerWeightList.Add(neuronWeights);
            }

            weightsList.Add(layerWeightList.ToArray());
        }

        weights = weightsList.ToArray();
    }

    public float[] FeedForward(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }
        for (int i = 1; i < inputs.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0.25f;
                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }

                neurons[i][j] = (float)Mathf.Tan(value);
            }
        }
            return neurons[neurons.Length-1];
    }
 
}
