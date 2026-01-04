namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface IPatrimonyService: ICrudService<Entity.Patrimony, int>
    {
        void ItbiRobot(Entity.Transaction transaction, bool approved = false);
        void ItbiRobotApproved(Entity.Transaction transaction);
        object FindItbiRobot(string id);
        object FindItbiSpRobot(string id);
    }
}
