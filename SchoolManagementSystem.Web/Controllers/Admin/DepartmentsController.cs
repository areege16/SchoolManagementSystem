using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Admin.Departments.Commands.CreateDepartment;
using SchoolManagementSystem.Application.Admin.Departments.Commands.DeleteDepartment;
using SchoolManagementSystem.Application.Admin.Departments.Commands.UpdateDepartment;
using SchoolManagementSystem.Application.Admin.Departments.Queries.GetAllDepartments;
using SchoolManagementSystem.Application.Admin.Departments.Queries.GetDepartmentById;
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Web.Extensions;

namespace SchoolManagementSystem.Web.Controllers.Admin
{
    [Route("api/admin/[controller]")]
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
        public async Task<IActionResult> CreateDepartment(CreateDepartmentDto departmentDto)
        {
            var adminId = User.GetUserId();

            var result = await mediator.Send(new CreateDepartmentCommand
            {
                DepartmentDto = departmentDto,
                AdminId = adminId
            });
            return Ok(result);
        }
        #endregion

        #region DeleteDepartment
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await mediator.Send(new DeleteDepartmentCommand { Id = id });
            return Ok(result);
        }
        #endregion

        #region UpdatedDepartment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, UpdateDepartmentDto departmentDto)
        {
            departmentDto.Id = id;
            var adminId = User.GetUserId();

            var result = await mediator.Send(new UpdateDepartmentCommand
            {
                DepartmentDto = departmentDto,
                AdminId = adminId,
            });
            return Ok(result);
        }
        #endregion

        #region GetAllDepartments
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await mediator.Send(new GetAllDepartmentsQuery());
            return Ok(result);
        }
        #endregion

        #region GetDepartmentById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var result = await mediator.Send(new GetDepartmentByIdQuery
            {
                Id = id
            });
            return Ok(result);
        }
        #endregion
    }
}