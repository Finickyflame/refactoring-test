using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IUserData _userData;
        private readonly IClientRepository _clientRepository;
        private readonly ICreditService _creditService;

        public UserService()
        {
            // if we want to keep the dispose on the UserCreditServiceClient,
            // we could create a factory instead of using directly the instance and never disposing it. 
            var userCreditService = new UserCreditServiceClient();
            this._userData = new UserData();
            this._clientRepository = new ClientRepository();
            this._creditService = new CreditServiceComposite
            {
                new VeryImportantClientCreditService(),
                new ImportantClientCreditService(userCreditService),
                new DefaultCreditService(userCreditService)
            };
        }

        public UserService(
            IClientRepository clientRepository,
            IUserCreditService userCreditService,
            IUserData userData)
        {
            this._userData = userData;
            this._clientRepository = clientRepository;
            this._creditService = new CreditServiceComposite
            {
                new VeryImportantClientCreditService(),
                new ImportantClientCreditService(userCreditService),
                new DefaultCreditService(userCreditService)
            };
        }

        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            Client client = this._clientRepository.GetById(clientId);
            
            var user = User.Create(firname, surname, email, dateOfBirth, client);
            user.SetCredit(this._creditService);
            if (user.IsValid)
            {
                this._userData.AddUser(user);
                return true;
            }
            return false;
        }
    }
}