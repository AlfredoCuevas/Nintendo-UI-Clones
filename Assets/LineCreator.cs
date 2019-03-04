using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    [SerializeField]
    private int numPoints = 50;

    [SerializeField]
    private LineRenderer _lineRenderer;

    private Vector3[] points;

    public void SetUpLine(Transform startPoint, Transform endPoint)
    {
        _lineRenderer.positionCount = numPoints;
        points = new Vector3[numPoints + 1];

        CreateQuadraticCurve(startPoint.position, endPoint.position);

        _lineRenderer.SetPositions(points);
    }

    private void CreateQuadraticCurve(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3 thirdPoint = CalculateThirdPoint(startPoint, endPoint);

        for (int i = 0; i <= numPoints; i++)
        {
            float t = i / (float)numPoints;
            points[i] = CalculateQuadraticBezierPoint(t, startPoint, endPoint, thirdPoint);
        }
    }

    private Vector3 CalculateThirdPoint(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3 midpoint = new Vector3((startPoint.x + endPoint.x) * 0.5f,
                                       (startPoint.y + endPoint.y) * 0.5f,
                                       (startPoint.z + endPoint.z) * 0.5f);
        //midpoint *= 1.4f;
        midpoint = Vector3.Normalize(midpoint);
        midpoint *= .7f;

        return midpoint;
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 startPoint, Vector3 endPoint, Vector3 thirdPoint)
    {
        float a = (1 - t);
        float axa = a * a;
        return (axa * startPoint) + (2 * a * t * thirdPoint) + ((t * t) * endPoint);
    }
}
