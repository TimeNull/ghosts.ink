using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUtils
{
    public static float GetPathLength(Transform from, Transform to, int areaMask)
    {
        NavMeshPath path = new NavMeshPath();

        if(NavMesh.CalculatePath(from.position, to.position, areaMask, path))
        {
            float distance = Vector3.Distance(from.position, path.corners[0]);

            for (int i = 1; i < path.corners.Length; i++)
                distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);

            return distance;
        }

        //invalid path
        return -1;
    }
}
