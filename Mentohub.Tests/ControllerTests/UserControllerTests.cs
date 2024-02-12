﻿using Mentohub.Controllers;
using Mentohub.Core.AllExceptions;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MassTransit.ValidationResultExtensions;

namespace Mentohub.Tests.ControllerTests
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        Mock<ILogger<UserController>> _loggerMoq = new Mock<ILogger<UserController>>();
        Mock<AllException> _exceptionMoq = new Mock<AllException>();
        Mock<IUserService> _userServiceMoq = new Mock<IUserService>();
        Mock<ICRUD_UserRepository> _cRUDMoq = new Mock<ICRUD_UserRepository>();
        Mock<ICourseService> _courseServiceMoq = new Mock<ICourseService>();

        public UserControllerTests()
        {
            _controller = new UserController(
                _loggerMoq.Object,
                _userServiceMoq.Object,
                _exceptionMoq.Object,
                _cRUDMoq.Object,
                _courseServiceMoq.Object
            );
        }

        [Fact]
        public async Task Delete_User_Returns_JsonResult()
        {
            var userId = "CE1RBVsMEAZLXkRVBxRSF1BMSA8AWxBJA10UDVQBUhBaQQYO";
            var result = await _controller.DeleteUser(userId) as JsonResult;
            var statusCode = result.StatusCode;
            Assert.NotNull(result);
            Assert.Equal(404, statusCode);
        }

        [Fact]
        public async Task Get_UserProfile_Returns_JsonResult()
        {
            var userId = "CE1RBVsMEAZLXkRVBxRSF1BMSA8AWxBJA10UDVQBUhBaQQYO";
            var result = await _controller.GetUserProfile(userId) as JsonResult;
            var statusCode = result.StatusCode;
            Assert.NotNull(result);
            Assert.Equal(404, statusCode);
        }

        [Fact]
        public async Task UpDate_UserProfile_Returns_JsonResult()
        {
            var userId404 = "CE1RBVsMEAZLXkRVBxRSF1BMSA8AWxBJA10UDVQBUhBaQQYO";
            var userId200 = "508c3e4e-801c-480f-b653-6f92019314e1";

            var userDTO = new UserDTO()
            {
                Id = userId200,
                Email = "chipolino@maildrop.cc4",
                FirstName = "DDDDDD",
                LastName = "NNNNN",
                AboutMe = "Hello",
                Name = "User23456",
                DateOfBirth = new DateTime(),
                UserRoles = null,
                EncryptedID = "WUAHBQgIRVBLDhQEUxRSRFxESFcKCBdJUllDUVZaA09YEV0C",
                PhoneNumber = "+380501111111",
            };

            var result = await _controller.UpdateUser(userDTO) as JsonResult;

            var statusCode = result.StatusCode;
            Assert.NotNull(result);
            Assert.Equal(200, statusCode);
        }
        [Fact]
        public async Task AddToUserRoleAuthor_Returns_JsonResult()
        {
            var userId = "XxRQAAFaTlJLWUQFBhRSQlxDSA8NAEFJAgEUUVEMUBdbEVdU";
            var result = await _controller.AddToUserRoleAuthor(userId) as JsonResult;
            var statusCode = result.StatusCode;
            Assert.NotNull(result);
            Assert.Equal(200, statusCode);
        }
        [Fact]
        public async Task AddUserRoles_Returns_JsonResult()
        {
            var userId = "WUAHBQgIRVBLDhQEUxRSRFxESFcKCBdJUllDUVZaA09YEV0C";
            var roleAdminId = "eeb6f53c-a9e5-4920-8667-ea02a3a6e38a";
            var result = await _controller.AddUserRoles(userId, roleAdminId) as JsonResult;
            var statusCode = result.StatusCode;
            Assert.NotNull(result);
            Assert.Equal(200, statusCode);
        }
    }
}