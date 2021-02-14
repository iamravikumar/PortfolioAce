using System;
using System.Runtime.Serialization;

namespace PortfolioAce.DataCentre.DeserialisedObjects
{
    public abstract class AVSecurityPriceData
    {
        public abstract DateTime TimeStamp { get; set; }
        public abstract decimal Close { get; set; }
        public string PriceSource { get { return "AlphaVantage"; } }

    }

    public class AVEquityPriceData : AVSecurityPriceData
    {
        public override DateTime TimeStamp { get; set; }
        public override decimal Close { get; set; }
    }
    [DataContract]
    public class AVCryptoPriceData : AVSecurityPriceData
    {
        // I only support Crypto Assets valued in USD at the moment
        [DataMember(Name = "timestamp")]
        public override DateTime TimeStamp { get; set; }
        [DataMember(Name = "Close (USD)")]
        public override decimal Close { get; set; }
    }

    [DataContract]
    public class AVFXPriceData : AVSecurityPriceData
    {
        [DataMember(Name = "timestamp")]
        public override DateTime TimeStamp { get; set; }
        [DataMember(Name = "close")]
        public override decimal Close { get; set; }
    }

}
