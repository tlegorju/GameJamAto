using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _points;
    public List<Transform> Points { get => _points; set => _points = value; }
}
