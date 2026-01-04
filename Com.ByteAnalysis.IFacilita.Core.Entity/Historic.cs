using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class Historic : BasicEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int IdUser { get; set; }
        public DateTime Created { get; set; }
        public int IdTransaction { get; set; }
        public User User { get; set; }
        public Transaction Transaction { get; set; }

        public string Topic { get; set; }
    }
}
