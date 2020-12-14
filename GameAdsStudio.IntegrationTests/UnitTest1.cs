using GAME_ADS_STUDIO_API;
using GAME_ADS_STUDIO_API.Models.Organization;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;

namespace GameAdsStudio.IntegrationTests
{
    public class UnitTest1
    {
        private readonly HttpClient _client;

        public UnitTest1()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient();
        }

        [Fact]
        public async Task GetOrganizationValid()
        {
            var response = await _client.GetAsync(requestUri: "api/organizations/{id}".Replace("{id}", "1"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetOrganizationInvalid()
        {
            var response = await _client.GetAsync(requestUri: "api/organizations/{id}".Replace("{id}", "-1"));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostOrganizationValid()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "organization"),
                new KeyValuePair<string, string>("PrivateEmail", "test@google.fr"),
                new KeyValuePair<string, string>("Type", "Advertiser"),
            }
            );

            var response = await _client.PostAsync(requestUri: "api/organizations", formContent);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task PostOrganizationIncompleteRequest()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "organization"),
                new KeyValuePair<string, string>("Type", "Advertiser"),
            }
            );

            var response = await _client.PostAsync(requestUri: "api/organizations", formContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostOrganizationInvalidEmail()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "organization"),
                new KeyValuePair<string, string>("PrivateEmail", "test"),
                new KeyValuePair<string, string>("Type", "Advertiser"),
            }
            );

            var response = await _client.PostAsync(requestUri: "api/organizations", formContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PatchOrganizationValid()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "organization"),
                new KeyValuePair<string, string>("PrivateEmail", "test@google.fr"),
                new KeyValuePair<string, string>("Type", "Advertiser"),
            }
            );

            var response = await _client.PatchAsync(requestUri: "api/organizations/{id}".Replace("{id}", "1"), formContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PatchOrganizationIncompleteRequest()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(null, null),
            }
            );

            var response = await _client.PatchAsync(requestUri: "api/organizations/{id}", formContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PutOrganizationValid()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "organization"),
                new KeyValuePair<string, string>("PrivateEmail", "test@google.fr"),
                new KeyValuePair<string, string>("Type", "Advertiser"),
            }
            );

            var response = await _client.PutAsync(requestUri: "api/organizations/{id}".Replace("{id}", "1"), formContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PutOrganizationIncompleteRequest()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(null, null),
            }
            );

            var response = await _client.PutAsync(requestUri: "api/organizations/{id}".Replace("{id}", "1"), formContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteOrganizationValid()
        {
            var response = await _client.DeleteAsync(requestUri: "api/organizations/{id}".Replace("{id}", "1"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddUserToOrganizationValid()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Username", "JohnnyBoy"),
                new KeyValuePair<string, string>("Firstname", "John"),
                new KeyValuePair<string, string>("Lastname", "Doe"),
            }
            );

            var response = await _client.PostAsync(requestUri: "api/organizations/{id}/users/{userId}".Replace("{id}", "1").Replace("{userId}", "1"), formContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetOrganizationUsersValid()
        {
            var response = await _client.GetAsync(requestUri: "api/organizations/{id}".Replace("{id}", "1"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteUserFromOrganizationValid()
        {
            var response = await _client.DeleteAsync(requestUri: "api/organizations/{id}/users/{userId}".Replace("{id}", "1").Replace("{userId}", "1"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }



    }
}
