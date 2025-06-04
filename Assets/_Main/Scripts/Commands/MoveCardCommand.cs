using _Main.Scripts.CardSystem;
using _Main.Scripts.Commands.Interfaces;
using UnityEngine;

namespace _Main.Scripts.Commands
{
    public class MoveCardCommand : ICommand
    {
        private readonly CardView _card;
        private readonly CardStack _from;
        private readonly CardStack _to;

        public MoveCardCommand(CardView card, CardStack from, CardStack to)
        {
            _card = card;
            _from = from;
            _to = to;
        }

        public void Execute()
        {
            if (!_from.CanTake(_card))
            {
                Debug.LogWarning("Can't move this card, not on top!");
                return;
            }
        
            _from.RemoveCard(_card);
            _to.AddCard(_card);
        }

        public void Undo()
        {
            _to.RemoveCard(_card);

            if (_card == null || !_card.gameObject.activeSelf)
            {
                return;
            }

            _from.AddCard(_card);
        }

        public bool IsValidForUndo()
        {
            return _card != null && _card.gameObject.activeSelf;
        }
    }
}