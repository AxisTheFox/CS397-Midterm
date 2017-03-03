using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FoxBraydonMidterm
{
    public partial class Developer : System.Web.UI.Page
    {
        string selectedBugId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (userNotAuthorized())
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                fillAssignedBugsDropDownList();
                selectedBugId = assignedBugsDropDownList.SelectedValue;
                showSelectedBugInfo();
            }
            selectedBugId = assignedBugsDropDownList.SelectedValue;
        }

        protected void assignedBugsDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedBugId = assignedBugsDropDownList.SelectedValue;
            showSelectedBugInfo();
        }

        private void fillAssignedBugsDropDownList()
        {
            string databaseQuery = "select [BugId], [Subject] from Bugs where [AssignedTo]=@thisUser and [Status]='Assigned'";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            databaseCommand.Parameters.AddWithValue("@thisUser", Session["userId"]);
            databaseConnection.Open();
            OleDbDataReader databaseReader = databaseCommand.ExecuteReader();
            while (databaseReader.Read())
            {
                string bugSubject = databaseReader["Subject"].ToString();
                string bugId = databaseReader["BugID"].ToString();
                assignedBugsDropDownList.Items.Add(new ListItem(bugSubject, bugId));
            }
        }

        private void showSelectedBugInfo()
        {
            string databaseQuery = "select [Subject], [Description] from Bugs where [BugID]=@selectedBug";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            databaseCommand.Parameters.AddWithValue("@selectedBug", selectedBugId);
            databaseConnection.Open();
            OleDbDataReader databaseReader = databaseCommand.ExecuteReader();
            while (databaseReader.Read())
            {
                bugSubjectLabel.Text = "Subject: " + databaseReader["Subject"];
                bugDescriptionLabel.Text = "Description: " + databaseReader["Description"];
            }
        }

        protected void fixedButton_Click(object sender, EventArgs e)
        {
            submitFixedBug();
            reloadAssignedBugsDropDownList();
            showSelectedBugInfo();
            successLabel.Text = "Bug fix successfully submitted.";
        }

        private void submitFixedBug()
        {
            string databaseQuery = "update Bugs set [Status] = 'Completed', [Changes] = @changes where [BugID] = @selectedBug";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            databaseCommand.Parameters.AddWithValue("@changes", changesTextBox.Text);
            databaseCommand.Parameters.AddWithValue("@selectedBug", selectedBugId);
            databaseConnection.Open();
            databaseCommand.ExecuteNonQuery();
            databaseConnection.Close();
        }

        private void reloadAssignedBugsDropDownList()
        {
            assignedBugsDropDownList.Items.Clear();
            fillAssignedBugsDropDownList();
            selectedBugId = assignedBugsDropDownList.SelectedValue;
        }

        private bool userNotAuthorized()
        {
            return userNotLoggedIn() || userNotADeveloper();
        }

        private bool userNotLoggedIn()
        {
            return Session["userType"] == null;
        }

        private bool userNotADeveloper()
        {
            return Session["userType"].ToString() != "Developer";
        }
    }
}