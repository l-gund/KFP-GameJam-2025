using UnityEngine;

public interface Triggerable
{
    bool ShouldTrigger(GameObject gameObject);

    void Trigger(GameObject gameObject);
}