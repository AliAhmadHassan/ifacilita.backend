namespace Com.ByteAnalysis.IFacilita.eCartorio.Data
{
    public interface IMongoDatabaseSettings
    {
        string MongoCollectionName { get; set; }

        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}
