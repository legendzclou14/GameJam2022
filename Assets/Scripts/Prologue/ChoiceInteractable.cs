using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChoiceInteractable : Interactable
{
    [SerializeField] private bool _choiceAfterBoss = false;
    [SerializeField] private string _choiceText = "";
    [SerializeField] private string[] _choices;
    private bool _inChoice = false;

    public override void Interact()
    {
        StartCoroutine(DisplayEndText());
    }

    public override void NextDialogue()
    {
        base.NextDialogue();

        if (_inChoice)
        {
            ProcessChoice(PrologueManager.Instance.PrologueUI.SelectButton());
            _inChoice = false;
        }
    }

    private IEnumerator DisplayEndText()
    {
        yield return DisplayText();

        if(_choiceAfterBoss || !Inventory.Instance.HasBeatenBoss)
        {
            PrologueControls.IsInteracting = true;

            yield return PrologueManager.Instance.PrologueUI.FillTextBox(_choiceText);
            _inChoice = true;
            PrologueManager.Instance.PrologueUI.ShowChoiceButtons(_choices);

            yield return new WaitUntil(() => !_inChoice);
            PrologueManager.Instance.PrologueUI.ClearTextBox();
            PrologueControls.IsInteracting = false;
        }
    }

    public abstract void ProcessChoice(int index);
}
