using Dapper;
using EmployeeManagementWebAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly string connectionString;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;

            connectionString = _configuration.GetConnectionString("EMAppConn");
        }

        // GET api/employee
        [HttpGet]
        public JsonResult Get()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                string query = "dbo.spEmployee_GetAll";

                var result = connection.Query(query);

                return new JsonResult(result);
            }
        }

        // POST api/employee
        [HttpPost]
        public JsonResult Post(EmployeeModel employee)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                string query = $"insert into dbo.Employee (Name, DepartmentId, DateOfJoining, PhotoFileName) values " +
                    $"(" +
                    $"N'{ employee.Name }'," +
                    $"{ employee.DepartmentId }," +
                    $"'{ employee.DateOfJoining }'," +
                    $"N'{ employee.PhotoFileName }'" +
                    $")";

                var result = $"Added successfully { connection.Execute(query) } employee(s)";

                return new JsonResult(result);
            }
        }

        // PUT api/employee
        [HttpPut]
        public JsonResult Put(EmployeeModel employee)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                string query = $"update dbo.Employee set " +
                    $"Name = N'{ employee.Name }'," +
                    $"DepartmentId = { employee.DepartmentId }," +
                    $"DateOfJoining = '{ employee.DateOfJoining }'," +
                    $"PhotoFileName = N'{ employee.PhotoFileName }' " +
                    $"where Id = { employee.Id }";

                var result = $"Updated successfully { connection.Execute(query) } employee(s)";

                return new JsonResult(result);
            }
        }

        // DELETE api/employee/3
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                string query = $"delete from dbo.Employee where Id = { id }";

                var result = $"Deleted successfully { connection.Execute(query) } employee(s)";

                return new JsonResult(result);
            }
        }

        // POST api/employee/savefile
        [Route("savefile")]
        [HttpPost]
        public async Task<JsonResult> SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = $"{ _env.ContentRootPath }/Photos/{ fileName }";

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await postedFile.CopyToAsync(stream);
                }

                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

        [Route("get-all-department")]
        [HttpGet]
        public JsonResult GetAllDepartmentNames()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                string query = "select Name from dbo.Department";

                var result = connection.Query(query);

                return new JsonResult(result);
            }
        }
    }
}
