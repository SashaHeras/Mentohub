﻿using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Tests
{
    public class MockSignInManager
    {
        private readonly Mock<SignInManager<CurrentUser>> _mock;
        
        public MockSignInManager(Mock<SignInManager<CurrentUser>> mock)
        {
            _mock = mock;
           
        }
        public Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return Task.FromResult(SignInResult.Success);
        }

        public Task SignInAsync(CurrentUser user, bool isPersistent, string authenticationMethod = null)
        {
            return Task.CompletedTask;
        }

        public Task SignOutAsync()
        {
            return Task.CompletedTask;
        }

        public Mock<SignInManager<CurrentUser>> Setup()
        {
            return _mock;
        }
    }

}
