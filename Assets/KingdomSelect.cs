using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingdomSelect : MonoBehaviour
{
    [SerializeField]
    private List<Kingdom> _kingdoms = new List<Kingdom>();

    [SerializeField]
    private Transform _sphere;

    [SerializeField]
    private Transform _kingdomPivotPrefab;

    //-------------------------------------

    private void Start()
    {
        foreach (Kingdom kingdom in _kingdoms)
        {
            SpawnKingdomPoint(kingdom);
        }
    }

    // Rotates camera pivot based on a kingdoms pivot rotation
    public void LookAtKingdom(Kingdom kingdom)
    {
        Transform mainCamera = Camera.main.transform;
        Transform cameraPivot = mainCamera.parent.parent;

        cameraPivot.localEulerAngles = new Vector3(kingdom.xAngle, kingdom.yAngle, 0);
    }

    private void SpawnKingdomPoint(Kingdom kingdom)
    {
        Transform tempKingdom = Instantiate(_kingdomPivotPrefab, _sphere);
        tempKingdom.localEulerAngles = new Vector3(kingdom.xAngle, kingdom.yAngle, 0);
        tempKingdom.name = kingdom.name;
    }

    [Serializable]
    public class Kingdom
    {
        public string name;
        public float xAngle, yAngle;
    }
}
