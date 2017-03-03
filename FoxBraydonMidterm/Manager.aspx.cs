using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FoxBraydonMidterm
{
    public partial class Manager : System.Web.UI.Page
    {
        int selectedBugId = 0;
        int enteredByUserId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (userNotAuthorized())
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                fillDropDownLists();
            }
        }

        private void fillDropDownLists()
        {
            fillBugDropDownList();
            fillDeveloperDropDownList();
            showSelectedBugInfo();
        }

        protected void bugDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            showSelectedBugInfo();
        }

        protected void assignButton_Click(object sender, EventArgs e)
        {
            assignBugToDeveloper();
            reloadBugDropDownList();
            successLabel.Text = "Bug successfully assigned.";
        }

        private void fillBugDropDownList()
        {
            string databaseQuery = "select [BugID], [Subject] from Bugs where [Status] = 'Open'";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            databaseConnection.Open();
            OleDbDataReader databaseReader = databaseCommand.ExecuteReader();
            while(databaseReader.Read())
            {
                string bugId = databaseReader["BugID"].ToString();
                string bugSubject = databaseReader["Subject"].ToString();
                ListItem bugListItem = new ListItem(bugSubject, bugId);
                bugDropDownList.Items.Add(bugListItem);
            }
            databaseReader.Close();
            databaseConnection.Close();
        }

        private void fillDeveloperDropDownList()
        {
            string databaseQuery = "select [UserID], [Name] from Users where [Type] = 'Developer'";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            databaseConnection.Open();
            OleDbDataReader databaseReader = databaseCommand.ExecuteReader();
            while(databaseReader.Read())
            {
                string userId = databaseReader["UserID"].ToString();
                string userName = databaseReader["Name"].ToString();
                ListItem developerListItem = new ListItem(userName, userId);
                developerDropDownList.Items.Add(developerListItem);
            }
            databaseReader.Close();
            databaseConnection.Close();
        }

        private void showSelectedBugInfo()
        {
            selectedBugId = Int32.Parse(bugDropDownList.SelectedValue);
            bugIdLabel.Text = "Bug ID: " + selectedBugId.ToString();
            setBugDetailLabels();
            setEnteredByLabel(enteredByUserId);
        }

        private void setBugDetailLabels()
        {
            string databaseQuery = "select [EnteredBy], [Priority], [Subject], [Description] from Bugs where [BugID]=@bugId";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            databaseCommand.Parameters.AddWithValue("@bugId", selectedBugId);
            databaseConnection.Open();
            OleDbDataReader databaseReader = databaseCommand.ExecuteReader();
            while (databaseReader.Read())
            {
                bugPriorityLabel.Text = "Priority: " + databaseReader["Priority"].ToString();
                bugSubjectLabel.Text = databaseReader["Subject"].ToString();
                bugDescriptionLabel.Text = databaseReader["Description"].ToString();
                enteredByUserId = Int32.Parse(databaseReader["EnteredBy"].ToString());
            }
            databaseReader.Close();
            databaseConnection.Close();
        }

        private void setEnteredByLabel(int userId)
        {
            string userName = getUserNameFromId(userId);
            enteredByLabel.Text = "Entered by: " + userName;
        }

        private string getUserNameFromId(int userId)
        {
            string userName = "";
            string databaseQuery = "select [Name] from Users where UserID=@id";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            databaseCommand.Parameters.AddWithValue("@id", enteredByUserId);
            databaseConnection.Open();
            OleDbDataReader databaseReader = databaseCommand.ExecuteReader();
            while (databaseReader.Read())
            {
                userName = databaseReader["Name"].ToString();
            }
            databaseReader.Close();
            databaseConnection.Close();
            return userName;
        }

        private void assignBugToDeveloper()
        {
            selectedBugId = Int32.Parse(bugDropDownList.SelectedValue.ToString());
            string selectedDeveloperId = developerDropDownList.SelectedValue.ToString();
            string databaseQuery = "update Bugs set [Status] = 'Assigned', [AssignedTo] = @u where BugID = @b";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            databaseCommand.Parameters.AddWithValue("@u", selectedDeveloperId);
            databaseCommand.Parameters.AddWithValue("@b", selectedBugId);
            databaseConnection.Open();
            databaseCommand.ExecuteNonQuery();
            databaseConnection.Close();
        }

        private void reloadBugDropDownList()
        {
            bugDropDownList.Items.Clear();
            fillBugDropDownList();
        }

        private bool userNotAuthorized()
        {
            return userNotLoggedIn() || userNotAManager();
        }

        private bool userNotLoggedIn()
        {
            return Session["userType"] == null;
        }

        private bool userNotAManager()
        {
            return Session["userType"].ToString() != "Manager";
        }
    }
}