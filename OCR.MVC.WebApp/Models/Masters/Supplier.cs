using System;

namespace OCR.MVC.WebApp.Models
{
    //===================================================​
    // Date :​ 23 Des 2021
    // Author :​ A.M.Lubis
    // Description :​ Model Suppliers
    //===================================================​
    // Revision History:​
    // Name	|Date	|Description |​
    //	    |	    |	         |​
    //===================================================​
    public class Supplier
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NPWP { get; set; }
        public string Telp { get; set; }
        public string Address { get; set; }
        public string PJ { get; set; }
    }
}
