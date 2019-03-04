using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingdomSelect : MonoBehaviour
{
    [SerializeField]
    private List<Kingdom> _kingdoms = new List<Kingdom>();

    [SerializeField]
    private Transform _sphereModel;

    [SerializeField]
    private Transform _kingdomPivotPrefab;

    [SerializeField]
    private Transform _uiContainer;

    [SerializeField]
    private Transform _kingdomButtonPrefab;

    [SerializeField]
    private Transform _bezierCurvePrefab;

    private Button _firstButton;
    private Transform _previousKingdom;

    //-------------------------------------

    private void Start()
    {
        foreach (Kingdom kingdom in _kingdoms)
        {
            SpawnKingdomPoint(kingdom);
        }
        
        // Select the first kindom
        LookAtKingdom(_kingdoms[0]);
        _firstButton.GetComponent<KingdomButton>().ManuallySelect();
    }

    public void LookAtKingdom(Kingdom kingdom)
    {
        Transform mainCamera = Camera.main.transform;
        Transform cameraPivot = mainCamera.parent.parent;

        cameraPivot.DOLocalRotate(new Vector3(kingdom.xAngle, kingdom.yAngle, 0), 1, RotateMode.Fast);
    }

    private void SpawnKingdomPoint(Kingdom kingdom)
    {
        Transform tempKingdom = Instantiate(_kingdomPivotPrefab, _sphereModel);
        tempKingdom.localEulerAngles = new Vector3(kingdom.xAngle, kingdom.yAngle, 0);
        tempKingdom.name = kingdom.name;

        SpawnKingdomButton(kingdom);

        if (_previousKingdom)
        {
            CreateConnectingLine(_previousKingdom.GetComponentInChildren<MeshRenderer>().transform, tempKingdom.GetComponentInChildren<MeshRenderer>().transform);
        }
        _previousKingdom = tempKingdom;
    }

    private void SpawnKingdomButton(Kingdom kingdom)
    {
        Button kingdomButton = Instantiate(_kingdomButtonPrefab, _uiContainer).GetComponent<Button>();
        kingdomButton.name = kingdom.name + " Button";
        kingdomButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = kingdom.name;
        kingdomButton.onClick.AddListener(() => LookAtKingdom(kingdom));

        if (!_firstButton) _firstButton = kingdomButton;
    }

    private void CreateConnectingLine(Transform prevKingdom, Transform currentKingdom)
    {
        var line = Instantiate(_bezierCurvePrefab, _sphereModel).GetComponent<LineCreator>();
        line.SetUpLine(prevKingdom, currentKingdom);
    }

    [Serializable]
    public class Kingdom
    {
        public string name;
        public float xAngle, yAngle;
    }
}
