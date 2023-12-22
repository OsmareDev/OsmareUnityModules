using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct choiceAnswer {
    public string choice;
    public BasicNode answer;
}


[CreateAssetMenu(menuName = "Dialogue/ChoiceNode")]
public class ChoiceNode : BasicNode
{   
    public choiceAnswer[] choices;
}