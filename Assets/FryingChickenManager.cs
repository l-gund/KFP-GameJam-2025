using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FryingChickenManager : MonoBehaviour
{
    public string[] possibleCharacters = { "K", "I", "A", "R" };

    public TextMeshProUGUI buttonMashText;
    public string characterToMash;
    public GameTimer station3Timer;

    private float characterChangeInterval = 3f;
    private float characterChangeTimer = 0f;

    void Start()
    {
        SetNewCharacterToMash();
        station3Timer.StartPrepTime(20f);
    }

    void Update()
    {
        characterChangeTimer += Time.deltaTime;

        if (characterChangeTimer >= characterChangeInterval)
        {
            SetNewCharacterToMash();
            characterChangeTimer = 0f;
        }

        CheckKeyPress();
    }

    void CheckKeyPress()
    {
        if (characterToMash.Length == 1)
        {
            KeyCode key;
            if (System.Enum.TryParse(characterToMash.ToUpper(), out key))
            {
                if (Input.GetKeyDown(key))
                {
                    station3Timer.RecordHit(characterToMash);
                }
            }
        }
    }

    void SetNewCharacterToMash()
    {
        characterToMash = possibleCharacters[Random.Range(0, possibleCharacters.Length)];
        if (buttonMashText != null)
        {
            buttonMashText.text = characterToMash;
        }
    }
}
