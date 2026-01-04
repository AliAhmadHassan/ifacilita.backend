using System;

namespace Com.ByteAnalysis.IFacilita.Chat.Model
{
    public class Message
    {
        public int TransactionId { get; set; }

        public User UserFrom { get; set; }
        
        public DateTime? MessageDate { get; set; }
        
        public string MessageText { get; set; }
        
        public User UserTo { get; set; }
    }
}
