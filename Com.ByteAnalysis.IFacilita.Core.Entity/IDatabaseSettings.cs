namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
    }

    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
    }
}