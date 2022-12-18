using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string[] _textBoxes;
    [SerializeField] private bool _giveItem = false;
    [SerializeField] private ItemType _itemToGive = ItemType.ATK_BOOST;
    private bool _canSkipDialogue = false;
    private bool _skipDialogue = false;
    private bool _hasGivenItem = false;

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Interact()
    {
        StartCoroutine(DisplayText());
    }

    public void NextDialogue()
    {
        if (_canSkipDialogue)
        {
            _skipDialogue = true;
        }
    }

    private IEnumerator DisplayText()
    {
        PrologueControls.IsInteracting = true;
        _canSkipDialogue = false;

        foreach (string textBox in _textBoxes)
        {
            yield return PrologueManager.Instance.PrologueUI.FillTextBox(textBox);
            _canSkipDialogue = true;
            yield return new WaitUntil(() => _skipDialogue);
            _canSkipDialogue = false;
            _skipDialogue = false;
        }

        if (_giveItem && !_hasGivenItem)
        {
            _hasGivenItem = true;
            yield return PrologueManager.Instance.PrologueUI.FillTextBox(Inventory.Instance.PickupItem(_itemToGive));
            _canSkipDialogue = true;
            yield return new WaitUntil(() => _skipDialogue);
            _canSkipDialogue = false;
            _skipDialogue = false;
        }

        PrologueControls.IsInteracting = false;
        PrologueManager.Instance.PrologueUI.ClearTextBox();
    }
}
