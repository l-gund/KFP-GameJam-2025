using System;

public enum JumperState
{
    Hostile,
    JumpStartup,
    Jump,
    JumpRecovery,
}

public static class DiverStateExtensions
{
    public static int ToAnimatorState(this JumperState state)
    {
        switch (state)
        {
            case JumperState.Hostile:
                return 0;
            case JumperState.JumpStartup:
                return 1;
            case JumperState.Jump:
                return 2;
            case JumperState.JumpRecovery:
                return 3;
            default:
                throw new ArgumentException(
                    string.Format("Failed to map jumper state: {0} to an animator state", state)
                );
        }
    }
}