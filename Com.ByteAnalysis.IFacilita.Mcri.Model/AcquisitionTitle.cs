namespace Com.ByteAnalysis.IFacilita.Mcri.Model
{
    public class AcquisitionTitle
    {
        public AcquisitionTitleType Document { get; set; }

        public string Registry { get; set; }

        /// <summary>
        /// Ofício
        /// </summary>
        public string Trade { get; set; }

        public string Book { get; set; }

        public string Leaf { get; set; }

        /// <summary>
        /// DD/MM/YYYY
        /// </summary>
        public string Date { get; set; }

        public decimal Value { get; set; }

        public decimal TransferTax { get; set; }

        public int GuideNumber { get; set; }

    }
}
