using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Admin.Departments.Commands.CreateDepartment;
using SchoolManagementSystem.Application.Admin.Departments.Commands.DeleteDepartment;
using SchoolManagementSystem.Application.Admin.Departments.Commands.UpdateDepartment;
using SchoolManagementSystem.Application.Admin.Departments.Queries;
using SchoolManagementSystem.Application.Admin.Departments.Queries.GetAllDepartments;
using SchoolManagementSystem.Application.Admin.Departments.Queries.GetDepartmentById;
using SchoolManagementSystem.Application.DTOs.Department;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Web.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public DepartmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        #region CreateNewDepartment
        [HttpPost]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentDto createDepartmentDto)
        {
            var result = await mediator.Send(new CreateDepartmentCommand
            {
                DepartmentDto = createDepartmentDto
            });
            return Ok(result);
        }
        #endregion

        #region DeleteDepartment
        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await mediator.Send(new DeleteDepartmentCommand { Id = id });
            return Ok(result);
        }
        #endregion

        #region UpdatedDepartment
        [HttpPut]
        public async Task<IActionResult> UpdateDepartment(int id, UpdateDepartmentDto updateDepartmentDto)
        {
            updateDepartmentDto.Id = id;
            var result = await mediator.Send(new UpdateDepartmentCommand
            {
                DepartmentDto = updateDepartmentDto
            });
            return Ok(result);
        }
        #endregion

        #region GetAllDepartments
        [HttpGet("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await mediator.Send(new GetAllDepartmentsQuery());
            return Ok(result);
        }
        #endregion

        #region GetDepartmentById
        [HttpGet("GetDepartmentById")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var result = await mediator.Send(new GetDepartmentByIdQuery { Id = id });
            return Ok(result);
        } 
        #endregion
    }
}
