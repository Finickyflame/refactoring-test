using System;

namespace LegacyApp
{
    public class VeryImportantClientCreditService : ICreditService
    {
        public Credit GetCredit(Client client, string firstname, string surname, DateTime dateOfBirth)
        {
            return client.Name == "VeryImportantClient" ? new Credit(false, 0) : null;
        }
    }
}