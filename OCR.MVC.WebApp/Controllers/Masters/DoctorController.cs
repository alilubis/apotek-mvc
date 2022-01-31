//===================================================​
// Date :​ 23 DES 2021
// Author :​ A.M.Lubis
// Description : Supplier Controller 
//===================================================​
// Revision History
// Name	     |Date	     |Description               
//           |           |
//===================================================
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCR.MVC.WebApp.Extensions;
using OCR.MVC.WebApp.Models;

namespace OCR.MVC.WebApp.Controllers
{

    public class DoctorController : Controller
    {
        private readonly ANFBContext _db;

        public DoctorController(ANFBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            var result = _db.Doctors.AsQueryable();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) );
            }
            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);
            var filteredResultsCount = await result.CountAsync();
            var totalResultsCount = await _db.Doctors.CountAsync();
            return Json(new DtResult<Doctor>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = await result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToListAsync()
            });
        }

        public JsonResult Edit(int id)
        {
            var dataById = _db.Doctors.First(u => u.Id == id);
            return Json(new
            {
                success = true,
                data = dataById
            });
        }

        [HttpPost]
        public JsonResult AddOrUpdate(Doctor model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    var checkData = _db.Doctors.Where(u => u.Name == model.Name).SingleOrDefault();
                    if (checkData != null)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Dokter sudah terdaftar"
                        });
                    };

                    //var add = new Doctor()
                    //{
                    //    Name = model.Name,
                    //    Specialization = model.Specialization,
                    //    Telp = model.Telp,
                    //    Address = model.Address,
                    //    Email = model.Email
                    //};
                    _db.Doctors.Add(model);
                    _db.SaveChanges();

                    return Json(new
                    {
                        success = true,
                        message = "Data berhasil ditambahkan"
                    });
                }
                else
                {
                    var data = _db.Doctors.First(u => u.Id == model.Id);
                    data.Name = model.Name;
                    data.Specialization = model.Specialization;
                    data.Address = model.Address;
                    data.Telp = model.Telp;
                    data.Email = model.Email;
                    data.DateOfStart = model.DateOfStart;
                    data.DateOfEnd = model.DateOfEnd;
                    data.DayOfWork = model.DayOfWork;
                    _db.SaveChanges();
                    return Json(new
                    {
                        success = true,
                        message = "Data berhasil diupdate"
                    });
                }
            }

            return Json(new
            {
                success = false,
                message = "Error saat update data"
            });
        }

        [HttpPost]
        public JsonResult Delete(int id, string reason)
        {
            //List<Doctor> data = new List<Doctor>();
            List<string> listString = new List<string>();
            var delete = _db.Doctors.First(u => u.Id == id);
            if (delete == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Data tidak ditemukan"
                });
            }
            //data.Add(delete);

            //var columnNames = (from t in typeof(Supplier).GetProperties()
            //                   select t.Name).ToList();

            // audit log
            //var auditLog = new AuditLog()
            //{
            //    AuditLogDateTime = DateTime.Now,
            //    AuditLogTableName = "Department",
            //    AuditLogModuleName = "/Department/Index",
            //    AuditLogAction = "DELETE",
            //    AuditLogReason = reason,
            //    AuditLogRowID = id,
            //    AuditLogRowName = "",
            //    AuditLogRowVersion = "",
            //    AuditLogUserID = HttpContext.Session.GetInt32("userId").Value,
            //    AuditLogUserName = HttpContext.Session.GetString("username"),
            //    AuditLogComputerID = 0,
            //    AuditLogComputerName = Dns.GetHostName(),
            //};
            //_db.Add(auditLog);
            //_db.SaveChanges();
            //// audit log detail
            //foreach (var list in department)
            //{
            //    listString.Add(Convert.ToString(list.Id));
            //    listString.Add(list.Name);
            //}
            //for(int i = 0; i < listString.Count; i++)
            //{
            //    var auditLogField = new AuditLogField()
            //    {
            //        AuditLogID = auditLog.AuditLogID,
            //        AuditLogFieldName = columnNames[i],
            //        AuditLogFieldBefore = listString[i],
            //        AuditLogFieldAfter = ""
            //    };
            //    _db.Add(auditLogField);
            //    _db.SaveChanges();
            //}   
            // delete data
            _db.Remove(delete);
            _db.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Data berhasil dihapus"
            });
        }
    }
}
