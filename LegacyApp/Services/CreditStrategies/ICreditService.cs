using System;

namespace LegacyApp
{
    public interface ICreditService
    {
        Credit GetCredit(Client client, string firstname, string surname, DateTime dateOfBirth);
    }
}