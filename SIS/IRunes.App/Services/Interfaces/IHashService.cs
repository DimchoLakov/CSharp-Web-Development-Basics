namespace IRunes.App.Services.Interfaces
{
    public interface IHashService
    {
        string ComputeSha256Hash(string password);
    }
}
