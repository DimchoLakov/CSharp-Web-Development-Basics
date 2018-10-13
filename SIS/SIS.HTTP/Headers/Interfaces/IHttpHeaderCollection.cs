namespace SIS.HTTP.Headers.Interfaces
{
    public interface IHttpHeaderCollection
    {
        void Add(HttpHeader header);

        bool ContainsKey(string key);

        HttpHeader GetHeader(string key);
    }
}
