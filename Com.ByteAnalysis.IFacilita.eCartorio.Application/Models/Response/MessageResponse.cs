using System;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response
{
    public class MessageResponse
    {
        public string Remetente { get; set; }

        public string MensagemText { get; set; }

        public DateTime DataHora { get; set; }
    }

    public class MessageDto
    {
        public string From { get; set; }

        public string MessageText { get; set; }

        public DateTime DateTime { get; set; }
    }
}
