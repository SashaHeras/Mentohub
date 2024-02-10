using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Core.Services;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using Moq;
using Mentohub.Core.AllExceptions;
using Mentohub.Domain.Data.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Mentohub.Tests
{
    public class AccountControllerTests
    {
        private readonly AccountController _controller;
        private static readonly Mock<SignInManager<CurrentUser>> signIn = new Mock<SignInManager<CurrentUser>>();
        Mock<MockSignInManager> signInManagerMock = new Mock<MockSignInManager>(signIn);
        Mock<ILogger<AccountController>> loggerMock = new Mock<ILogger<AccountController>>();
        Mock<AllException> exceptionMock = new Mock<AllException>();
        Mock<IUserService> userServiceMock = new Mock<IUserService>();
        Mock<IEmailSender> emailSenderMock = new Mock<IEmailSender>();
        Mock<UserManager<CurrentUser>> userManagerMock = new Mock<UserManager<CurrentUser>>(
            //new Mock<IUserStore<CurrentUser>>().Object,
            /*null, null, null, null, null, null, null, null*/);
        Mock<IHubContext<SignalRHub>> hubContextMock = new Mock<IHubContext<SignalRHub>>();

        //public AccountControllerTests()
        //{
        //     _controller = new AccountController(
        //        signInManagerMock,
        //        userServiceMock.Object,
        //        loggerMock.Object,
        //        exceptionMock.Object,
        //        emailSenderMock.Object,
        //        userManagerMock.Object,
        //        hubContextMock.Object
        //    );
        //}
        //[Fact]
        //public async Task Register_Returns_JsonResult_When_ModelState_Is_Valid()
        //{
            
        //    var model = new RegisterDTO()
        //    {
        //        Email="aponurko@gmail.com",
        //        NickName="Allaa",
        //        Password="Alla2023_",
        //        ConfirmPassword= "Alla2023_"
        //    };

       
        //    var result = await _controller.Register(model) as JsonResult;

          
        //    Assert.NotNull(result);
        //    Assert.Equal(200, result.StatusCode);

        //}

        //[Fact]
        //public async Task Login_Returns_JsonResult_When_ModelState_Is_Valid()
        //{
            
        //    var credentials = new LoginDTO()
        //    {
        //        Email="ponurkoalla264@ukr.net",
        //        Password="Alla2023_"
        //    };

           
        //    var result = await _controller.LoginAsync(credentials) as JsonResult;

           
        //    Assert.NotNull(result);

        //}

    }
}
