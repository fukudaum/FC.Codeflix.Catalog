using FC.Codeflix.Catalog.Domain.Exceptions;
using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTest.Domain.Entity.Category
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            //Arrange
            var validData = new
            {
                Name = "Category Name",
                Description = "Category Description"
            };
            var datetimeBefore = DateTime.Now;

            //Act
            var category = new DomainEntity.Category(validData.Name, validData.Description);
            var datetimeAfter = DateTime.Now;

            //Assert
            Assert.NotNull(category);
            Assert.Equal(category.Name, validData.Name);
            Assert.Equal(category.Description, validData.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.True(category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            //Arrange
            var validData = new
            {
                Name = "Category Name",
                Description = "Category Description"
            };
            var datetimeBefore = DateTime.Now;

            //Act
            var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
            var datetimeAfter = DateTime.Now;

            //Assert
            Assert.NotNull(category);
            Assert.Equal(category.Name, validData.Name);
            Assert.Equal(category.Description, validData.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.Equal(isActive, category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            Action action = () => new DomainEntity.Category(name!, "Category Description");

            var exception = Assert.Throws<Catalog.Domain.Exceptions.EntityValidationException>(action);
            Assert.Equal("Name should not be empty or null.", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            Action action = () => new DomainEntity.Category("Category Name", null!);

            var exception = Assert.Throws<Catalog.Domain.Exceptions.EntityValidationException>(action);
            Assert.Equal("Description should not be null.", exception.Message);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("ab")]
        [InlineData("In")]
        public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            Action action = () => new DomainEntity.Category(invalidName, "In");
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be at least 3 characters long.", exception.Message);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsMoreThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("Alopecio Arlindo Miranda da Silva Ferdinando Guilhermino de Souza Real Munhoz de Melo Albardino da Costa Alopecio Arlindo Miranda da Silva Ferdinando Guilhermino de Souza Real Munhoz de Melo Albardino da Costa Alopecio Arlindo Miranda da Silva Ferdinando Guilhermino de Souza Real Munhoz de Melo Albardino da Costa")]
        [InlineData("Alopecio Arlindo Miranda da Silva Ferdinando Guilhermino de Souza Real Munhoz de Melo Albardino da Costa Alopecio Arlindo Miranda da Silva Ferdinando Guilhermino de Souza Real Munhoz de Melo Albardino da Costa Alopecio Arlindo D Miranda da Silva Ferdinando")]
        public void InstantiateErrorWhenNameIsMoreThan255Characters(string invalidName)
        {
            Action action = () => new DomainEntity.Category(invalidName, "In");
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be less than 255 characters long.", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThan10000Characters()
        {
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
            Action action = () => new DomainEntity.Category("Category name", invalidDescription);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Description should be less than 10000 characters long.", exception.Message);
        }
    }
}
