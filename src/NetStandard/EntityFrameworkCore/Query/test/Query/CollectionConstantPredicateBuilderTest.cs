using System;
using System.Collections;
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
        public const string FirstChildName = "Child 1";
        public const string SecondChildName = "Child 2";
        public const string ThirdChildName = "Child 3";
        public const string FirstManName = "Man 1";
        public const string SecondManName = "Man 2";

        [Fact]
        public async Task Find_single_child_by_fathers_name_and_nested_collection_constant()
        {
            var comparisonValueList = new[] {
                new ComparisonChild() { Fathers = new List<ComparisonMan?>() {
                    new ComparisonMan() { Name = SecondManName } } } };

            var findFatherExpression = CollectionConstantPredicateBuilder<MockedChild>
                .CreateFromCollection(comparisonValueList)
                .DefinePredicatePerItem(Expression.OrElse,
                    (child, comparisonChild) => true)
                .ThenCreateFromCollection(Expression.AndAlso,
                    comparisonChild => comparisonChild.Fathers)
                .DefinePredicatePerItem(Expression.OrElse,
                    (child, comparisonFather) => comparisonFather != null && child.MockedFatherName == comparisonFather.Name)
                .BuildBodyExpression<Child>(memberMapper => {
                    // We have to map each member access that are actually used above.
                    memberMapper.Map(b => b.MockedFatherName, a => a.FatherName);
                }, out var targetParameter);

            var parameterCollector = new ParameterExpressionCollectorVisitor();
            parameterCollector.Visit(findFatherExpression);

            // Ensure that all parameters used in body expression are targeting the same mapped Child parameter.
            Assert.All(parameterCollector.ParameterExpressions, parameter => {
                Assert.Equal(targetParameter, parameter);
            });

            using var context = new PersonContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.SaveChangesAsync();

            context.Man.AddRange(
                new Man() { Name = FirstManName },
                new Man() { Name = SecondManName });

            await context.SaveChangesAsync();

            context.Children.AddRange(
                new Child() { Name = FirstChildName, FatherName = null! },
                new Child() { Name = SecondChildName, FatherName = SecondManName },
                new Child() { Name = ThirdChildName, FatherName = "Any Father" });

            await context.SaveChangesAsync();

            var findFatherLambdaExpression = Expression.Lambda<Func<Child, bool>>(
                findFatherExpression,
                targetParameter);

            var foundChildren = await context.Children.AsQueryable()
                .Where(findFatherLambdaExpression).ToListAsync();

            var foundChild = Assert.Single(foundChildren);
            Assert.Equal(SecondChildName, foundChild.Name);
            Assert.Equal(SecondManName, foundChild.FatherName);
        }

        [Fact]
        public async Task Find_single_child_by_friend_in_navigation_property_and_nested_collection_constant()
        {
            var comparisonValueList = new[] {
                new ComparisonChild() {
                    Friends = new List<ComparisonMan>() {
                        new ComparisonMan() { Name = SecondManName } } } };

            var findExpression = CollectionConstantPredicateBuilder<MockedChild>
                .CreateFromCollection(comparisonValueList)
                .DefinePredicatePerItem(Expression.OrElse,
                    // Select only those children who have a name and at least one friend.
                    (child, comparisonChild) => true)
                .ThenCreateFromCollection(Expression.AndAlso,
                    comparisonChild => comparisonChild.Friends)
                .DefinePredicatePerItem(Expression.OrElse,
                    /// Select the children only if it has <see cref="SecondManName"/> as friend.
                    (child, comparisonFriend) => child.MockedFriends.Select(x => x.FriendName).Contains(comparisonFriend.Name))
                // Here begins the the member mapping from MockedChild to Child.
                .BuildBodyExpression<Child>(memberMapper => {
                    // We have to map each member access that are actually used above.
                    memberMapper.Map(b => b.MockedFriends, a => a.Friends);
                }, out var targetParameter);

            var parameterCollector = new ParameterExpressionCollectorVisitor();
            parameterCollector.Visit(findExpression);

            var collectedParameterExpressions = parameterCollector.ParameterExpressions
                .Where(x => x.Name != "x")
                .ToList();

            // Ensure that all parameters used in body expression are targeting the same mapped Child parameter.
            Assert.All(collectedParameterExpressions, parameter => {
                Assert.Equal(targetParameter, parameter);
            });

            using var context = new PersonContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.SaveChangesAsync();

            var friend1 = new Friend() { Name = FirstManName };
            var friend2 = new Friend() { Name = SecondManName };
            context.Friends.AddRange(friend1, friend2);
            await context.SaveChangesAsync();


            var child1 = new Child() { Name = FirstChildName, FatherName = null };
            var child3 = new Child() { Name = ThirdChildName };

            context.Children.AddRange(
                child1,
                new Child() { Name = SecondChildName, FatherName = SecondManName },
                child3);

            await context.SaveChangesAsync();

            context.Friendships.AddRange(new Friendship() { Child = child1, Friend = friend2 },
                new Friendship() { Child = child3, Friend = friend1 });

            await context.SaveChangesAsync();

            var findLambdaExpression = Expression.Lambda<Func<Child, bool>>(
                findExpression,
                targetParameter);

            var foundChildren = await context.Children.AsQueryable()
                .Where(findLambdaExpression).ToListAsync();

            var foundChild = Assert.Single(foundChildren);
            Assert.Equal(FirstChildName, foundChild.Name);
            Assert.True(foundChild?.Friends.Where(x => x.FriendName == SecondManName).Any());
        }

        [Theory]
        [ClassData(typeof(ComparisonChildrenWithFlagsAndNotEmptyExpectationGenerator))]
        public async Task Find_single_child_by_nested_collection_constant(
            ComparisonChild[] comparisonChildren,
            ComparisonValuesBehaviourFlags comparisonValuesFalseEvaluationFlags,
            bool notEmptyExpectation)
        {
            var findLambdaExpression = CollectionConstantPredicateBuilder<MockedChild>
                .CreateFromCollection(comparisonChildren)
                .DefinePredicatePerItem(Expression.OrElse,
                    (child, comparisonChild) => true)
                .ThenCreateFromCollection(Expression.AndAlso,
                    comparisonChild => comparisonChild.Children,
                    comparisonValuesFalseEvaluationFlags)
                .DefinePredicatePerItem(Expression.OrElse,
                    (child, comparisonFather) => true)
                // Here begins the the member mapping from MockedChild to Child.
                .BuildLambdaExpression<Child>(mapper => { });

            using var context = new PersonContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.SaveChangesAsync();

            context.Man.AddRange(
                new Man() { Name = FirstManName },
                new Man() { Name = SecondManName });

            await context.SaveChangesAsync();

            context.Children.AddRange(new Child() { Name = FirstChildName });
            await context.SaveChangesAsync();

            var foundChildren = await context.Children.AsQueryable()
                .Where(findLambdaExpression).ToListAsync();

            if (notEmptyExpectation) {
                Assert.NotEmpty(foundChildren);
            } else {
                Assert.Empty(foundChildren);
            }
        }

        public class ComparisonMan
        {
            public string? Name { get; set; }
        }

        public class ComparisonChild
        {
            public string? Name { get; set; }
            public List<ComparisonMan?>? Fathers { get; set; }
            public List<ComparisonMan>? Friends { get; set; }
            public ComparisonChild[]? Children { get; set; }
        }

        private interface MockedChild
        {
            string MockedName { get; set; }
            string MockedFatherName { get; set; }
            List<Friendship> MockedFriends { get; set; }
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

        private class Friend : Person
        { }

        private class Child : Person
        {
            public string? FatherName { get; set; }
            public string MotherName { get; set; } = null!;

            public Man Father { get; set; } = null!;
            public List<Friendship> Friends { get; set; } = null!;
        }

        private class Friendship
        {
            public string ChildName { get; set; } = null!;
            public Child Child { get; set; } = null!;
            public string FriendName { get; set; } = null!;
            public Friend Friend { get; set; } = null!;
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
            public DbSet<Friend> Friends { get; set; } = null!;
            public DbSet<Child> Children { get; set; } = null!;
            public DbSet<Friendship> Friendships { get; set; } = null!;

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

                modelBuilder.Entity<Friendship>(options => {
                    options.HasKey(x => new { x.ChildName, x.FriendName });

                    options.HasOne(x => x.Child)
                        .WithMany(x => x.Friends);

                    options.HasOne(x => x.Friend)
                        .WithMany();
                });
            }
        }

        public class ComparisonChildrenWithFlagsAndNotEmptyExpectationGenerator : IEnumerable<object?[]>
        {
            public IEnumerator<object?[]> GetEnumerator()
            {
                static object?[] array(params object?[] items) =>
                    items;

                var nullChildren = new ComparisonChild[] { new ComparisonChild() { Children = null } };
                var emptyChildren = new ComparisonChild[] { new ComparisonChild() { Children = new ComparisonChild[] { } } };

                yield return array(nullChildren, ComparisonValuesBehaviourFlags.NullOrEmptyLeadsToSkip, true);
                yield return array(emptyChildren, ComparisonValuesBehaviourFlags.NullOrEmptyLeadsToSkip, true);

                yield return array(nullChildren, ComparisonValuesBehaviourFlags.NullLeadsToFalse, false);
                yield return array(emptyChildren, ComparisonValuesBehaviourFlags.NullLeadsToFalse, true);

                yield return array(nullChildren, ComparisonValuesBehaviourFlags.EmptyLeadsToFalse, true);
                yield return array(emptyChildren, ComparisonValuesBehaviourFlags.EmptyLeadsToFalse, false);
            }

            IEnumerator IEnumerable.GetEnumerator() =>
                GetEnumerator();
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
