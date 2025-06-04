using _Main.Scripts.Controllers;
using UnityEngine;

namespace _Main.Scripts.UI
{
    public class UndoButton : MonoBehaviour
    {
        public void OnUndoPressed()
        {
            UndoController.Instance.Undo();
        }
    }
}