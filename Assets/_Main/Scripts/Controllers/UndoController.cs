using System.Collections.Generic;
using _Main.Scripts.Commands;
using _Main.Scripts.Commands.Interfaces;
using _Main.Scripts.Utils;

namespace _Main.Scripts.Controllers
{
    public class UndoController : BaseSingleton<UndoController>
    {
        private readonly Stack<ICommand> _history = new();

        private void Start()
        {
            SubToEvents();
        }

        private void SubToEvents()
        {
            EventController.Subscribe<bool>(EventNames.OnLevelWin, OnLevelWin);
        }
        
        private void OnLevelWin(bool obj)
        {
            ClearHistory();
        }

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _history.Push(command);
        }

        public void Undo()
        {
            while (_history.Count > 0)
            {
                var command = _history.Pop();

                if (command is MoveCardCommand moveCmd)
                {
                    if (!moveCmd.IsValidForUndo()) continue;
                    moveCmd.Undo();
                }
                else
                {
                    command.Undo();
                }

                return;
            }
        }
     
        private void ClearHistory()
        {
            _history.Clear();
        }

    }
}