using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        transform.RotateAround(Vector3.zero, new Vector3(0, 1, 0), _rotationSpeed * Time.deltaTime);
    }
}
