using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfPrologueInteractable : ChoiceInteractable
{
    public override void ProcessChoice(int index)
    {
        switch(index)
        {
            case 0:
                PrologueManager.Instance.EndPrologue();
                break;

            case 1:
                Debug.Log("Get out of choice");
                break;
        }
    }
}
