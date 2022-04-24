using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADO_AddressBook.ADO_AddressBook
{
    class AddressBookRespitory
    {
        //Give path for Database Connection
        public static string connection = @"Server=.;Database=Address_Book_Service_DB;Trusted_Connection=True;";
        //Represents a connection to Sql Server Database
        SqlConnection sqlConnection = new SqlConnection(connection);
    }
}