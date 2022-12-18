using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string[] _textBoxesBeforeBoss;
    [SerializeField] private bool _differentTextsAfterBoss = false;
    [SerializeField] private string[] _textBoxesAfterBoss;
    private string[] _textBoxesToRead;
    [SerializeField] private bool _giveItem = false;
    [SerializeField] private ItemType _itemToGive = ItemType.ATK_BOOST;
    protected bool _canSkipDialogue = false;
    protected bool _skipDialogue = false;
    private bool _hasGivenItem = false;

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        _textBoxesToRead = _textBoxesBeforeBoss;
        if (Inventory.Instance.HasBeatenBoss)
        {
            _hasGivenItem = true;
            if (_differentTextsAfterBoss)
            {
                _textBoxesToRead = _textBoxesAfterBoss;
            }
        }
    }

    public virtual void Interact()
    {
        StartCoroutine(DisplayText());
    }

    public virtual void NextDialogue()
    {
        if (_canSkipDialogue)
        {
            _skipDialogue = true;
        }
    }

    protected IEnumerator DisplayText()
    {
        PrologueControls.IsInteracting = true;
        _canSkipDialogue = false;

        foreach (string textBox in _textBoxesToRead)
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
