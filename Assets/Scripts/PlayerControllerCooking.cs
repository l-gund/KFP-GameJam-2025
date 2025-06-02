using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerCooking : MonoBehaviour
{
    public float minDistance = 0.1f;
    public List<Vector2> cutPoints = new List<Vector2>();

    private LineRenderer lineRenderer;
    private bool isCutting = false;
    public GameTimer station1Timer;
    public CutEvaluator evaluator;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        station1Timer.StartPrepTime(10f);
    }

    void Update()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = transform.position.z;

        transform.position = mouseWorldPosition;

        if (Input.GetMouseButtonDown(0))
        {
            cutPoints.Clear();
            lineRenderer.positionCount = 0;
            isCutting = true;
        }

        if (Input.GetMouseButton(0) && isCutting)
        {
            if (cutPoints.Count == 0 || Vector2.Distance(cutPoints[cutPoints.Count - 1], mouseWorldPosition) > minDistance)
            {
                cutPoints.Add(mouseWorldPosition);
                UpdateLineRenderer();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCutting = false;
            evaluator?.EvaluateCut();
        }
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = cutPoints.Count;
        for (int i = 0; i < cutPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, cutPoints[i]);
        }
    }

    public void ClearCut()
    {
        cutPoints.Clear();
        lineRenderer.positionCount = 0;
    }
}