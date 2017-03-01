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
    public partial class Manager : System.Web.UI.Page
    {
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
        }

        protected void bugDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            databaseConnection.Close();
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