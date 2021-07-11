namespace ShopBridgeAPI
{
    using System;
    
    public partial class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<System.DateTime> created { get; set; }
    }
}
