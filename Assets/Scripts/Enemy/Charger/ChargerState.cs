using System;

public enum ChargerState
{
    Following,
    StartingCharge,
    PerformingCharge,
    Charging,
    RecoveringFromCharge,
}

public static class Extensions
{
    public static int ToAnimatorState(this ChargerState state)
    {
        switch (state)
        {
            case ChargerState.Following:
                return 0;
            case ChargerState.StartingCharge:
                return 1;
            case ChargerState.PerformingCharge:
                return 2;
            case ChargerState.Charging:
                return 3;
            case ChargerState.RecoveringFromCharge:
                return 4;
            default:
                throw new ArgumentException(
                    string.Format("Failed to map charger state: {0} to an animator state", state)
                );
        }
    }
}