namespace _Main.Scripts.Commands.Interfaces
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
