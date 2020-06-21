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
    class User_ConfirmationMessageTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IUserRoleService> _userRoleService;
        private Mock<IUserConfirmationMessageRepository> _confirmationMessageRepository;
        private Mock<IUnitOfWork> _unitOfWork;
        private UserConfirmationMessageService _userConfirmationService;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<IUserRepository>();
            _userRoleService = new Mock<IUserRoleService>();
            _confirmationMessageRepository = new Mock<IUserConfirmationMessageRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            //arrange
            
            _userConfirmationService =
                new UserConfirmationMessageService(_unitOfWork.Object,
                                          _confirmationMessageRepository.Object);


        }
        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void UpdateUserConfirmationMessage_CodeIsEmpty_ThrowsNullArgumentException(string code)
        {
            Assert.That(() => 
            _userConfirmationService.UpdateUserConfirmationMessage(code)
            , Throws.ArgumentNullException);

        }

        [Test]
        public void UpdateUserConfirmationMessage_InValidCode_ThrowsException()
        {
            _confirmationMessageRepository
                .Setup(x => x.IsValidCode("x"))
                .Returns(false);
            Assert.That(() =>
            _userConfirmationService.UpdateUserConfirmationMessage("x")
            , Throws.Exception);

        }
        [Test]
        public void UpdateUserConfirmationMessage_GetUserByCode_ReturnsTrue()
        {
            _confirmationMessageRepository
                .Setup(x => x.IsValidCode("x"))
                .Returns(true);
            _confirmationMessageRepository
                .Setup(x => x.GetUserConfirmMessage("x"))
                .Returns(new UserConfirmationMessage { });

            _userConfirmationService.UpdateUserConfirmationMessage("x");

            _confirmationMessageRepository
                .Verify(x => x.GetUserConfirmMessage("x"));
            
            
        }
    }
}
