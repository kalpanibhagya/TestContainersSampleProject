namespace TestContainerSampleProject.StudentService.Tests
{
    public sealed class StudentTests : IAsyncLifetime
    {
        private Cluster? _cluster;
        private ISession? _session;
        private readonly IContainer _container = new ContainerBuilder()
            .WithImage("new-image")
            .WithName("cassandra-testing-student")
            .WithPortBinding(9042, false)
            .WithBindMount("C:\\Users\\KalpaniR\\Documents\\backup\\student", "/var/lib/cassandra")
            .Build();

        public Task InitializeAsync()
        {
            return _container.StartAsync();
        }

        public Task DisposeAsync()
        {
            return _container.DisposeAsync().AsTask();
        }

        private bool CreateConnection()
        {
            try
            {
                _cluster = Cluster.Builder()
                    .AddContactPoint(_container.Hostname)
                    .Build();

                const int startupDelayMs = 60000; // delay
                Thread.Sleep(startupDelayMs);

                _session = _cluster.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex}");
                return false;
            }
        }

        [Fact]
        public void TestStudentCreation()
        {
            if (CreateConnection())
            {
                var studentService = new TestContainersSampleProject.StudentService(_session);

                // Create a test student
                var student = new Student
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Age = 25
                };

                // Act: Call the CreateStudent method
                studentService.CreateStudent(student);

                // Assert: Query the database to verify student creation
                var selectStatement = _session.Prepare("SELECT * FROM studentdetails.students WHERE student_id = ?");
                var boundStatement = selectStatement.Bind(student.StudentId);
                var row = _session.Execute(boundStatement).FirstOrDefault();

                Assert.NotNull(row);
            }
            else
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void TestStudentListAllStudents()
        {
            if (CreateConnection())
            {
                var studentService = new TestContainersSampleProject.StudentService(_session);
                var studentList = studentService.GetAllStudents().ToList();

                Assert.True(studentList.Count != 0);
            }
            else
            {
                Assert.True(false);
            }

        }

        [Fact]
        public void TestStudentUpdate()
        {
            if (CreateConnection())
            {
                var studentService = new TestContainersSampleProject.StudentService(_session);
                var studentList = studentService.GetAllStudents().ToList();
                var student = new Student
                {
                    StudentId = studentList[0].StudentId,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Age = 28
                };
                Assert.True(studentService.UpdateStudent(student));
            }
            else
            {
                Assert.True(false);
            }

        }
    }
}