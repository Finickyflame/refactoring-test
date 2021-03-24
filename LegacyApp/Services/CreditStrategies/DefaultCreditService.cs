using System;

namespace LegacyApp
{
    public class DefaultCreditService : ICreditService
    {
        private readonly IUserCreditService _userCreditService;

        public DefaultCreditService(IUserCreditService userCreditService)
        {
            this._userCreditService = userCreditService;
        }

        public Credit GetCredit(Client client, string firstname, string surname, DateTime dateOfBirth)
        {
            // Do credit check
            int creditLimit = this._userCreditService.GetCreditLimit(firstname, surname, dateOfBirth);
            return new Credit(true, creditLimit);
        }
    }
}