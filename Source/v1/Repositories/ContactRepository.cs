using System;
using System.Collections.Generic;
using System.Text;
using TingTango.Source.v1.Models;

namespace TingTango.Source.v1.Repositories
{
    public class ContactRepository
    {
        static readonly ContactRepository singleton;

        static ContactRepository()
        {
            singleton = new ContactRepository();
        }

        public static ContactRepository Instance() => singleton;

        private readonly Dictionary<string, Contact> contacts;

        ContactRepository()
        {
            contacts = new Dictionary<string, Contact>
            {
                { "0001", new Contact { Name = "John Doe" } },
                { "0002", new Contact { Name = "Shaun Michael" } },
            };
        }

        public IReadOnlyCollection< Contact> GetAll() => this.contacts.Values;

        public Contact Get(string id)
        {
            this.contacts.TryGetValue(id, out var contact);
            return contact;
        }

        public void Create(Contact contact)
        {
            if (contacts.ContainsKey(contact.Id))
                throw new ApplicationException("Duplicate entry!");
            contacts.Add(contact.Id, contact);
        }

        public void Update(string contactId, Contact contact)
        {
            if (!contacts.TryGetValue(contactId, out var existingContact))
                throw new ApplicationException("Entry unavailable!");

            contacts[contact.Id] = contact;
        }
    }
}
