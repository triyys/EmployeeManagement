using Dapper;
using EmployeeManagementWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString("EMAppConn");
        }

        // GET api/department
        [HttpGet]
        public JsonResult Get()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                string query = "dbo.spDepartment_GetAll";

                var result = connection.Query(query);

                return new JsonResult(result);
            }
        }

        // POST api/department
        [HttpPost]
        public JsonResult Post(DepartmentModel department)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                string query = $"insert into dbo.Department values (N'{ department.Name }')";

                var result = $"Added successfully { connection.Execute(query) } department(s)";

                return new JsonResult(result);
            }
        }

        // PUT api/department
        [HttpPut]
        public JsonResult Put(DepartmentModel department)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                string query = $"update dbo.Department set Name = N'{ department.Name }' where Id = { department.Id }";

                var result = $"Updated successfully { connection.Execute(query) } department(s)";

                return new JsonResult(result);
            }
        }

        // DELETE api/department/3
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                string query = $"delete from dbo.Department where Id = { id }";

                var result = $"Deleted successfully { connection.Execute(query) } department(s)";

                return new JsonResult(result);
            }
        }
    }
}
