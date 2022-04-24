using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ADO_AddressBook.ADO_AddressBook
{
    class TransactionClass1
    {
        //Give path for Database Connection
        public static string connection = @"Server=.;Database=Address_Book_Service_DB;Trusted_Connection=True;";
        //Represents a connection to Sql Server Database
        SqlConnection SqlConnection = new SqlConnection(connection);
        private object contactList;

        public int AlterTableaddStartDate()
        {
            int result = 0;
            SqlConnection.Open();
            using (SqlConnection)
            {

                //Begin SQL transaction
                SqlTransaction sqlTransaction = SqlConnection.BeginTransaction();
                SqlCommand sqlCommand = SqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    sqlCommand.CommandText = "alter table Contact_Person add Date_Added Date";
                    result = sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    Console.WriteLine("Updated!");
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    //Rollback to the point before exception
                    sqlTransaction.Rollback();
                    Console.WriteLine("Not Updated!");

                }
            }
            SqlConnection.Close();
            return result;
        }
        //Set date value based on query
        public int SetStartDateValue(string query)
        {
            int result = 0;
            SqlConnection.Open();
            using (SqlConnection)
            {
                //Begin SQL transaction
                SqlTransaction sqlTransaction = SqlConnection.BeginTransaction();
                SqlCommand sqlCommand = SqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    sqlCommand.CommandText = query;
                    result = sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    Console.WriteLine("Updated!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //Rollback to the point before exception
                    sqlTransaction.Rollback();
                    Console.WriteLine("Not Updated!");
                }
            }
            SqlConnection.Close();
            return result;
        }
        //Retrieve data based on Date_Added
        public string RetrievebasedOnDate()
        {
            string nameList = "";
            SqlConnection.Open();
            using (SqlConnection)
            {

                //Begin SQL transaction
                SqlTransaction sqlTransaction = SqlConnection.BeginTransaction();
                SqlCommand sqlCommand = SqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    sqlCommand.CommandText = @"select Concat(FirstName,' ',SecondName) as Name,Concat(Address,' ,',City,' ,',State,' ,',zip) as Address,PhoneNumber,Email,Date_Added from Contact_Person where Date_Added between Cast('2017-08-14' as date) and GetDate()";
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            Console.WriteLine("{0} \t {1} \t {2} \t {3} \t {4} \t", sqlDataReader[0], sqlDataReader[1], sqlDataReader[2], sqlDataReader[3], sqlDataReader[4]);
                            nameList += sqlDataReader[0].ToString() + " ";
                        }
                    }
                    sqlDataReader.Close();
                    sqlTransaction.Commit();
                    Console.WriteLine("Updated!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //Rollback to the point before exception
                    sqlTransaction.Rollback();
                    Console.WriteLine("Not Updated!");
                }
            }
            SqlConnection.Close();
            return nameList;
        }

        //Insert into Table using Transactions
        public int InsertIntoTables()
        {
            int result = 0;
            SqlConnection.Open();
            using (SqlConnection)
            {
                //Begin SQL transaction
                SqlTransaction sqlTransaction = SqlConnection.BeginTransaction();
                SqlCommand sqlCommand = SqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    //Insert data into Table
                    sqlCommand.CommandText = "insert into Contact_Person values(1,'Neha','Kejriwal','3645 Catherine Street','Mysore','Karnataka',223001,8842905050,'neha@gmail.com','2019-02-03')";
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "insert into TypeManager values(3,5)";
                    sqlCommand.ExecuteNonQuery();
                    //Commit 
                    sqlTransaction.Commit();
                    Console.WriteLine("Updated!");
                    result = 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //Rollback to the point before exception
                    sqlTransaction.Rollback();
                    result = 1;
                }
            }
            SqlConnection.Close();
            return result;
        }
        // Ability to Add multiple Contacts to List
        public int AddMultipleDataToList()
        {
            string nameList = "";
            //query to be executed
            string query = "select Address_BookName,FirstName,SecondName,Address,City,State,zip,PhoneNumber,Email,ContactType_Name from Contact_Person  INNER JOIN  Address_Book on Address_Book.Address_BookID=AddressBook_ID  INNER JOIN TypeManager on TypeManager.Contact_Identity=Contact_ID INNER JOIN ContactType on TypeManager.ContactType_Identity=ContactType_ID";
            SqlCommand sqlCommand = new SqlCommand(query, this.SqlConnection);
            SqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {

                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    DisplayEmployeeDetails(sqlDataReader);
                    nameList += sqlDataReader["FirstName"].ToString() + " ";
                    stopWatch.Stop();
                    Console.WriteLine("Time elapsed using Thread: {0}", stopWatch.ElapsedMilliseconds);
                }
            }
            SqlConnection.Close();
            return contactList.Count;
        }

        //Display all Object details
        public void DisplayEmployeeDetails(SqlDataReader sqlDataReader)
        {
            ContactDataManager addressBook = new ContactDataManager();
            addressBook.FirstName = Convert.ToString(sqlDataReader["FirstName"]);
            addressBook.LastName = Convert.ToString(sqlDataReader["SecondName"]);
            addressBook.Address = Convert.ToString(sqlDataReader["Address"] + " " + sqlDataReader["City"] + " " + sqlDataReader["State"] + " " + sqlDataReader["zip"]);
            addressBook.PhoneNumber = Convert.ToInt64(sqlDataReader["PhoneNumber"]);
            addressBook.Email = Convert.ToString(sqlDataReader["Email"]);
            addressBook.AddressBookName = Convert.ToString(sqlDataReader["Address_BookName"]);
            addressBook.Type = Convert.ToString(sqlDataReader["ContactType_Name"]);
            Task task = new Task(() =>
            {
                Console.WriteLine("{0} \t {1} \t {2} \t {3} \t {4} \t {5} \t {6}", addressBook.FirstName, addressBook.LastName, addressBook.Address, addressBook.PhoneNumber, addressBook.Email, addressBook.AddressBookName, addressBook.Type);
                contactList.Add(addressBook);
            });
            task.Start();
        }
    }
}