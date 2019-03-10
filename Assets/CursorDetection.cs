using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorDetection : MonoBehaviour
{
    private GraphicRaycaster _graphicRaycaster;
    private PointerEventData _pointerEventData = new PointerEventData(null);

    void Start()
    {
        _graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
    }

    void Update()
    {
        _pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(_pointerEventData, results);

        if(results.Count > 0)
        {
            Debug.Log(results[0].gameObject.name);
        }
    }
}
