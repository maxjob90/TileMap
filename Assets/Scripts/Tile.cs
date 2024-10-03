using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _greenColor;
    [SerializeField] private Color _redColor;
    private List<Material> _materials = new();

    private void Awake()
    {
        var renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in renderers)
        {
            _materials.Add(meshRenderer.material);
        }
    }

    public void ChangeColor(bool isTileWithinPlane, bool isTilePlaced)
    {
        if (!isTileWithinPlane || !isTilePlaced)
        {
            foreach (var material in _materials)
            {
                material.color = _redColor;
            }
        }
        else
        {
            foreach (var material in _materials)
            {
                material.color = _greenColor;
            }
        }
    }

    public void ResetColor()
    {
        foreach (var material in _materials)
        {
            material.color = Color.white;
        }
    }
}