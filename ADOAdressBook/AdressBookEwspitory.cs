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
        private object addressBook;

        public int InsertIntoTable(ContactDataManager addressBook)
        {
            int result = 0;
            try
            {
                using (sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("spInsertintoTable", this.sqlConnection);
                    //setting command type as stored procedure
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@FirstName", addressBook.FirstName);
                    sqlCommand.Parameters.AddWithValue("@LastName", addressBook.LastName);
                    sqlCommand.Parameters.AddWithValue("@Address", addressBook.Address);
                    sqlCommand.Parameters.AddWithValue("@City", addressBook.City);
                    sqlCommand.Parameters.AddWithValue("@State", addressBook.State);
                    sqlCommand.Parameters.AddWithValue("@zip", addressBook.zip);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", addressBook.PhoneNumber);
                    sqlCommand.Parameters.AddWithValue("@Email", addressBook.Email);
                    sqlCommand.Parameters.AddWithValue("@addressBookName", addressBook.AddressBookName);
                    sqlCommand.Parameters.AddWithValue("@addressBookType", addressBook.Type);
                    sqlConnection.Open();
                    //Return the number of rows updated
                    result = sqlCommand.ExecuteNonQuery();
                    if (result != 0)
                    {
                        Console.WriteLine("Updated");
                    }
                    else
                    {
                        Console.WriteLine("Not Updated");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            return result;
        }
        //UseCase 3: Modify Existing Contact using their name
        public int UpdateQueryBasedonName()
        {
            //Open Connection
            sqlConnection.Open();
            string query = "Update Address_Book_Table set Email = 'RaniMalvi@gmail.com' where FirstName = 'Rani'";
            //Pass query to TSql
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            int result = sqlCommand.ExecuteNonQuery();
            if (result != 0)
            {
                Console.WriteLine("Updated!");
            }
            else
            {
                Console.WriteLine("Not Updated!");
            }

            //Close Connection
            sqlConnection.Close();
            return result;
        }
        //UseCase 4-Delete Contact using their name
        public int DeletePersonBasedonName()
        {
            //Open Connection
            sqlConnection.Open();
            string query = "delete from Address_Book_Table where FirstName = 'Anita' and LastName = 'Vargheese'";
            //Pass query to TSql
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            int result = sqlCommand.ExecuteNonQuery();
            if (result != 0)
            {
                Console.WriteLine("Updated!");
            }
            else
            {
                Console.WriteLine("Not Updated!");
            }

            //Close Connection
            sqlConnection.Close();
            return result;
        }
        //UseCase 5: Ability to Retrieve Person belonging to a City or State from the Address Book
        public string PrintDataBasedOnCity(string city, string State)
        {
            string nameList = "";
            //query to be executed
            string query = @"select * from Address_Book_Table where City =" + "'" + city + "' or State=" + "'" + State + "'";
            SqlCommand sqlCommand = new SqlCommand(query, this.sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    DisplayEmployeeDetails(sqlDataReader);
                    nameList += sqlDataReader["FirstName"].ToString() + " ";
                }
            }
            return nameList;
        }
        //UC 6: Ability to Retrieve Count of Person belonging to a City or State
        public string PrintCountDataBasedOnCity()
        {
            string nameList = "";
            //query to be executed
            string query = @"select Count(*),state,City from Address_Book_Table Group by state,City";
            SqlCommand sqlCommand = new SqlCommand(query, this.sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    Console.WriteLine("{0} \t {1} \t {2}", sqlDataReader[0], sqlDataReader[1], sqlDataReader[2]);
                    nameList += sqlDataReader[0].ToString() + " ";
                }
            }
            return nameList;
        }
        //UC 7: Ability to retrieve entries sorted alphabetically
        public string PrintSortDataBasedOnCity(string City)
        {
            string nameList = "";
            //query to be executed
            string query = "select * from Address_Book_Table where City='" + City + "' order by(FirstName)";
            SqlCommand sqlCommand = new SqlCommand(query, this.sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    DisplayEmployeeDetails(sqlDataReader);
                    nameList += sqlDataReader["FirstName"].ToString() + " ";
                }
            }
            return nameList;
        }
        //UC 8: Ability to get number of contact persons by Type
        public string ContactDataBasedOnType()
        {
            string nameList = "";
            //query to be executed
            string query = @"select Count(*) as NumberOfContacts,Type from Address_Book_Table Group by Type";
            SqlCommand sqlCommand = new SqlCommand(query, this.sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    Console.WriteLine("{0} \t {1}", sqlDataReader[0], sqlDataReader[1]);
                    nameList += sqlDataReader[0].ToString() + " ";
                }
            }
            return nameList;
        }

        public void DisplayEmployeeDetails(SqlDataReader sqlDataReader)
        {

            addressBook.FirstName = Convert.ToString(sqlDataReader["FirstName"]);
            addressBook.LastName = Convert.ToString(sqlDataReader["LastName"]);
            addressBook.Address = Convert.ToString(sqlDataReader["Address"] + " " + sqlDataReader["City"] + " " + sqlDataReader["State"] + " " + sqlDataReader["zip"]);
            addressBook.PhoneNumber = Convert.ToInt64(sqlDataReader["PhoneNumber"]);
            addressBook.Email = Convert.ToString(sqlDataReader["email"]);
            addressBook.AddressBookName = Convert.ToString(sqlDataReader["AddressBookName"]);
            addressBook.Type = Convert.ToString(sqlDataReader["TypeOfAddressBook"]);
            Console.WriteLine("{0} | {1} | {2} | {3} | {4} | {5} | {6}", addressBook.FirstName, addressBook.LastName, addressBook.Address, addressBook.PhoneNumber, addressBook.Email, addressBook.AddressBookName, addressBook.Type);

        }
    }
}