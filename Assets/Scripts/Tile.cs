using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void ChangeColor(Color color)
    {
        _meshRenderer.material.color = color;
    }
}