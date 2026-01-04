namespace Com.ByteAnalysis.IFacilita.DocuSign.Data.Impl
{
    public class MongoDatabaseSettings : IMongoDatabaseSettings
    {
        public string MongoCollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
