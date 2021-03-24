using System;

namespace LegacyApp
{
    public class ImportantClientCreditService : ICreditService
    {
        private readonly IUserCreditService _userCreditService;

        public ImportantClientCreditService(IUserCreditService userCreditService)
        {
            this._userCreditService = userCreditService;
        }

        public Credit GetCredit(Client client, string firstname, string surname, DateTime dateOfBirth)
        {
            if (client.Name == "ImportantClient")
            {
                // Do credit check and double credit limit
                int creditLimit = this._userCreditService.GetCreditLimit(firstname, surname, dateOfBirth) * 2;
                return new Credit(true, creditLimit);
            }

            return null;
        }
    }
}