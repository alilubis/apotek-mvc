using System;

namespace OCR.MVC.WebApp.Models
{
    //===================================================​
    // Date :​ 04 Jan 2022
    // Author :​ A.M.Lubis
    // Description :​ Model Doctor
    //===================================================​
    // Revision History:​
    // Name	|Date	|Description |​
    //	    |	    |	         |​
    //===================================================​
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string Address { get; set; }
        public string Telp { get; set; }
        public string Email { get; set; }
        public DateTime DateOfStart { get; set; }
        public DateTime DateOfEnd { get; set; }
        public string DayOfWork { get; set; }
    }
}
