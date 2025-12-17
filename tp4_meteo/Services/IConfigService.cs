namespace tp4_meteo.Services
{
    public interface IConfigService
    {
        string ApiKey { get; set; }
        string Language { get; set; }
        void Save();
    }
}