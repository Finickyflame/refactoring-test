using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IUserData _userData;
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditService _creditService;

        public UserService()
        {
            // if we want to keep the dispose on the UserCreditServiceClient,
            // we could create a factory instead of using directly the instance and never disposing it. 
            this._userData = new UserData();
            this._clientRepository = new ClientRepository();
            this._creditService = new UserCreditServiceClient();
        }

        public UserService(
            IClientRepository clientRepository,
            IUserCreditService userCreditService,
            IUserData userData)
        {
            this._userData = userData;
            this._clientRepository = clientRepository;
            this._creditService = userCreditService;
        }
        
        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            if (string.IsNullOrEmpty(firname) || string.IsNullOrEmpty(surname))
            {
                return false;
            }

            if (email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            var now = DateTimeService.GetCurrentTime();
            int age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            if (age < 21)
            {
                return false;
            }

            var client = this._clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };

            if (client.Name == "VeryImportantClient")
            {
                // Skip credit chek
                user.HasCreditLimit = false;
            }
            else if (client.Name == "ImportantClient")
            {
                // Do credit check and double credit limit
                user.HasCreditLimit = true;
                    var creditLimit = this._creditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
            }
            else
            {
                // Do credit check
                user.HasCreditLimit = true;
                var creditLimit = this._creditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
                user.CreditLimit = creditLimit;
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }
            
            this._userData.AddUser(user);

            return true;
        }
    }
}