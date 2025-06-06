#nullable enable

using UnityEngine;

public class JumperDamage : Damage
{
    // TODO: Bad practice for now, put state into a shared state manager later
    private JumperBehaviour? behaviour;

    protected override void Init()
    {
        base.Init();
        behaviour = GetComponent<JumperBehaviour>();
    }

    public override bool ShouldTrigger(GameObject gameObject)
    {
        return behaviour!.GetState() != JumperState.Jump;
    }
}