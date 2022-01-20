using MonsterTradingCards.Server.DAL.Repositories.Users;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace MonsterTradingCards.Server.Test
{
    [TestFixture]
    public class TestsUserManager
    {
        [Test]
        public void RegisterUser_ValidUser()
        {
            // arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);
            var user = new Credentials { Username = "testusr", Password = "testpwd" };
            userRepo.Setup(u => u.InsertUser(It.Is<User>(u => (
            u.Username == user.Username && u.Password == user.Password && u.Token == "testusr-mtcgToken"
                && u.Gold == 20 && u.Elo == 100)))).Returns(true);

            // act
            // assert
            Assert.DoesNotThrow(() => userManager.RegisterUser(user), "InsertUser did not work!");
        }

        [Test]
        public void RegisterUser_InvalidUser()
        {
            // arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);
            var user = new Credentials { Username = "testusr", Password = "testpwd" };
            //user already in db -> InsertUser returns false
            userRepo.Setup(u => u.InsertUser(It.Is<User>(u => (
            u.Username == user.Username && u.Password == user.Password && u.Token == "testusr-mtcgToken"
                && u.Gold == 20 && u.Elo == 100)))).Returns(false);

            // act
            // assert
            Assert.Throws<DuplicateUserException>(() => userManager.RegisterUser(user), "InsertUser did work (but is not supposed to)!");
        }

        [Test]
        public void LoginUser_ValidUser()
        {
            //arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);
            var user = new Credentials { Username = "testusr", Password = "testpwd" };
            var returnedUser = new User()
            {
                Username = user.Username,
                Password = user.Password,
                Elo = 100,
                Gold = 20
            };

            userRepo.Setup(u => u.GetUserByCredentials("testusr", "testpwd")).Returns(returnedUser);

            //act
            //assert
            Assert.AreEqual(userManager.LoginUser(user), returnedUser);
        }

        [Test]
        public void LoginUser_InvalidUser()
        {
            //arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);
            var user = new Credentials { Username = "testusr", Password = "testpwd" };
            var returnedUser = new User()
            {
                Username = user.Username,
                Password = user.Password,
                Elo = 100,
                Gold = 20
            };
            //user not in DB -> returns null
            userRepo.Setup(u => u.GetUserByCredentials("testusr", "testpwd")).Returns<User>(null);

            //act
            //assert
            Assert.Throws<UserNotFoundException>(() => userManager.LoginUser(user), "Login worked (but it should not!)");
        }
    }
}