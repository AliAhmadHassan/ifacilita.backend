namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Entity
{
    public class MongoDatabaseSettings : IMongoDatabaseSettings
    {
        public string MongoCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMongoDatabaseSettings
    {
        string MongoCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
