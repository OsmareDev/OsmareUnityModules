using System;

public class Helpers
{
    public static T GetNextInEnum<T>(int value) where T : Enum {
        int nextValue = (int)(object)value + 1;
        if (!Enum.IsDefined(typeof(T), nextValue)) nextValue = 0;
        return (T)(object)nextValue;
    } 
}
