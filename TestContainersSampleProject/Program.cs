using Cassandra;

namespace TestContainersSampleProject;

class Program
{
    static void Main(string[] args)
    {
        // Initialize Cassandra cluster and session as mentioned earlier
        // Define the Cassandra cluster
        var cluster = Cluster.Builder()
            .AddContactPoint("127.0.0.1") // Replace with your Cassandra cluster's contact point
            .Build();

        // Create a session
        var session = cluster.Connect();

        // Create the keyspace if it doesn't exist
        session.Execute(@"
            CREATE KEYSPACE IF NOT EXISTS StudentDetails
            WITH replication = {
                'class': 'SimpleStrategy',
                'replication_factor': 1
            }");


        session.Execute(@"
        CREATE TABLE IF NOT EXISTS StudentDetails.students (
            student_id UUID PRIMARY KEY,
            first_name TEXT,
            last_name TEXT,
            age INT
        )");
    }
}