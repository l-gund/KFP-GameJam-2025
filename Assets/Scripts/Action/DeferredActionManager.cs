using System;
using System.Collections.Generic;

public class DeferredActionManager
{
    private List<DeferredAction> deferredActions = new();

    public void OnFixedUpdate()
    {
        for (int i = deferredActions.Count - 1; i >= 0; i--)
        {
            DeferredAction action = deferredActions[i];
            if (action.ShouldExecute())
            {
                action.Execute();
                deferredActions.RemoveAt(i);
                continue;
            }

            action.Tick();
        }
    }

    public void Defer(int ticks, Action action)
    {
        deferredActions.Add(new DeferredAction(ticks, action));
    }
}