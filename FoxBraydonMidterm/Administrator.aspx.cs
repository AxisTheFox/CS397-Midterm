using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FoxBraydonMidterm
{
    public partial class Administrator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (userNotAuthorized())
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void createUserButton_Click(object sender, EventArgs e)
        {
            resultLabel.Text = "";
            if (allFieldsAreValid())
            {
                addUser();
            }
        }

        private bool allFieldsAreValid()
        {
            if (nameIsValid() && loginUsernameIsValid() && passwordIsValid())
            {
                return true;
            }
            return false;
        }

        private bool nameIsValid()
        {
            if (nameTextBox.Text.Equals(""))
            {
                showErrorMessage("New user must have a name.");
                return false;
            }
            return true;
        }

        private bool loginUsernameIsValid()
        {
            if (loginUsernameTextBox.Text.Equals(""))
            {
                showErrorMessage("New user must have a username.");
                return false;
            }
            if (usernameIsTaken())
            {
                showErrorMessage("Username already exists.");
                return false;
            }
            return true;
        }

        private bool passwordIsValid()
        {
            if (passwordTooShort() || passwordNotAlphanumeric())
            {
                showErrorMessage("Passwords must be at least 8 characters long, and contain at least one number and one letter.");
                return false;
            }
            return true;
        }

        private bool usernameIsTaken()
        {
            DataTable usernameTable = getUsernameTable();
            if (usernameExistsIn(usernameTable))
            {
                return true;
            }
            return false;
        }

        private DataTable getUsernameTable()
        {
            string databaseQuery = "select Login from Users";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            OleDbDataAdapter databaseAdapter = new OleDbDataAdapter(databaseCommand);
            DataTable databaseTable = new DataTable();
            databaseConnection.Open();
            databaseAdapter.Fill(databaseTable);
            databaseConnection.Close();
            return databaseTable;
        }

        private bool usernameExistsIn(DataTable usernameTable)
        {
            foreach (DataRow user in usernameTable.Rows)
            {
                if (user["Login"].Equals(loginUsernameTextBox.Text))
                {
                    return true;
                }
            }
            return false;
        }

        private bool passwordTooShort()
        {
            if (passwordTextBox.Text.Length < 8)
            {
                return true;
            }
            return false;
        }

        private bool passwordNotAlphanumeric()
        {
            Regex rg = new Regex(@"^[a-zA-z0-9]*$");
            return !rg.IsMatch(passwordTextBox.Text);
        }

        private void addUser()
        {
            string userId = getNextAvailableUserId();
            string hashedPassword = getHashedPassword();
            addUserToDatabaseWith(userId, hashedPassword);
            showSuccessMessage("Successfully created user with ID: " + userId + ".");
        }

        private string getNextAvailableUserId()
        {
            DataTable userIdTable = getUserIdTable();
            int nextId = Int32.Parse(userIdTable.Rows[0][0].ToString());
            nextId++;
            return nextId.ToString();
        }

        private DataTable getUserIdTable()
        {
            string databaseQuery = "select max(UserID) from Users";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            OleDbDataAdapter databaseAdapter = new OleDbDataAdapter(databaseCommand);
            DataTable userIdTable = new DataTable();
            databaseConnection.Open();
            databaseAdapter.Fill(userIdTable);
            databaseConnection.Close();
            return userIdTable;
        }

        private string getHashedPassword()
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            string passwordFromUser = passwordTextBox.Text;
            sha1.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwordFromUser));
            byte[] result = sha1.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            string hashedPasswordFromUser = strBuilder.ToString();
            return hashedPasswordFromUser;
        }

        private void addUserToDatabaseWith(string userId, string hashedPassword)
        {
            string databaseQuery = "insert into Users([UserID], [Name], [Login], [Password], [Type]) values(@u, @n, @l, @p, @t)";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            databaseCommand.Parameters.AddWithValue("@u", userId);
            databaseCommand.Parameters.AddWithValue("@n", nameTextBox.Text);
            databaseCommand.Parameters.AddWithValue("@l", loginUsernameTextBox.Text);
            databaseCommand.Parameters.AddWithValue("@p", hashedPassword);
            databaseCommand.Parameters.AddWithValue("@t", userTypeDropDownList.SelectedValue);
            databaseConnection.Open();
            databaseCommand.ExecuteNonQuery();
            databaseConnection.Close();
        }

        private void showSuccessMessage(string message)
        {
            resultLabel.ForeColor = System.Drawing.Color.ForestGreen;
            resultLabel.Text = message;
        }

        private void showErrorMessage(string message)
        {
            resultLabel.ForeColor = System.Drawing.Color.Red;
            resultLabel.Text = message;
        }

        private bool userNotAuthorized()
        {
            return userNotLoggedIn() || userNotAdministrator();
        }

        private bool userNotLoggedIn()
        {
            return Session["userType"] == null;
        }

        private bool userNotAdministrator()
        {
            return Session["userType"].ToString() != "Administrator";
        }
    }
}