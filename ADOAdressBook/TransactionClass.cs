using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADO_AddressBook.ADO_AddressBook
{
    class TransactionClass1
    {
        //Give path for Database Connection
        public static string connection = @"Server=.;Database=Address_Book_Service_DB;Trusted_Connection=True;";
        //Represents a connection to Sql Server Database
        SqlConnection SqlConnection = new SqlConnection(connection);
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
    }
}