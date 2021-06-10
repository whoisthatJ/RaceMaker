using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public static class CSRandom
{
    //Random for Two Value
    public static int Range(int a, int b)
    {
        int t = UnityEngine.Random.Range(-1, 1);
        if (t < 0) return a;
        else
            return b;
    }
    public static float Range(float a, float b)
    {
        float t = UnityEngine.Random.Range(-1, 1);
        if (t < 0) return a;
        else
            return b;
    }


    public static int RandomNumber(int minN, int maxN, params int[] exNumbers)
    {
        System.Random random = new System.Random();
        int result = exNumbers.First();
        while (exNumbers.ToList().Contains(result))
        {
            result = random.Next(minN, maxN + 1);
        }
        return result;
    }

    public static List<int> GetRandomNumbers(int count, int maxCount)
    {
        List<int> randomNumbers = new List<int>();
        System.Random random = new System.Random();
        for (int i = 0; i < count; i++)
        {
            int number;

            do
            {
                number = random.Next(0, maxCount);
            }
            while (randomNumbers.Contains(number) || randomNumbers.FindIndex(x => x == (number + 1)) != -1 || randomNumbers.FindIndex(x => x == (number - 1)) != -1);

            randomNumbers.Add(number);
        }

        return randomNumbers;
    }
}