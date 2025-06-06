using System;

public enum ChargerState
{
    Hostile,
    ChargeStartup,
    Charge,
    ChargeRecovery,
}

public static class Extensions
{
    public static int ToAnimatorState(this ChargerState state)
    {
        switch (state)
        {
            case ChargerState.Hostile:
                return 0;
            case ChargerState.ChargeStartup:
                return 1;
            case ChargerState.Charge:
                return 2;
            case ChargerState.ChargeRecovery:
                return 3;
            default:
                throw new ArgumentException(
                    string.Format("Failed to map charger state: {0} to an animator state", state)
                );
        }
    }
}