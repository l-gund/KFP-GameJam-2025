using UnityEngine;

public class ChargerStateManager : MonoBehaviour
{
    [SerializeField] private ChargerState state = ChargerState.Hostile;

    public ChargerState GetState()
    {
        return state;
    }

    public void SetState(ChargerState state)
    {
        this.state = state;
    }
}