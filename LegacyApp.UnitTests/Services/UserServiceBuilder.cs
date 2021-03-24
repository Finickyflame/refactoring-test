using Moq;

namespace LegacyApp.UnitTests
{
    public class UserServiceBuilder
    {
        private IClientRepository _clientRepository = Mock.Of<IClientRepository>();
        private IUserCreditService _userCreditService = Mock.Of<IUserCreditService>();
        private IUserData _userData = Mock.Of<IUserData>();

        public UserServiceBuilder WithClient(Client client)
        {
            this._clientRepository = CreateService(client);
            return this;
        }

        public UserServiceBuilder WithUserCreditService(IUserCreditService userCreditService)
        {
            this._userCreditService = userCreditService;
            return this;
        }

        public UserServiceBuilder WithUserData(IUserData userData)
        {
            this._userData = userData;
            return this;
        }

        public UserService Build() => new(
            this._clientRepository,
            this._userCreditService,
            this._userData
        );

        private static IClientRepository CreateService(Client client)
        {
            var repository = new Mock<IClientRepository>();
            repository.Setup(clientRepository => clientRepository.GetById(client.Id)).Returns(client);
            return repository.Object;
        }
    }
}