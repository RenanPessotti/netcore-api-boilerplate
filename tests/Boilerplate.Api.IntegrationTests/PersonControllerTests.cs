using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Boilerplate.Api.IntegrationTests.Helpers;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.DTOs.Person;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Boilerplate.Api.IntegrationTests
{
    public class PersonControllerTests : IntegrationTest
    {
        #region GET

        [Fact]
        public async Task Get_AllPersons_ReturnsOk()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var response = await client.GetAsync("/api/Person");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = JsonConvert.DeserializeObject<PaginatedList<GetPersonDto>>(await response.Content.ReadAsStringAsync());
            json.Should().NotBeNull();
            json.Result.Should().OnlyHaveUniqueItems();
            json.Result.Should().HaveCount(3);
            json.CurrentPage.Should().Be(1);
            json.TotalItems.Should().Be(3);
            json.TotalPages.Should().Be(1);
        }

        [Fact]
        public async Task Get_AllPersonsWithPaginationFilter_ReturnsOk()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var response = await client.GetAsync("/api/Person?PageSize=1&CurrentPage=1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = JsonConvert.DeserializeObject<PaginatedList<GetPersonDto>>(await response.Content.ReadAsStringAsync());
            json.Should().NotBeNull();
            json.Result.Should().OnlyHaveUniqueItems();
            json.Result.Should().HaveCount(1);
            json.CurrentPage.Should().Be(1);
            json.TotalItems.Should().Be(3);
            json.TotalPages.Should().Be(3);
        }

        [Fact]
        public async Task Get_AllPersonsWithNegativePageSize_ReturnsOk()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var response = await client.GetAsync("/api/Person?PageSize=-1&CurrentPage=1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = JsonConvert.DeserializeObject<PaginatedList<GetPersonDto>>(await response.Content.ReadAsStringAsync());
            json.Should().NotBeNull();
            json.Result.Should().OnlyHaveUniqueItems();
            json.Result.Should().HaveCount(3);
            json.CurrentPage.Should().Be(1);
            json.TotalItems.Should().Be(3);
            json.TotalPages.Should().Be(1);
        }

        [Fact]
        public async Task Get_AllPersonsWithNegativeCurrentPage_ReturnsOk()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var response = await client.GetAsync("/api/Person?PageSize=15&CurrentPage=-1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = JsonConvert.DeserializeObject<PaginatedList<GetPersonDto>>(await response.Content.ReadAsStringAsync());
            json.Should().NotBeNull();
            json.Result.Should().OnlyHaveUniqueItems();
            json.Result.Should().HaveCount(3);
            json.CurrentPage.Should().Be(1);
            json.TotalItems.Should().Be(3);
            json.TotalPages.Should().Be(1);
        }

        [Fact]
        public async Task Get_ExistingPersonsWithFilter_ReturnsOk()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var response = await client.GetAsync("/api/Person?Name=Corban");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = JsonConvert.DeserializeObject<PaginatedList<GetPersonDto>>(await response.Content.ReadAsStringAsync());
            json.Should().NotBeNull();
            json.Result.Should().OnlyHaveUniqueItems();
            json.Result.Should().HaveCount(1);
            json.CurrentPage.Should().Be(1);
            json.TotalItems.Should().Be(1);
            json.TotalPages.Should().Be(1);
        }


        [Fact]
        public async Task Get_NonExistingPersonsWithFilter_ReturnsOk()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var response = await client.GetAsync("/api/Person?Name=asdfsdlkafhsduifhasduifhsdui");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = JsonConvert.DeserializeObject<PaginatedList<GetPersonDto>>(await response.Content.ReadAsStringAsync());
            json.Should().NotBeNull();
            json.Result.Should().BeEmpty();
            json.CurrentPage.Should().Be(1);
            json.TotalItems.Should().Be(0);
            json.TotalPages.Should().Be(0);
        }

        [Fact]
        public async Task GetById_ExistingPerson_ReturnsOk()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var response = await client.GetAsync("/api/Person/824a7a65-b769-4b70-bccb-91f880b6ddf1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = JsonConvert.DeserializeObject<GetPersonDto>(await response.Content.ReadAsStringAsync());
            json.Should().NotBeNull();
            json.Id.Should().NotBeEmpty();
            json.Name.Should().NotBeNull();
            json.Sex.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_ExistingPerson_ReturnsNotFound()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var response = await client.GetAsync($"/api/Person/{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST

        [Fact]
        public async Task Post_ValidPerson_ReturnsCreated()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var newPerson = JsonSerializer.Serialize(new
            {
                Name = "Name Person success",
                Sex = 1
            });
            var response = await client.PostAsync("/api/Person", new StringContent(newPerson, Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var json = JsonConvert.DeserializeObject<GetPersonDto>(await response.Content.ReadAsStringAsync());
            json.Should().NotBeNull();
            json.Id.Should().NotBeEmpty();
            json.Name.Should().NotBeNull();
            json.Sex.Should().NotBeNull();
        }

        [Fact]
        public async Task Post_NamelessPerson_ReturnsBadRequest()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var newPerson = JsonSerializer.Serialize(new
            {
                Job = "Job Person badrequest"
            });
            var response = await client.PostAsync("/api/Person", new StringContent(newPerson, Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_JoblessPerson_ReturnsBadRequest()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var newPerson = JsonSerializer.Serialize(new
            {
                Name = "Name Person badrequest"
            });
            var response = await client.PostAsync("/api/Person", new StringContent(newPerson, Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_EmptyPerson_ReturnsBadRequest()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var newPerson = JsonSerializer.Serialize(new
            {
            });
            var response = await client.PostAsync("/api/Person", new StringContent(newPerson, Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion


        #region PUT

        [Fact]
        public async Task Put_ValidPerson_ReturnsNoContent()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var newPerson = JsonSerializer.Serialize(new
            {
                Name = "Name Person success",
                Sex = 1
            });
            var response = await client.PutAsync("/api/Person/824a7a65-b769-4b70-bccb-91f880b6ddf1", new StringContent(newPerson, Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }


        [Fact]
        public async Task Put_NamelessPerson_ReturnsBadRequest()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var newPerson = JsonSerializer.Serialize(new
            {
                Sex = 1
            });
            var response = await client.PutAsync("/api/Person/824a7a65-b769-4b70-bccb-91f880b6ddf1", new StringContent(newPerson, Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_Jobless_ReturnsBadRequest()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var newPerson = JsonSerializer.Serialize(new
            {
                Name = "Name Person badrequest"
            });
            var response = await client.PutAsync("/api/Person/824a7a65-b769-4b70-bccb-91f880b6ddf1", new StringContent(newPerson, Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_EmptyPerson_ReturnsBadRequest()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var newPerson = JsonSerializer.Serialize(new
            {
            });
            var response = await client.PutAsync("/api/Person/824a7a65-b769-4b70-bccb-91f880b6ddf1", new StringContent(newPerson, Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_InvalidPersonId_ReturnsNotFound()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            // Act
            var newPerson = JsonSerializer.Serialize(new
            {
                Name = "Name Person not found",
                Sex = 1
            });
            var response = await client.PutAsync("/api/Person/1d2c03e0-cc51-4f22-b1be-cdee04b1f896", new StringContent(newPerson, Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE

        [Fact]
        public async Task Delete_ValidPerson_ReturnsNoContent()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            var response = await client.DeleteAsync("/api/Person/824a7a65-b769-4b70-bccb-91f880b6ddf1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_InvalidPerson_ReturnsNotFound()
        {
            // Arrange
            var client = Factory.RebuildDb().CreateClient();

            var response = await client.DeleteAsync("/api/Person/88d59ace-2c1a-49b0-8190-49b8304f8120");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}
