#nullable enable

using UnityEngine;

public class JumperDamage : Damage
{
    private JumperStateManager? stateManager;

    protected override void Init()
    {
        base.Init();
        stateManager = GetComponent<JumperStateManager>();
    }

    public override bool ShouldTrigger(GameObject gameObject)
    {
        return stateManager!.GetState() != JumperState.Jump;
    }
}