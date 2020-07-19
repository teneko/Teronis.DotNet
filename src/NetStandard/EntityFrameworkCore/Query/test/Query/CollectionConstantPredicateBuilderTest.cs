using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Teronis.EntityFrameworkCore.Query;
using Xunit;

namespace Test.NetStandard.EntityFrameworkCore.Query
{
    public class CollectionConstantPredicateBuilderTest
    {
        [Fact]
        public async Task Replaces_source_and_target_mapping_parameters_by_instance_ones()
        {
            var secondChildName = "Child 2";
            var secondFatherName = "Man 2";

            var comparisonValueList = new[] { new ComparisonChild() { Fathers = new List<ComparisonMan?>() {
                new ComparisonMan() { Name = null },
                new ComparisonMan() { Name = "" },
                new ComparisonMan() { Name = secondFatherName } } } };

            var findFatherExpression = CollectionConstantPredicateBuilder<MockedChild>
                .CreateFromCollection(comparisonValueList)
                .DefinePredicatePerItem(Expression.OrElse,
                    // Select only those children who have a name and a father.
                    (child, comparisonChild) => child.MockedName != null && child.MockedFatherName != null)
                .ThenCreateFromCollection(Expression.AndAlso,
                    comparisonChild => comparisonChild.Fathers)
                .DefinePredicatePerItem(Expression.OrElse,
                    // Select the children only if the father name is equal to the comparison father name.
                    (child, comparisonFather) => comparisonFather != null && child.MockedFatherName == comparisonFather.Name)
                // Here begins the the member mapping from MockedChild to Child.
                .BuildBodyExpression<Child>(memberMapper => {
                    // We have to map each member access that are actually used above.
                    memberMapper.Map(b => b.MockedName, a => a.Name);
                    memberMapper.Map(b => b.MockedFatherName, a => a.FatherName);
                }, out var targetParameter);

            var parameterCollector = new ParameterExpressionCollectorVisitor();
            parameterCollector.Visit(findFatherExpression);

            // Ensure that all parameters used in body expression are targeting the same mapped Child parameter.
            Assert.All(parameterCollector.ParameterExpressions, parameter => {
                Assert.Equal(targetParameter, parameter);
            });

            using var context = new PersonContext();
            await context.Database.EnsureCreatedAsync();

            await context.SaveChangesAsync();

            context.Man.AddRange(
                new Man() { Name = "Man 1" },
                new Man() { Name = "Man 2" });

            await context.SaveChangesAsync();

            context.Children.AddRange(
                new Child() { Name = "Child 1" },
                new Child() { Name = secondChildName, FatherName = secondFatherName });

            await context.SaveChangesAsync();

            var findFatherLambdaExpression = Expression.Lambda<Func<Child, bool>>(
                findFatherExpression,
                targetParameter);

            var foundChildren = await context.Children.AsQueryable()
                .Where(findFatherLambdaExpression).ToListAsync();

            var foundChild = Assert.Single(foundChildren);
            Assert.Equal(secondChildName, foundChild.Name);
            Assert.Equal(secondFatherName, foundChild.FatherName);
        }

        private class ComparisonMan
        {
            public string? Name { get; set; }
        }

        private class ComparisonChild
        {
            public string? Name { get; set; }
            public List<ComparisonMan?>? Fathers { get; set; }
        }

        private class MockedChild
        {
            public string MockedName { get; set; } = null!;
            public string MockedFatherName { get; set; } = null!;
        }

        private class Person
        {
            [Key]
            public string Name { get; set; } = null!;
        }

        private class Man : Person
        {
            public List<Child> Children { get; } = null!;
        }

        private class Child : Person
        {
            public string FatherName { get; set; } = null!;
            public string MotherName { get; set; } = null!;

            public Man Father { get; set; } = null!;
        }

        private class PersonContext : DbContext
        {
            private static readonly IServiceProvider serviceProvier;

            static PersonContext()
            {
                serviceProvier = new ServiceCollection()
                    .AddLogging(configure => configure.AddConsole())
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
            }

            public DbSet<Man> Man { get; set; } = null!;
            public DbSet<Child> Children { get; set; } = null!;

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder
                    .UseInternalServiceProvider(serviceProvier)
                    .UseInMemoryDatabase(nameof(PersonContext));
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Child>(options => {
                    options.HasOne(x => x.Father)
                        .WithMany(x => x.Children)
                        .HasForeignKey(x => x.FatherName);
                });
            }
        }

        private class ParameterExpressionCollectorVisitor : ExpressionVisitor
        {
            public List<ParameterExpression> ParameterExpressions { get; }

            public ParameterExpressionCollectorVisitor() =>
                ParameterExpressions = new List<ParameterExpression>();

            protected override Expression VisitParameter(ParameterExpression node)
            {
                ParameterExpressions.Add(node);
                return node;
            }
        }
    }
}
