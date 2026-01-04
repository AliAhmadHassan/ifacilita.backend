using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class TransactionCertificationMap: EntityMap<Entity.TransactionCertification>
    {
        internal TransactionCertificationMap()
        {
            Map(u => u.BuyerAccept).ToColumn("buyer_accept");
            Map(u => u.BuyerRefuse).ToColumn("buyer_refuse");
            Map(u => u.BuyerResponse).ToColumn("buyer_response");
            Map(u => u.BuyerSeen).ToColumn("buyer_seen");
            Map(u => u.BuyerWeSolve).ToColumn("buyer_we_solve");
            Map(u => u.BuyerWhyRejected).ToColumn("buyer_why_rejected");

            Map(u => u.SellerAccept).ToColumn("seller_accept");
            Map(u => u.SellerRefuse).ToColumn("seller_refuse");
            Map(u => u.SellerResponse).ToColumn("seller_response");
            Map(u => u.SellerSeen).ToColumn("seller_seen");
            Map(u => u.SellerWeSolve).ToColumn("seller_we_solve");
            Map(u => u.SellerWhyRejected).ToColumn("seller_why_rejected");

            Map(u => u.CertificateName).ToColumn("certificate_name");
            Map(u => u.CertificateFilename).ToColumn("certificate_filename");
            Map(u => u.CertificatePath).ToColumn("certificate_path");

            Map(u => u.ExpirationDate).ToColumn("expiration_date");

            Map(u => u.Id).ToColumn("id");
            Map(u => u.Idtransaction).ToColumn("idtransaction");

            Map(u => u.Received).ToColumn("received");
            Map(u => u.ReceiptForecast).ToColumn("receipt_forecast");
            Map(u => u.Value).ToColumn("value");
            Map(u => u.EcartorioId).ToColumn("ecartorio_id");
            Map(u => u.Notary).ToColumn("notary");
        }
    }
}
