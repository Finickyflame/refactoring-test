using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LegacyApp
{
    public class CreditServiceComposite : ICreditService, IEnumerable<ICreditService>
    {
        private readonly List<ICreditService> _strategies = new();

        public void Add(ICreditService service) => this._strategies.Add(service);
        
        public Credit GetCredit(Client client, string firstname, string surname, DateTime dateOfBirth)
        {
            return (from strategy in this._strategies
                    let credit = strategy.GetCredit(client, firstname, surname, dateOfBirth)
                    where credit is not null
                    select credit
                ).FirstOrDefault();
        }

        public IEnumerator<ICreditService> GetEnumerator() => this._strategies.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this._strategies).GetEnumerator();
    }
}