using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CerberusMultiBranch.Support
{    
    public class Record
    {
        [XmlElement("Codigo")]
        public string Code { get; set; }

        [XmlElement("Categoria")]
        public string Category { get; set; }

        [XmlElement("Descripcion")]
        public string Description { get; set; }

        [XmlElement("Precio")]
        public double Price { get; set; }

        [XmlElement("Marca")]
        public string TradeMark { get; set; }

        [XmlElement("Unidad")]
        public string Unit { get; set; }
    }

    [XmlRoot("ProductList")]
    public class RecordSet
    {
        [XmlElement("Product")]
        public List<Record> Records { get; set; }

        public RecordSet()
        {
            this.Records = new List<Record>();
        }
    }

    public class ProdToAdd
    {
        public int ProviderId { get; set; }

     
        public string Code { get; set; }

        
        public string Category { get; set; }

        
        public string Description { get; set; }

        
        public double Price { get; set; }

        
        public string TradeMark { get; set; }

        public string Unit { get; set; }


        public int? ProductId { get; set; }
    }
}