using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.CardSystem;
using _Main.Scripts.Controllers;
using UnityEngine;

public class CardStack : MonoBehaviour
{
    public bool allowMatch = true;

    private readonly List<CardView> _cards = new();

    private static readonly List<CardStack> AllStacks = new();

    private void OnEnable()
    {
        AllStacks.Add(this);
    }

    private void OnDisable()
    {
        AllStacks.Remove(this);
    }

    public CardView TopCard => _cards.Count > 0 ? _cards[^1] : null;

    public bool CanTake(CardView card)
    {
        return _cards.Count > 0 && _cards[^1] == card;
    }

    public void AddCard(CardView card)
    {
        _cards.Add(card);
        card.transform.SetParent(transform);
        card.transform.localPosition = new Vector3(0, -20 * (_cards.Count - 1), 0);
        card.CurrentStack = this;

        if (allowMatch)
        {
            CheckMatch();
        }
    }

    public void RemoveCard(CardView card)
    {
        if (CanTake(card))
        {
            _cards.Remove(card);
        }

        StartCoroutine(CheckStacksAfterDelay());
    }

    private IEnumerator CheckStacksAfterDelay()
    {
        yield return new WaitForSeconds(0.15f);
        if (AreAllStacksEmpty())
        {
            CheckLevelWin();
        }
    }

    private static bool AreAllStacksEmpty()
    {
        return AllStacks.All(stack => stack._cards.Count == 0);
    }

    private void CheckLevelWin()
    {
        EventController.Trigger(EventNames.OnLevelWin, true);
    }

    private void CheckMatch()
    {
        if (_cards.Count < 3) return;

        var lastThree = _cards.GetRange(_cards.Count - 3, 3);
        if (lastThree.Any(c => c.CardId != lastThree[0].CardId)) return;

        foreach (var cardView in lastThree)
        {
            _cards.Remove(cardView);
            Destroy(cardView.gameObject);
        }

        UpdateCardPositions();
    }

    private void UpdateCardPositions()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].transform.localPosition = new Vector3(0, -30 * i, 0);
        }
    }
}