namespace UVS.Services
{
    public interface IDataGeneratorService
    {
        string GenerateRandomString(int length);
    }

    public class DataGeneratorService : IDataGeneratorService
    {
        private static readonly Random _rnd = new();
        public string GenerateRandomString(int length)
        {
            return Guid.NewGuid().ToString("N").Substring(0, length);
        }
    }
}