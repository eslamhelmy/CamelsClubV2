using CamelsClub.Data.Helpers;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Models;
using CamelsClub.Repositories;
using CamelsClub.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services.Test.User_Register
{
    [TestFixture]
    class User_LoginTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IUserRoleService> _userRoleService;
        private Mock<IUserConfirmationMessageRepository> _confirmationMessageRepository;
        private Mock<IUnitOfWork> _unitOfWork;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<IUserRepository>();
            _userRoleService = new Mock<IUserRoleService>();
            _confirmationMessageRepository = new Mock<IUserConfirmationMessageRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            //arrange
            
            _userService =
                new UserService(_unitOfWork.Object,
                                _userRepository.Object,
                                _userRoleService.Object,
                                _confirmationMessageRepository.Object);


        }
        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Login_EmptyPhone_ThrowsException(string phone)
        {
           
            Assert.That(() => _userService.Login(phone), Throws.Exception);

        }
        [Test]
        public void Login_PhoneNotExist_ThrowsException()
        {
            var phone = SecurityHelper.Encrypt("x");
            _userRepository
                .Setup(ur => ur.IsPhoneAlreadyExists(phone))
                .Returns(false);
            
            Assert.That(() => _userService.Login("x"), Throws.Exception);

        }
        [Test]
        public void Login_UserNotExist_ThrowsException()
        {
            var phone = SecurityHelper.Encrypt("x");
            _userRepository
                .Setup(ur => ur.IsPhoneAlreadyExists(phone))
                .Returns(true);

            _userRepository
                .Setup(ur => ur.GetUserByPhone(phone))
                .Returns(()=>null);

            Assert.That(() => _userService.Login("x"), Throws.Exception);

        }
        [Test]
        public void Login_GenerateVerificationCode_ReturnsTrue()
        {
            var phone = SecurityHelper.Encrypt("x");
            _userRepository
                .Setup(ur => ur.IsPhoneAlreadyExists(phone))
                .Returns(true);
            
            _userRepository
                .Setup(ur => ur.GetUserByPhone(phone))
                .Returns(() => new User { ID=1 });

            _userService.Login("x");

            _confirmationMessageRepository
                          .Verify(cm => cm.Add(It.IsAny<UserConfirmationMessage>()));

        }

      
    }
}
