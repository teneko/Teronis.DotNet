using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Teronis.Linq.Expressions.EntityExtension
{
    public class TypedSourceTargetMemberMapperTests
    {
        [Fact]
        public async Task Parcel_entity_can_be_queried_with_unrelated_but_mapped_interface_members()
        {
            using var dbContext = new ManagmentContext();
            dbContext.Parcels.Add(new Parcel() { ProudParcelId = 2 });
            dbContext.Parcels.Add(new Parcel() { ProudParcelId = 3 });
            await dbContext.SaveChangesAsync();

            var parcelRepository = new ParcelRepository(dbContext.Parcels);
            var service = new ManagmentService(parcelRepository);

            static void configureMappings(IParameterReplacableExpressionMapper<IParcel, Parcel> mapper) =>
                mapper.MapBodyAndParams(x => x.ParcelId, x => x.ProudParcelId);

            var hasParcel = service.HasParcel(2, configureMappings);
            Assert.True(hasParcel);

            var hasStillParcel = service.HasParcel(4, configureMappings);
            Assert.False(hasStillParcel);
        }

        public class Parcel
        {
            [Key]
            public int ProudParcelId { get; set; }
        }

        private class ManagmentContext : DbContext
        {
            private static readonly IServiceProvider serviceProvier;

            static ManagmentContext()
            {
                serviceProvier = new ServiceCollection()
                    .AddLogging(configure => configure.AddConsole())
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
            }

            public DbSet<Parcel> Parcels { get; set; } = null!;

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder
                    .UseInternalServiceProvider(serviceProvier)
                    .UseInMemoryDatabase(nameof(ManagmentContext));
            }
        }

        public class ParcelRepository : IRepository<Parcel>
        {
            public ParcelRepository(IQueryable<Parcel> query) =>
                Query = query;

            public IQueryable<Parcel> Query;

            public virtual IQueryable<Parcel> Get(Expression<Func<Parcel, bool>> query) =>
                Query.Where(query);
        }

        public class ManagmentService
        {
            public ManagmentService(IRepository<Parcel> parcelRepository) =>
                ParcelRepository = parcelRepository;

            private IRepository<Parcel> ParcelRepository { get; set; }

            // Pass mappings directly or replace it with mapping provider where 
            // you can define IParcel -> TParcel mappings for needed entities.
            public bool HasParcel(int existingParcelId, Action<IParameterReplacableExpressionMapper<IParcel, Parcel>> configureMappings)
            {
                var hasAnyParcel = ParcelRepository.Get((e, p) => p.ParcelId == existingParcelId, configureMappings).Any();
                return hasAnyParcel;
            }
        }
    }

    public interface IRepository<T>
    {
        IQueryable<T> Get(Expression<Func<T, bool>> query);
    }

    /// <summary>
    /// We use it to describe entities we want to have extended
    /// by the properties of this interface, so we do not have
    /// to implement this interface in respective entities.
    /// </summary>
    public interface IParcel
    {
        int ParcelId { get; }
    }

    public static class IRepositoryGenericExtensions
    {
        public static IQueryable<T> Get<T>(this IRepository<T> repository, Expression<Func<T, IParcel, bool>> query, Action<IParameterReplacableExpressionMapper<IParcel, T>> configureMappings)
        {
            var replacedBody = SourceExpression.ReplaceParameters(query.Body, query.Parameters[1], query.Parameters[0], configureMappings);
            var newParameterizedQuery = Expression.Lambda<Func<T, bool>>(replacedBody, query.Parameters[0]);
            return repository.Get(newParameterizedQuery);
        }
    }
}
