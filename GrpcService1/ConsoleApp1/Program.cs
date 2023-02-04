using Grpc.Net.Client;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            Console.WriteLine();
            GrpcTest();
            Console.ReadKey();
        }

        static async void GrpcTest()
        {
            AppContext.SetSwitch(
                    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress("http://localhost:5241",
                new GrpcChannelOptions { HttpHandler = httpHandler });
            // создаем клиент
            var client = new Values.ValuesClient(channel);

            var response = client.GetValues(new RequestValues());

            var values = response.ResponseStream;

            while (await values.MoveNext(new CancellationToken()))
            {
                SendValues sendValues = values.Current;
                Console.WriteLine(sendValues.Value);
            }

            var responseCount = client.Count(new RequestCount());
            Console.WriteLine(responseCount.Count);
        }
    }
}