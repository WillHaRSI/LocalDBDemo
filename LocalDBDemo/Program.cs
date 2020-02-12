using System;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace LocalDBDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // based on documentation https://jakeydocs.readthedocs.io/en/latest/tutorials/first-mvc-app/working-with-sql.html

            SqlConnection tmpConn = new SqlConnection();

            tmpConn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; Integrated Security=True;Connect Timeout=30;Encrypt=False";
            tmpConn.Open();

            string action = "";

            while (!action.Equals("ADD") && !action.Equals("DELETE"))
            {
                Console.WriteLine("Please type an action ADD or DELETE: ");
                action = Console.ReadLine();
            }

            if (action.Equals("ADD"))
            {
                Console.WriteLine("Enter a string value to add to the table: ");
                string inputValue = Console.ReadLine();

                string sql = $"INSERT INTO StringDb.dbo.StringTable (Strings) values ('{inputValue}')";
                var command = new SqlCommand(sql, tmpConn);
                command.ExecuteNonQuery();
            }

            if (action.Equals("DELETE"))
            {
                Console.WriteLine("Enter a string value to remove from the table: ");
                string inputValue = Console.ReadLine();

                string sql = $"DELETE FROM StringDb.dbo.StringTable WHERE Strings = '{inputValue}'";
                var command = new SqlCommand(sql, tmpConn);
                command.ExecuteNonQuery();
            }

            var selectAllQuery = "SELECT Strings FROM StringDb.dbo.StringTable";
            SqlCommand myCommand = new SqlCommand(selectAllQuery, tmpConn);
            SqlDataReader sqlRead = myCommand.ExecuteReader();

            try
            {
                List<string> lstResults = new List<string>();

                while (sqlRead.Read()) lstResults.Add(sqlRead.GetString(0));

                Console.WriteLine($"Values in table ({lstResults.Count}): ");

                foreach (string s in lstResults) Console.WriteLine(s);
            }

            finally
            {
                sqlRead.Close();
                tmpConn.Close();
            }

            Console.ReadKey();
        }
    }
}
