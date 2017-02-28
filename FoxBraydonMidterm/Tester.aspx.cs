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
    public partial class Tester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (userNotAuthorized())
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void submitButton_Click(object sender, EventArgs e)
        {
            if(!emptyFieldsExist())
            {
                attemptBugSubmission();
            }
            else
            {
                showEmptyFieldsErrorMessage();
            }
        }

        private bool emptyFieldsExist()
        {
            return subjectTextBox.Text.Equals("") || descriptionTextBox.Text.Equals("");
        }

        private void attemptBugSubmission()
        {
            try
            {
                sendBugInfoToDatabase();
                showSuccessMessage();
            }
            catch
            {
                showFailureMessage();
            }
        }

        private void showEmptyFieldsErrorMessage()
        {
            submissionResultLabel.ForeColor = System.Drawing.Color.Red;
            submissionResultLabel.Text = "All fields must be filled out to submit a bug.";
        }

        private void sendBugInfoToDatabase()
        {
            string databaseQuery = "Insert into Bugs ([EnteredBy], [Subject], [Priority], [Description], [Status]) values (@ent, @subj, @pri, @desc, @stat)";
            OleDbConnection databaseConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            OleDbCommand databaseCommand = new OleDbCommand(databaseQuery, databaseConnection);
            addQueryParameters(databaseCommand);
            runInsertQuery(databaseConnection, databaseCommand);
        }

        private void addQueryParameters(OleDbCommand databaseCommand)
        {
            databaseCommand.Parameters.AddWithValue("@ent", Session["userId"].ToString());
            databaseCommand.Parameters.AddWithValue("@subj", subjectTextBox.Text);
            databaseCommand.Parameters.AddWithValue("@pri", priorityDropDownList.SelectedValue.ToString());
            databaseCommand.Parameters.AddWithValue("@desc", descriptionTextBox.Text);
            databaseCommand.Parameters.AddWithValue("@stat", "Open");
        }

        private static void runInsertQuery(OleDbConnection databaseConnection, OleDbCommand databaseCommand)
        {
            databaseConnection.Open();
            databaseCommand.ExecuteNonQuery();
            databaseConnection.Close();
        }

        private void showSuccessMessage()
        {
            submissionResultLabel.ForeColor = System.Drawing.Color.ForestGreen;
            submissionResultLabel.Text = "Bug successfully submitted.";
        }

        private void showFailureMessage()
        {
            submissionResultLabel.ForeColor = System.Drawing.Color.Red;
            submissionResultLabel.Text = "Failed to submit bug to database.";
        }

        private bool userNotAuthorized()
        {
            return userNotLoggedIn() || userNotATester();
        }

        private bool userNotLoggedIn()
        {
            return Session["userType"] == null;
        }

        private bool userNotATester()
        {
            return Session["userType"].ToString() != "Tester";
        }
    }
}