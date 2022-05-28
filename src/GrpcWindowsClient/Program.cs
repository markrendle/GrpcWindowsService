using Grpc.Net.Client;
using GrpcWindowsService;

try
{
    var channel = GrpcChannel.ForAddress("http://localhost:5099");
    var client = new Greeter.GreeterClient(channel);

    var request = new HelloRequest();
    var response = await client.SayHelloAsync(request);

    if (response is null)
    {
        Console.WriteLine("No response.");
    }
    else
    {
        Console.WriteLine(response.Message);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}