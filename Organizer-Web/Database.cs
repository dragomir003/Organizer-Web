using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Organizer_Web
{
    public class Database
    {
        private static string CS { get => ConfigurationManager.ConnectionStrings["main"].ConnectionString; }

        private static SqlConnection Connection { get; set; }

        public static void OpenConnection()
        {
            Connection.Open();
        }

        public static void CloseConnection()
        {
            if (Connection.State == ConnectionState.Open)
                Connection.Close();
        }

        public static void CreateConnection()
        {
            Connection = new SqlConnection(CS);
        }

        private static DataTable CreateTable(string command)
        {

            var rv = new DataTable();

            var adapter = new SqlDataAdapter(command, Connection);

            adapter.Fill(rv);

            return rv;
        }

        public static DataTable UsersProjects(int userId) => CreateTable($"select * from dbo.GetProjects({userId});");

        public static Dictionary<int, string> ProjectMembers(int projectId) => CreateTable($"select * from dbo.ClanoviProjekta({projectId}")
                                                                               .AsEnumerable()
                                                                               .Aggregate(new Dictionary<int, string>(),
                                                                                          (acc, row) => {
                                                                                              acc.Add((int)row["id"], row["username"].ToString());
                                                                                              return acc;
                                                                                          }
                                                                                );

        public static bool IsLoginPossible(string username) => (int)new SqlCommand($"select dbo.ValidateLogin('{username}');", Connection).ExecuteScalar() == 0;

        public static void AddMembership(int project, int user) => new SqlCommand($"exec dbo.DodajClanstvoBasic {user}, {project};", Connection).ExecuteNonQuery();

        public static bool Register(string username) => (int)new SqlCommand($"declare @res int; exec @res = dbo.Register '{username}'; select @res;", Connection).ExecuteScalar() == 0;

        public static int GetUserId(string username) => (int)new SqlCommand($"select top 1 id from Korisnik where username = '{username}'", Connection).ExecuteScalar();

        public static T RunWithOpenConnection<T>(Func<int, T> func, int input)
        {
            OpenConnection();
            var res = func(input);
            CloseConnection();

            return res;
        }

        public static T RunWithOpenConnection<T>(Func<string, T> func, string input)
        {
            OpenConnection();
            var res = func(input);
            CloseConnection();

            return res;
        }

        public static T RunWithOpenConnection<T>(Func<int, int, T> func, int input1, int input2)
        {
            OpenConnection();
            var res = func(input1, input2);
            CloseConnection();

            return res;
        }
    }
}