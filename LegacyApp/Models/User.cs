using System;

namespace LegacyApp
{
    public class User
    {
        private readonly EmailAddress _emailAddress;
        private readonly BirthDate _dateOfBirth;
        private Credit _credit;

        private User(string firstname, string surname, EmailAddress emailAddress, BirthDate dateOfBirth, Client client)
        {
            this._emailAddress = emailAddress;
            this._dateOfBirth = dateOfBirth;
            this.Firstname = firstname;
            this.Surname = surname;
            this.Client = client;
        }

        public string Firstname { get; }
        public string Surname { get; }
        public DateTime DateOfBirth => this._dateOfBirth.Value;
        public string EmailAddress => this._emailAddress.Value;
        public bool HasCreditLimit => this._credit?.HasLimit ?? false;
        public int CreditLimit => this._credit?.Limit ?? 0;

        public Client Client { get; }

        public bool CanRequestCredit => !string.IsNullOrEmpty(this.Firstname) &&
                                        !string.IsNullOrEmpty(this.Surname) &&
                                        this._emailAddress.HasValidFormat() &&
                                        this._dateOfBirth.IsOlderOrEqualTo(21) &&
                                        this.Client is not null;

        public bool IsValid => this.CanRequestCredit && (this._credit?.IsHigherOrEqualTo(500) ?? false);


        public void SetCredit(ICreditService creditService)
        {
            if (this.CanRequestCredit)
            {
                this._credit = creditService.GetCredit(this.Client, this.Firstname, this.Surname, this.DateOfBirth);
            }
        }


        public static User Create(
            string firstname,
            string surname,
            string emailAddress,
            DateTime dateOfBirth,
            Client client)
            => new(firstname, surname, new EmailAddress(emailAddress), new BirthDate(dateOfBirth), client);
    }
}