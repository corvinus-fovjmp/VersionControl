﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Controllers;

namespace UnitTestExample.Test
{
   public  class AccountControllerTestFixture
    {
        [Test,
         TestCase("abcd1234", false),
         TestCase("irf@uni-corvinus", false),
         TestCase("irf.uni-corvinus.hu", false),
         TestCase("irf@uni-corvinus.hu", true)
        ]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            //Arrange
            var accountController = new AccountController();
            //Act
            var actualResult = accountController.ValidateEmail(email);
            //Assert
            Assert.AreEqual(expectedResult, actualResult);


        
        }
        [Test,
            TestCase("Asdfgg",false),
            TestCase("MASDMASMDAS", false),
            TestCase("jasdjasjdaj2", false),
            TestCase("asd1", false),
            TestCase("Asdfghjj23", true)

            ]
        public void TestValidatePassword(string password, bool expectedResult)
        {
            //Arrange
            var accountController = new AccountController();
            //Act
            var actualResult = accountController.ValidatePassword(password);
            //Assert
            Assert.AreEqual(expectedResult, actualResult);


        }
        [Test,
            TestCase("irf@uni-corvinus.hu", "Asdfghj1"),
            TestCase("irf@uni-corvinus.hu", "ASDFhvkbkb1234")
            ]
        public void TestRegisterHappyPath(string email, string password)
        {
            //Arrange 
            var accountController = new AccountController();
            //Act
            var actualResult = accountController.Register(email, password);
            //Assert
            Assert.AreEqual(email, actualResult.Email);
            Assert.AreEqual(password, actualResult.Password);
            Assert.AreNotEqual(Guid.Empty, actualResult.ID);

        }

        
    }
}
