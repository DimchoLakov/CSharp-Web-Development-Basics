namespace SIS.HTTP.Sessions.Interfaces
{
    public interface IHttpSession
    {
        string Id { get; }

        object GetParameter(string name);

        bool ContainsParameter(string name);

        void AddParameter(string name, object parameter);

        void ClearParameters();
    }
}
