using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float prepTimeDuration = 10f;

    private float prepTimeRemaining;
    private float elapsedTimer;
    private bool prepTimeActive = false;
    private bool completedTimer = false;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI buttonMash;

    private bool miniGameObjectsChecked = false;
    public GameObject[] miniGameObjects;

    void Update()
    {
        if (!prepTimeActive) return;

        prepTimeRemaining -= Time.deltaTime;

        if (prepTimeRemaining <= 0f)
        {
            prepTimeRemaining = 0f;
            prepTimeActive = false;
            completedTimer = true;

            if (!miniGameObjectsChecked)
            {
                CheckAndDeactivateMiniGameObjects();
                miniGameObjectsChecked = true;
            }
        }
        else
        {
            elapsedTimer += Time.deltaTime;
        }

        UpdateTimerText();
    }

    public void StartPrepTime(float customDuration)
    {
        prepTimeRemaining = customDuration;
        prepTimeActive = true;
        completedTimer = false;
    }

    public void ResetTimer()
    {
        elapsedTimer = 0f;
        prepTimeRemaining = prepTimeDuration;
        prepTimeActive = false;
        completedTimer = false;
        miniGameObjectsChecked = false;
    }

    public bool IsPrepTimeActive() => prepTimeActive;
    public bool HasCompletedPrepTime() => completedTimer;
    public float GetElapsedTime() => elapsedTimer;

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = prepTimeActive ? $"Prep Time: {prepTimeRemaining:F1}s" : "";
        }

        if (!prepTimeActive && buttonMash != null)
        {
            buttonMash.text = "";
        }
    }

    void CheckAndDeactivateMiniGameObjects()
    {
        foreach (var obj in miniGameObjects)
        {
            if (obj != null && obj.activeSelf)
            {
                Debug.Log($"Deactivating {obj.name}");
                obj.SetActive(false);
            }
        }
    }

    public void RecordHit(string hitType)
    {
        if (!prepTimeActive) return;

        switch (hitType)
        {
            case "Perfect": prepTimeRemaining = Mathf.Max(0, prepTimeRemaining - 3f); break;
            case "Okay": prepTimeRemaining = Mathf.Max(0, prepTimeRemaining - 2f); break;
            case "Bad": break;
        }

        if (hitType == "K" || hitType == "I" || hitType == "A" || hitType == "R")
        {
            prepTimeRemaining = Mathf.Max(0, prepTimeRemaining - 0.1f);
        }
    }
}

