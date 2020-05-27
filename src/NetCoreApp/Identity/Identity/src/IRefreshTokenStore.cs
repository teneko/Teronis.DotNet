using System.Threading;
using System.Threading.Tasks;
using Teronis.Identity.Entities;

namespace Teronis.Identity.BearerSignInManaging
{
    public interface IRefreshTokenStore<RefreshTokenType>
        where RefreshTokenType : RefreshTokenEntity
    {
        ValueTask<RefreshTokenType> FindAsync(object[] keyValues, CancellationToken cancellationToken = default);
    }
}
