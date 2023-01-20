using System.Diagnostics;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Services;

public class StudentService
{
    private string? _connectionString = "Server=127.0.0.1;Port=5432;Database=GetDb;User Id=postgres;Password=22385564;";
    public StudentService()
    {

    }
    
    private readonly DataContext _context;

    public StudentService(DataContext context)
    {
        _context = context;
    }
    
    public async  Task<List<Student>> GetStudentWithDapper()
    {
        var sw = new Stopwatch();
        sw.Start();
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            string sql = "SELECT * FROM \"Students\" ";
            var result = await connection.QueryAsync<Student>(sql);

            sw.Stop();
            Console.WriteLine($"Elapsed Times with dapper /  {sw.ElapsedMilliseconds}");
            return result.ToList();
        }

    }
    public async Task<List<Student>> GetStudentsWithoutDapper()
    {
        
        string sql = "SELECT * FROM \"Students\" " +
                     "";
        var students = new List<Student>();
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            var sw = new Stopwatch();
            sw.Start();
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                using (var reader =await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var student = new Student()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                            LastName = reader.GetString(reader.GetOrdinal("lastname")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            Age = reader.GetInt32(reader.GetOrdinal("age")),
                        };
                        students.Add(student);
                    }
                }
            }
            sw.Stop();
            System.Console.WriteLine($"Elapsed Times without dapper /  {sw.ElapsedMilliseconds}");
            connection.Close();
        }

        return students;
    }
    
    public async Task<List<Student>> GetStudentsWithEntity()
    {
        var S = new Stopwatch();
        S.Start();
        var t = await _context.Students.Select(x => new Student(x.FirstName,x.LastName,x.Age,x.Email)).ToListAsync();
        S.Stop();
        Console.WriteLine($"Entity / {S.ElapsedMilliseconds}");
        return  t;
    }
    
    public async Task AddStudent(Student studentss)
    {

        for (int i = 0; i < 1000; i++)
        {
            var t = new Student(studentss.FirstName,studentss.LastName,studentss.Age,studentss.Email);
            
            _context.Students.Add(t);
        }
        
        await _context.SaveChangesAsync();
    }
    
}