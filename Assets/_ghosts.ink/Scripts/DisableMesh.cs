using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMesh : MonoBehaviour
{
    private MeshRenderer mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        Destroy(mesh);
    }
}
