using UnityEngine;

public class SpawnSettings
{
    [SerializeField] private int ticks;
    [SerializeField] private int maxObjects;

    public SpawnSettings(int ticks, int maxObjects)
    {
        this.ticks = ticks;
        this.maxObjects = maxObjects;
    }

    public int GetTicks()
    {
        return ticks;
    }

    public int GetMaxObjects()
    {
        return maxObjects;
    }
}