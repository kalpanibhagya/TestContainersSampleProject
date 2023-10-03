# TestContainersSampleProject
This project contains an example on how to use ContainerBuilder for generating testcontainers for Cassandra.

TestContainers is a testing library that has support for different languages and frameworks i.e. Java, Dotnet, Ruby, Go, Clojure etc. Using Testcontainers, we can write tests talking to the same type of services you use in production without mocks or in-memory services.

A typical Testcontainers-based test works as follows:

- Start your required services (databases, messaging systems, etc.) in docker containers using Testcontainers API and making the required connections and configurations to connect to the containarized service.

- Run the tests using the new containers.

- After the tests, Testcontainers will take care of destroying those containers irrespective of whether the tests are executed successfully or not.

TestContainers for .Net contains predefined modules for most of the popular databases like MSSql, CosmosDb, MongoDb, etc. and, some other services like RabbitMQ, Elasticsearch, and Redis. However, it still doesn’t contain a module for Cassandra. But we can directly use the ```ContainerBuilder()``` to generate a container instance with mapping predefined Cassandra data volumes.


```
var _container = new ContainerBuilder()
            .WithImage("cassandra:latest")
            .WithName("docker-container-name")
            .WithPortBinding(9042, false)
            .WithResourceMapping(local_data_path, "/var/lib/cassandra")
            .Build();
```

```IAsyncLifetime``` interface in Xunit executes ```InitializeAsync()``` immediately after the test class get created and it is responsible for starting the container and the container is disposed by ```DisposeAsync()``` after executing the test method.
