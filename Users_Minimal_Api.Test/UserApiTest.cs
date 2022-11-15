using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Users_Minimal_Api.Data;
using Users_Minimal_Api.Dtos;
using Users_Minimal_Api.Services;

namespace Users_Minimal_Api.Test
{
    public class UserApiTest
    {
        // Source : https://www.twilio.com/blog/test-aspnetcore-minimal-apis
        [Fact]
        public async void TestCreateUser()
        {
            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services =>
                    {
                        services.AddScoped<IUsersService, UsersService>();
                    }));
            using var client = application.CreateClient();

            var response = await client.PostAsJsonAsync("/users", new UserDto
            {
                Username= "tonystark56",
            });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async void TestGetUsers()
        {
            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services =>
                    {
                        services.AddScoped<IUsersService, UsersService>();
                    })); 
            using var client = application.CreateClient();

            await client.PostAsJsonAsync("/users", new UserDto
            {
                Username = "joeblogs",
            });
            await client.PostAsJsonAsync("/users", new UserDto
            {
                Username = "alice_brown",
            });
            await client.PostAsJsonAsync("/users", new UserDto
            {
                Username = "brucewayne39",
            });

            var response = await client.GetAsync("/users");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void TestUpdateUser()
        {
            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services =>
                    {
                        services.AddScoped<IUsersService, UsersService>();
                    }));
            using var client = application.CreateClient();

            await client.PostAsJsonAsync("/users", new UserDto
            {
                Username = "tonystark63",
            });

            var response = await client.PutAsJsonAsync("/users/1", new UserDto
            {
                Username = "ironman63"
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void TestDeleteUser()
        {
            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services =>
                    {
                        services.AddScoped<IUsersService, UsersService>();
                    }));
            using var client = application.CreateClient();

            await client.PostAsJsonAsync("/users", new UserDto
            {
                Username = "tonystark63",
            });

            var response = await client.DeleteAsync("/users/1");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void TestCreateUserWithEmptyString()
        {
            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services =>
                    {
                        services.AddScoped<IUsersService, UsersService>();
                    }));
            using var client = application.CreateClient();

            var response = await client.PostAsJsonAsync("/users", new UserDto
            {
                Username = "",
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void TestCreateUserWithExistingUsername()
        {
            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services =>
                    {
                        services.AddScoped<IUsersService, UsersService>();
                    }));
            using var client = application.CreateClient();

            await client.PostAsJsonAsync("/users", new UserDto
            {
                Username = "joeblogs",
            });

            var response = await client.PostAsJsonAsync("/users", new UserDto
            {
                Username = "joeblogs",
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void TestUpdateUserWithEmptyString()
        {
            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services =>
                    {
                        services.AddScoped<IUsersService, UsersService>();
                    }));
            using var client = application.CreateClient();

            await client.PostAsJsonAsync("/users", new UserDto
            {
                Username = "joeblogs",
            });

            var response = await client.PutAsJsonAsync("/users/1", new UserDto
            {
                Username = ""
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void TestUpdateUserWithExistingUsername()
        {
            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services =>
                    {
                        services.AddScoped<IUsersService, UsersService>();
                    }));
            using var client = application.CreateClient();

            await client.PostAsJsonAsync("/users", new UserDto
            {
                Username = "joeblogs",
            });

            await client.PostAsJsonAsync("/users", new UserDto
            {
                Username = "tonystark63",
            });

            var response = await client.PutAsJsonAsync("/users/2", new UserDto
            {
                Username = "joeblogs"
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void TestUpdateUserWithInvalidUserId()
        {
            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services =>
                    {
                        services.AddScoped<IUsersService, UsersService>();
                    }));
            using var client = application.CreateClient();

            var response = await client.PutAsJsonAsync("/users/123456", new UserDto
            {
                Username = "james_laird"
            });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async void TestDeleteUserWithInvalidUserId()
        {
            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services =>
                    {
                        services.AddScoped<IUsersService, UsersService>();
                    }));
            using var client = application.CreateClient();

            var response = await client.DeleteAsync("/user/123456");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}