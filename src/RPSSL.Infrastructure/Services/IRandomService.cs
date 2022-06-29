using System.Threading;
using System.Threading.Tasks;

namespace RPSSL.Infrastructure.Services
{
    public interface IRandomService
    {
        Task<GetRandomResponse> GetRandomAsync(CancellationToken cancellationToken = default);
    }
}
