namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Entity
{
    public enum CertiticateType
    {
        SearchProtest,
        DefectsDefined,
        TaxDebts,
        IptuDebts,
        PropertyRegistrationData,
        RealOnus
    }

    public enum Coverage
    {
        LAST5YEAR,
        LAST10YEAR
    }

    public enum Expedition
    {
        DIGITAL,
        PAPER
    }

    public enum PersonType
    {
        PHYSICAL,
        LEGAL
    }

    public enum DocumentType
    {
        RG,
        RGE
    }

    public enum GenderType
    {
        Female,
        Male
    }

    public enum OrderStatus
    {
        Generated,
        WaitCertificate,
        Finish
    }

    public enum ApiProcessStatus
    {
        Waiting,
        Processing,
        ErrorOnProcessing,
        Finished,
        Delivered,
        ErrorOnCallback
    }
}
