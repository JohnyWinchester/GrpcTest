using Grpc.Core;

namespace GrpcService1.Services
{
    public class ValuesService: Values.ValuesBase
    {
        private readonly Dictionary<int, string> _Values = Enumerable.Range(1, 10)
            .Select(i => (Id: i, Value: $"Value-{i}"))
            .ToDictionary(v => v.Id, v => v.Value);

        public override Task<SendCount> Count(RequestCount request, ServerCallContext context)
        {
            return Task.FromResult(new SendCount { Count = _Values.Count });
        }

        public override async Task GetValues(
            RequestValues request, 
            IServerStreamWriter<SendValues> responseStream, 
            ServerCallContext context)
        {
            foreach(var el in _Values)
            {
                await responseStream.WriteAsync( new SendValues { Value = el.Value });
            }
        }
    }
}
