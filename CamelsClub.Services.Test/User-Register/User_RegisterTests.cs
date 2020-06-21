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
    class User_RegisterTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IUserRoleService> _userRoleService;
        private Mock<IUserConfirmationMessageRepository> _confirmationMessageRepository;
        private Mock<IUnitOfWork> _unitOfWork;
        private CreateUserViewModel _addedUser;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<IUserRepository>();
            _userRoleService = new Mock<IUserRoleService>();
            _confirmationMessageRepository = new Mock<IUserConfirmationMessageRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            //arrange
            _addedUser = new CreateUserViewModel()
            {
                Email = "x",
                MobileNumber = "01092059091",
                Name = "eslam",
                NID = "NID"
            };
            _userService =
                new UserService(_unitOfWork.Object,
                                _userRepository.Object,
                                _userRoleService.Object,
                                _confirmationMessageRepository.Object);


        }
        [Test]
        public void Register_EmailAlreadyExists_ThrowsException()
        {
            _userRepository
                .Setup(ur => ur.IsEmailAlreadyExists(_addedUser.Email))
                .Returns(true);

            Assert.That(() => _userService.Register(_addedUser), Throws.Exception);

        }
        [Test]
        public void Register_PhoneAlreadyExists_ThrowsException()
        {
            _userRepository
                .Setup(ur => ur.IsPhoneAlreadyExists(_addedUser.MobileNumber))
                .Returns(true);

            Assert.That(() => _userService.Register(_addedUser), Throws.Exception);

        }
        [Test]
        public void Register_EncryptsMobileAndEmail_ReturnsTrue()
        {
            //arrange
            _userRepository
                .Setup(ur => ur.Add(It.IsAny<User>()))
                .Returns(() => new User { ID = 1 });
            _confirmationMessageRepository
                .Setup(cm => cm.IsValidCode(It.IsAny<string>()))
                .Returns(true);
            //act
            _userService.Register(_addedUser);
            var decryptedEmail = SecurityHelper.Decrypt(_addedUser.Email);
            var decryptedPhone = SecurityHelper.Decrypt(_addedUser.MobileNumber);
            //assert
            Assert.That(decryptedEmail, Is.EqualTo("x"));
            Assert.That(decryptedPhone, Is.EqualTo("01092059091"));

        }

        [Test]
        public void Register_AddUserRole_ReturnsTrue()
        {
            //arrange
            _userRepository
                .Setup(ur => ur.Add(It.IsAny<User>()))
                .Returns(() => new User { ID = 1 });
            _confirmationMessageRepository
                .Setup(cm => cm.IsValidCode(It.IsAny<string>()))
                .Returns(true);
            //act
            _userService.Register(_addedUser);

            //assert
            _userRoleService.Verify(ur => ur.InsertUserRole(1, ViewModels.Enums.Roles.User));
        }

        [Test]
        public void Register_GenerateConfirmationCode_ReturnsTrue()
        {
            //arrange
            _userRepository
                .Setup(ur => ur.Add(It.IsAny<User>()))
                .Returns(() => new User { ID = 1 });
            _confirmationMessageRepository
                .Setup(cm => cm.IsValidCode(It.IsAny<string>()))
                .Returns(true);
            //act
            _userService.Register(_addedUser);

            //assert
            _confirmationMessageRepository
                .Verify(cm => cm.Add(It.IsAny<UserConfirmationMessage>()));
        }
    }
}
