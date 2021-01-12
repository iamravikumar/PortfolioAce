using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PortfolioAce.DataCentre.DeserialisedObjects
{
    public abstract class AVSecurityPriceData
    {
        public abstract DateTime TimeStamp { get; set; }
        public abstract decimal Close {get;set;}
    }

    public class AVEquityPriceData:AVSecurityPriceData
    {
        public override DateTime TimeStamp { get; set; }
        public override decimal Close { get; set; }
    }
    [DataContract]
    public class AVCryptoPriceData:AVSecurityPriceData
    {
        // I only support Crypto Assets valued in USD at the moment
        [DataMember(Name = "timestamp")]
        public override DateTime TimeStamp { get; set; }
        [DataMember(Name="Close (USD)")]
        public override decimal Close { get; set; }
    }

    [DataContract]
    public class AVFXPriceData:AVSecurityPriceData
    {
        public override DateTime TimeStamp { get; set; }
        public override decimal Close { get; set; }
    }

}
