using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FoxBraydonMidterm
{
    public partial class Login : System.Web.UI.Page
    {
        int userId = 0;
        string hashedPasswordFromDatabase = "";
        string userType = "";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void loginButton_Click(object sender, EventArgs e)
        {
            executeLoginDatabaseQuery();
            string hashedPasswordFromUser = getHashedPassword();
            if (hashedPasswordFromUser.Equals(hashedPasswordFromDatabase))
            {
                Session["userId"] = userId;
                Session["userType"] = userType;
                redirectByUserType();
            }
            else
            {
                loginErrorLabel.Text = "You've entered an invalid username or password.";
            }
        }

        private void executeLoginDatabaseQuery()
        {
            string databaseQuery = "select [UserID], [Password], [Type] from Users where [Login]=@l";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            databaseCommand.Parameters.AddWithValue("@l", usernameTextBox.Text);
            databaseConnection.Open();
            OleDbDataReader dataReader = databaseCommand.ExecuteReader();
            readData(dataReader);
            dataReader.Close();
            databaseConnection.Close();
        }

        private void readData(OleDbDataReader dataReader)
        {
            while (dataReader.Read())
            {
                userId = Int32.Parse(dataReader["UserID"].ToString());
                hashedPasswordFromDatabase = dataReader["Password"].ToString();
                userType = dataReader["Type"].ToString();
            }
        }

        private string getHashedPassword()
        {
            SHA1CryptoServiceProvider sha1Provider = new SHA1CryptoServiceProvider();
            string passwordFromUser = passwordTextBox.Text;
            sha1Provider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwordFromUser));
            byte[] result = sha1Provider.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }

        private void redirectByUserType()
        {
            switch (userType)
            {
                case "Tester":
                    Response.Redirect("Tester.aspx");
                    break;
                case "Manager":
                    Response.Redirect("Manager.aspx");
                    break;
                case "Developer":
                    Response.Redirect("Developer.aspx");
                    break;
                case "Adminstrator":
                    Response.Redirect("Administrator.aspx");
                    break;
            }
        }
    }
}