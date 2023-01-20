using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("Controller")]
public class StudentConroller
{
    private readonly StudentService _studentService;

    public StudentConroller(StudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet("GetWithDapper")]
    public async Task<List<Student>> GetWithDapper()
    {
        return await _studentService.GetStudentWithDapper();
    }

    [HttpGet("GetWithoutDapper")]
    public async Task<List<Student>> GetWithoutDapper()
    {
        return await _studentService.GetStudentsWithoutDapper();
    }

    [HttpGet("GetEntity")]
    public async Task<List<Student>> GetWithEntity()
    {
        return await _studentService.GetStudentsWithEntity();
    }
    
    [HttpPost]
    public async Task<bool>Add(Student studentss)
    {
        _studentService.AddStudent(studentss);
        return true;

    }
}