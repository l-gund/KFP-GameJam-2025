using UnityEngine;

public class JumperStateManager : MonoBehaviour
{
    [SerializeField] private JumperState state = JumperState.Hostile;

    public JumperState GetState()
    {
        return state;
    }

    public void SetState(JumperState state)
    {
        this.state = state;
    }
}