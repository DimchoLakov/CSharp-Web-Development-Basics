namespace ByTheCakeApp.Services.Interfaces
{
    public interface IHashService
    {
        string ComputeSha256Hash(string rawData);
    }
}
