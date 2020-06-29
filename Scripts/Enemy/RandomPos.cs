using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class RandomPos 
{
  public static Vector3 GetRandomPoInitOnNavMesh(Vector3 center, float distance, int areaMask)
    {
        var randomPos = UnityEngine.Random.insideUnitSphere * distance + center;

        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, distance, areaMask);

        return hit.position;
    }

    public static float GetRandomNormalDistribution(float mean, float standard)
    {
        var x1 = UnityEngine.Random.Range(0f, 1f);
        var x2 = UnityEngine.Random.Range(0f, 1f);
        return mean + standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));


    }
}
