using System;
using System.Collections.Generic;
using System.Text;

namespace ADO_AddressBook.ADO_AddressBook
{
    class ContactDataManager
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Int64 PhoneNumber { get; set; }
        public Int64 zip { get; set; }
        public string Email { get; set; }
        public string AddressBookName { get; set; }
        public string Type { get; set; }
    }
}