namespace Com.ByteAnalysis.IFacilita.IptuDebit.Repository
{
    public interface IRequisitionRepository
    {
        Model.Requisition CreateOrUpdate(Model.Requisition requisition);

        Model.Requisition GetUnprocessed();

        Model.Requisition Get(string id);
    }
}
