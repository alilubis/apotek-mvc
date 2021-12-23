using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCR.MVC.WebApp.Extensions;
using OCR.MVC.WebApp.Models;

//===================================================​
// Date :​ 23 DES 2021
// Author :​ A.M.Lubis
// Description : Supplier Controller 
//===================================================​
// Revision History
// Name	     |Date	     |Description               
//           |           |
//===================================================

namespace OCR.MVC.WebApp.Controllers
{

    public class SupplierController : Controller
    {
        private readonly ANFBContext _db;

        public SupplierController(ANFBContext db)
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
            var result = _db.Suppliers.AsQueryable();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) );
            }
            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);
            var filteredResultsCount = await result.CountAsync();
            var totalResultsCount = await _db.Suppliers.CountAsync();
            return Json(new DtResult<Supplier>
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
            var dataJobTitleById = _db.Suppliers.First(u => u.Id == id);
            return Json(new
            {
                success = true,
                data = dataJobTitleById
            });
        }

        [HttpPost]
        public JsonResult AddOrUpdate(Supplier model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    var checkData = _db.Suppliers.Where(u => u.Name == model.Name).SingleOrDefault();
                    if (checkData != null)
                    {
                        return Json(new
                        {
                            success = false,
                            type = true,
                            message = "Supplier name already exist"
                        });
                    };
                    var add = new Supplier()
                    {
                        Code = model.Code,
                        Name = model.Name,
                        NPWP = model.NPWP,
                        Telp = model.Telp,
                        Address = model.Address,
                        PJ = model.PJ
                    };
                    _db.Suppliers.Add(add);
                    _db.SaveChanges();

                    return Json(new
                    {
                        success = true,
                        message = "Data added successfully"
                    });
                }
                else
                {
                    var departmentUpdate = _db.Suppliers.First(u => u.Id == model.Id);
                    var departmentNameBefore = departmentUpdate.Name;
                    departmentUpdate.Name = model.Name;
                    _db.SaveChanges();
                    //if(departmentNameBefore != name) 
                    //{ 
                    //    // audit log
                    //    var auditLog = new AuditLog()
                    //    {
                    //        AuditLogDateTime = DateTime.Now,
                    //        AuditLogTableName = "Department",
                    //        AuditLogModuleName = "/Department/Index",
                    //        AuditLogAction = "UPDATE",
                    //        AuditLogReason = reason,
                    //        AuditLogRowID = id,
                    //        AuditLogRowName = "",
                    //        AuditLogRowVersion = "",
                    //        AuditLogUserID = HttpContext.Session.GetInt32("userId").Value,
                    //        AuditLogUserName = HttpContext.Session.GetString("username"),
                    //        AuditLogComputerID = 0,
                    //        AuditLogComputerName = Dns.GetHostName(),
                    //    };
                    //    _db.Add(auditLog);
                    //    _db.SaveChanges();

                    //    // audit log detail
                    //    var auditLogField = new AuditLogField()
                    //    {
                    //        AuditLogID = auditLog.AuditLogID,
                    //        AuditLogFieldName = "Name",
                    //        AuditLogFieldBefore = departmentNameBefore,
                    //        AuditLogFieldAfter = name
                    //    };
                    //    _db.Add(auditLogField);
                    //    _db.SaveChanges();
                    //}

                    return Json(new
                    {
                        success = true,
                        message = "Data updated successfully"
                    });
                }
            }

            return Json(new
            {
                success = false,
                message = "Failed to added or updated data"
            });
        }

        [HttpPost]
        public JsonResult Delete(int id, string reason)
        {
            List<Supplier> department = new List<Supplier>();
            List<string> listString = new List<string>();
            var departmentDelete = _db.Suppliers.First(u => u.Id == id);
            if (departmentDelete == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Data not found"
                });
            }
            department.Add(departmentDelete);

            var columnNames = (from t in typeof(Supplier).GetProperties()
                               select t.Name).ToList();

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
            _db.Remove(departmentDelete);
            _db.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Data deleted successfully"
            });
        }
    }
}
