using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLEDColor : MonoBehaviour
{
    public Material material_red;
    public Material material_green;
    public bool useGreenMaterial;

    void Start()
    {
        // Assign material_red to all plane surfaces at the start
        ApplyMaterial(material_red);
    }

    void Update()
    {
        // Check the boolean value and change the material accordingly
        if(useGreenMaterial == true)
        {
            ApplyMaterial(material_green); 
        }
        else
        {
            ApplyMaterial(material_red);
        }
    }

    void ApplyMaterial(Material mat)
    {
        foreach (Transform stone in transform)
        {
            foreach (Transform plane in stone)
            {
                var renderer = plane.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = mat;
                }
                else
                {
                    Debug.LogWarning($"No renderer found on {plane.name}!");
                }
            }
        }
    }
}
