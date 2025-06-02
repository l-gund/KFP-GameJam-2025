using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutEvaluator : MonoBehaviour
{
    public PlayerControllerCooking player;
    public IdealCutLine[] idealLines;
    public float perfectThreshold = 0.1f;
    public float goodThreshold = 0.25f;
    public GameTimer station1Timer;

    public void EvaluateCut()
    {
        if (idealLines.Length == 0 || player.cutPoints.Count == 0)
        {
            Debug.Log("No ideal lines or no cut made.");
            return;
        }

        IdealCutLine bestLine = null;
        float bestScore = Mathf.Infinity;

        foreach (var line in idealLines)
        {
            float score = GetAverageDistance(line.startPoint, line.endPoint);
            if (score < bestScore)
            {
                bestScore = score;
                bestLine = line;
            }
        }

        Debug.Log($"Best match avg distance: {bestScore:F3}");

        if (bestScore < perfectThreshold)
            station1Timer.RecordHit("Perfect");
        else if (bestScore < goodThreshold)
            station1Timer.RecordHit("Okay");
        else
            Debug.Log("NoTimerlol");
    }

    float GetAverageDistance(Vector2 start, Vector2 end)
    {
        float totalDistance = 0f;
        foreach (var point in player.cutPoints)
        {
            totalDistance += DistanceFromPointToLine(point, start, end);
        }
        return totalDistance / player.cutPoints.Count;
    }

    float DistanceFromPointToLine(Vector2 point, Vector2 a, Vector2 b)
    {
        float lengthSquared = (b - a).sqrMagnitude;
        if (lengthSquared == 0f) return Vector2.Distance(point, a);

        float t = Mathf.Clamp01(Vector2.Dot(point - a, b - a) / lengthSquared);
        Vector2 projection = a + t * (b - a);
        return Vector2.Distance(point, projection);
    }
}