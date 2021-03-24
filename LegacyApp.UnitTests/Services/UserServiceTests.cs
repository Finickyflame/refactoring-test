using System;
using Xunit;
using Moq;

namespace LegacyApp.UnitTests
{
    public class UserServiceTests
    {
        private const string Firname = "John";
        private const string Surname = "Doe";
        private const string Email = "joedoe@example.com";
        private static readonly DateTime DateOfBirth = new(1980, 03, 1);
        private const int UserId = 1;
        private static readonly DateTime CurrentDate = new(2020, 03, 1);

        private static readonly Client DefaultClient = new()
        {
            Id = 1,
            Name = "Client",
            ClientStatus = ClientStatus.none
        };

        [Fact]
        public void AddUser_WithEmptyFirName_ShouldReturnFalse()
        {
            // Arrange
            const string firname = "";

            UserService sut = new UserServiceBuilder().Build();

            // Act
            bool actual = sut.AddUser(firname, Surname, Email, DateOfBirth, UserId);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void AddUser_WithEmptySurname_ShouldReturnFalse()
        {
            // Arrange
            const string surname = "";

            UserService sut = new UserServiceBuilder().Build();

            // Act
            bool actual = sut.AddUser(Firname, surname, Email, DateOfBirth, UserId);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void AddUser_WithInvalidEmail_ShouldReturnFalse()
        {
            // Arrange
            const string email = "joedoe@examplecom";

            UserService sut = new UserServiceBuilder().Build();

            // Act
            bool actual = sut.AddUser(Firname, Surname, email, DateOfBirth, UserId);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void AddUser_WithYoungAge_ShouldReturnFalse()
        {
            // Arrange
            DateTime dateOfBirth = new(1980, 03, 1);
            DateTimeService.GetCurrentTime = () => new DateTime(1990, 02, 01);

            UserService sut = new UserServiceBuilder()
                .Build();

            // Act
            bool actual = sut.AddUser(Firname, Surname, Email, dateOfBirth, UserId);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void AddUser_WithoutCreditLimit_ShouldAddToUserData_ShouldReturnTrue()
        {
            // Arrange
            var client = new Client
            {
                Id = 1,
                Name = "VeryImportantClient",
                ClientStatus = ClientStatus.none
            };
            DateTimeService.GetCurrentTime = () => CurrentDate;
            
            var userData = new Mock<IUserData>();
            UserService sut = new UserServiceBuilder()
                .WithClient(client)
                .WithUserData(userData.Object)
                .Build();

            // Act
            bool actual = sut.AddUser(Firname, Surname, Email, DateOfBirth, UserId);

            // Assert
            Assert.True(actual);
            userData.Verify(u => u.AddUser(It.Is<User>(user =>
                user.Client == client &&
                user.DateOfBirth == DateOfBirth &&
                user.Firstname == Firname &&
                user.Surname == Surname &&
                user.EmailAddress == Email &&
                user.HasCreditLimit == false &&
                user.CreditLimit == 0
            )), Times.Once);
        }
        
        [Fact]
        public void AddUser_WithDoubleCreditLimit_ShouldAddToUserData_ShouldReturnTrue()
        {
            // Arrange
            const int initialCreditLimit = 400;
            const int expectedCreditLimit = 800;
            var client = new Client
            {
                Id = 1,
                Name = "ImportantClient",
                ClientStatus = ClientStatus.none
            };
            var creditService = new Mock<IUserCreditService>();
            creditService.Setup(c => c.GetCreditLimit(Firname, Surname, DateOfBirth))
                .Returns(initialCreditLimit);
            
            DateTimeService.GetCurrentTime = () => CurrentDate;
            
            var userData = new Mock<IUserData>();
            UserService sut = new UserServiceBuilder()
                .WithClient(client)
                .WithUserData(userData.Object)
                .WithUserCreditService(creditService.Object)
                .Build();

            // Act
            bool actual = sut.AddUser(Firname, Surname, Email, DateOfBirth, UserId);

            // Assert
            Assert.True(actual);
            userData.Verify(u => u.AddUser(It.Is<User>(user =>
                user.Client == client &&
                user.DateOfBirth == DateOfBirth &&
                user.Firstname == Firname &&
                user.Surname == Surname &&
                user.EmailAddress == Email &&
                user.HasCreditLimit &&
                user.CreditLimit == expectedCreditLimit
            )), Times.Once);
        }
        
        [Fact]
        public void AddUser_WithNormalCreditLimit_ShouldAddToUserData_ShouldReturnTrue()
        {
            // Arrange
            const int initialCreditLimit = 500;
            var creditService = new Mock<IUserCreditService>();
            creditService.Setup(c => c.GetCreditLimit(Firname, Surname, DateOfBirth))
                .Returns(initialCreditLimit);
            
            DateTimeService.GetCurrentTime = () => CurrentDate;
            
            var userData = new Mock<IUserData>();
            UserService sut = new UserServiceBuilder()
                .WithClient(DefaultClient)
                .WithUserData(userData.Object)
                .WithUserCreditService(creditService.Object)
                .Build();

            // Act
            bool actual = sut.AddUser(Firname, Surname, Email, DateOfBirth, UserId);

            // Assert
            Assert.True(actual);
            userData.Verify(u => u.AddUser(It.Is<User>(user =>
                user.Client == DefaultClient &&
                user.DateOfBirth == DateOfBirth &&
                user.Firstname == Firname &&
                user.Surname == Surname &&
                user.EmailAddress == Email &&
                user.HasCreditLimit &&
                user.CreditLimit == initialCreditLimit
            )), Times.Once);
        }
        
        [Fact]
        public void AddUser_WithLowCreditLimit_ShouldReturnFalse()
        {
            // Arrange
            const int initialCreditLimit = 499;
            var creditService = new Mock<IUserCreditService>();
            creditService.Setup(c => c.GetCreditLimit(Firname, Surname, DateOfBirth))
                .Returns(initialCreditLimit);
            
            DateTimeService.GetCurrentTime = () => CurrentDate;
            
            UserService sut = new UserServiceBuilder()
                .WithClient(DefaultClient)
                .WithUserCreditService(creditService.Object)
                .Build();

            // Act
            bool actual = sut.AddUser(Firname, Surname, Email, DateOfBirth, UserId);

            // Assert
            Assert.False(actual);
        }
    }
}