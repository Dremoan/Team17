using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HealthbarManager : MonoBehaviour
{
    [SerializeField] private Material healthBarMat;
    [SerializeField] private float threshold = 1f;

    public float Threshold { get => threshold; set => threshold = value; }

    private void Update()
    {
        if(healthBarMat != null)
        healthBarMat.SetFloat("_Threshold", Threshold);
    }
}
