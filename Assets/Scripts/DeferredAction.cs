using System;

public class DeferredAction
{
    private int ticks;
    private Action action;

    public DeferredAction(int ticks, Action action)
    {
        this.ticks = ticks;
        this.action = action;
    }

    public int GetTicks()
    {
        return ticks;
    }

    public void Tick()
    {
        ticks--;
    }

    public void Execute()
    {
        action();
    }
}