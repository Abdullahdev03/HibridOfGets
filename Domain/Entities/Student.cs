public class Student
{
    public int  Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }

    public Student()
    {
        
    }

    public Student( string firstName, string lastName, int age, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Email = email;
    }
}