namespace Norm.Interfaces
{
    public interface INormExecute
    {
        INorm Execute(string command);
        INorm Execute(string command, params object[] parameters);
        INorm Execute(string command, params (string name, object value)[] parameters);
    }
}