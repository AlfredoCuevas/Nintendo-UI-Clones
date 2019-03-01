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

    private void Start()
    {
        foreach (Kingdom kingdom in _kingdoms)
        {
            SpawnKingdomPoint(kingdom);
        }
    }

    private void SpawnKingdomPoint(Kingdom kingdom)
    {
        Transform myKingdom = Instantiate(_kingdomPivotPrefab, _sphere);
    }

    [System.Serializable]
    public class Kingdom
    {
        public string name;
        public float xAngle, yAngle;
    }
}
