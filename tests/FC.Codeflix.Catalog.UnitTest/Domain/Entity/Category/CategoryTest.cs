using System.Xml.Linq;
using FC.Codeflix.Catalog.Domain.Entity;
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

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            var validData = new
            {
                Name = "Category Name",
                Description = "Category Description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, false);
            category.Activate();

            Assert.True(category.IsActive);
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            var validData = new
            {
                Name = "Category Name",
                Description = "Category Description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, true);
            category.Deactivate();

            Assert.False(category.IsActive);
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            var category = new DomainEntity.Category("Category Name", "Description Name");
            var newValues = new { Name = "New Name", Description = "New Description" };

            category.Update(newValues.Name, newValues.Description);

            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(newValues.Description, category.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var category = new DomainEntity.Category("Category Name", "Description Name");
            var newValues = new { Name = "New Name" };
            var currentDescription = category.Description;

            category.Update(newValues.Name);

            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(currentDescription, category.Description);
        }

        [Theory(DisplayName = nameof(UpdateWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData("  ")]
        public void UpdateWhenNameIsEmpty(string? name)
        {
            var category = new DomainEntity.Category("Category Name", "Description Name");
            Action action = () => category.Update(name!);

            var exception = Assert.Throws<Catalog.Domain.Exceptions.EntityValidationException>(action);
            Assert.Equal("Name should not be empty or null.", exception.Message);

        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("ab")]
        [InlineData("In")]
        public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            var category = new DomainEntity.Category("Category Name", "Description Name");
            Action action = () => category.Update(invalidName!);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be at least 3 characters long.", exception.Message);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsMoreThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("Alopecio Arlindo Miranda da Silva Ferdinando Guilhermino de Souza Real Munhoz de Melo Albardino da Costa Alopecio Arlindo Miranda da Silva Ferdinando Guilhermino de Souza Real Munhoz de Melo Albardino da Costa Alopecio Arlindo Miranda da Silva Ferdinando Guilhermino de Souza Real Munhoz de Melo Albardino da Costa")]
        [InlineData("Alopecio Arlindo Miranda da Silva Ferdinando Guilhermino de Souza Real Munhoz de Melo Albardino da Costa Alopecio Arlindo Miranda da Silva Ferdinando Guilhermino de Souza Real Munhoz de Melo Albardino da Costa Alopecio Arlindo D Miranda da Silva Ferdinando")]
        public void UpdateErrorWhenNameIsMoreThan255Characters(string invalidName)
        {
            var category = new DomainEntity.Category("Category Name", "Description Name");
            Action action = () => category.Update(invalidName!);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be less than 255 characters long.", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateOnlyDescription))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyDescription()
        {
            var category = new DomainEntity.Category("Category Name", "Description Name");
            var newValues = new { Description = "New Description" };
            var currentName = category.Name;

            category.Update(null, newValues.Description);

            Assert.Equal(newValues.Description, category.Description);
            Assert.Equal(currentName, category.Name);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenDescriptionIsGreaterThan10000Characters()
        {
            var category = new DomainEntity.Category("Category Name", "Description Name");
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
            Action action = () => category.Update(null, invalidDescription);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Description should be less than 10000 characters long.", exception.Message);
        }
    }
}
