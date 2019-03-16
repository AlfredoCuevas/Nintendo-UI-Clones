using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 1f;

    private void Update()
    {
        transform.Rotate(0, _rotationSpeed, 0, Space.Self);
    }
}
