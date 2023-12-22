using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITypeEffect
{
    // Begin The Typing Effect
    public void Begin(string text);
    // Finish the typing effect inmidiatly
    public void Finish();
    // Check if the typing effect has finished
    public bool Check();
}
