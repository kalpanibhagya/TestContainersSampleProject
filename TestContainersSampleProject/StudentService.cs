using Cassandra;

namespace TestContainersSampleProject;

public class StudentService
{
    private readonly ISession _session;

    public StudentService(ISession session)
    {
        _session = session;
    }

    public void CreateStudent(Student student)
    {
        // Generate a new UUID for the student
        student.StudentId = Guid.NewGuid();

        var insertStatement = _session.Prepare("INSERT INTO studentdetails.students (student_id, first_name, last_name, age) VALUES (?, ?, ?, ?)");
        var boundStatement = insertStatement.Bind(student.StudentId, student.FirstName, student.LastName, student.Age);

        _session.Execute(boundStatement);
    }

    public Student GetStudent(Guid studentId)
    {
        var selectStatement = _session.Prepare("SELECT * FROM studentdetails.students WHERE student_id = ?");
        var boundStatement = selectStatement.Bind(studentId);

        var row = _session.Execute(boundStatement).FirstOrDefault();

        if (row != null)
        {
            return new Student
            {
                StudentId = row.GetValue<Guid>("student_id"),
                FirstName = row.GetValue<string>("first_name"),
                LastName = row.GetValue<string>("last_name"),
                Age = row.GetValue<int>("age")
            };
        }

        return null;
    }

    public IEnumerable<Student> GetAllStudents()
    {
        var selectStatement = _session.Prepare("SELECT * FROM studentdetails.students");
        var boundStatement = selectStatement.Bind();

        var rows = _session.Execute(boundStatement);

        var students = new List<Student>();

        foreach (var row in rows)
        {
            students.Add(new Student
            {
                StudentId = row.GetValue<Guid>("student_id"),
                FirstName = row.GetValue<string>("first_name"),
                LastName = row.GetValue<string>("last_name"),
                Age = row.GetValue<int>("age")
            });
        }

        return students;
    }


    public bool UpdateStudent(Student student)
    {
        var updateStatement = _session.Prepare("UPDATE studentdetails.students SET first_name = ?, last_name = ?, age = ? WHERE student_id = ?");
        var boundStatement = updateStatement.Bind(student.FirstName, student.LastName, student.Age, student.StudentId);

        try
        {
            _session.Execute(boundStatement);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void DeleteStudent(Guid studentId)
    {
        var deleteStatement = _session.Prepare("DELETE FROM studentdetails.students WHERE student_id = ?");
        var boundStatement = deleteStatement.Bind(studentId);

        _session.Execute(boundStatement);
    }
}
