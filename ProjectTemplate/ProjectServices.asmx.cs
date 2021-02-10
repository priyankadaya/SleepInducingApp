using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProjectTemplate
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	[System.Web.Script.Services.ScriptService]


	/*
	 * Varun S 
	 * 2/9/2021
	 * Added web services to update an account and validate correct password entry by the user for the edit page. 
	 * 
	 * 2/10/2021 
	 * Varun S
	 * Added web services for log on and log off functions.
	 */

	public class ProjectServices : System.Web.Services.WebService
	{
		private string userID = "group4spring2021";
		private string dbPass = "spring2021group4";
		private string dbName = "group4spring2021";
		
		////////////////////////////////////////////////////////////////////////
		///call this method anywhere that you need the connection string!
		////////////////////////////////////////////////////////////////////////
		private string getConString() {
			return "SERVER=107.180.1.16; PORT=3306; DATABASE=" + dbName+"; UID=" + userID + "; PASSWORD=" + dbPass;
		}
		////////////////////////////////////////////////////////////////////////

		[WebMethod(EnableSession = true)]
		public bool LogOn(string username, string password)
		{
			bool doCredentialsMatch = false;
			string sqlQuery = "SELECT ID, IsAdmin FROM User WHERE Username=@username AND Password=@password";

			MySqlConnection sqlConnection = new MySqlConnection(getConString());
			MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
			sqlCommand.Parameters.AddWithValue("@username", HttpUtility.UrlDecode(username));
			sqlCommand.Parameters.AddWithValue("@password", HttpUtility.UrlDecode(password));

			MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
			DataTable queryResults = new DataTable();
			sqlDataAdapter.Fill(queryResults);

			if (queryResults.Rows.Count == 1)
			{
				Session["ID"] = queryResults.Rows[0]["ID"];
				Session["IsAdmin"] = queryResults.Rows[0]["IsAdmin"];
				doCredentialsMatch = true;
			}

			return doCredentialsMatch;
		}

		[WebMethod(EnableSession = true)]
		public bool LogOff()
		{
			Session.Abandon();
			return true;
		}


		/////////////////////////////////////////////////////////////////////////
		//don't forget to include this decoration above each method that you want
		//to be exposed as a web service!
		[WebMethod(EnableSession = true)]
		/////////////////////////////////////////////////////////////////////////
		public string TestConnection()
		{
			try
			{
				string testQuery = "select * from User";

				////////////////////////////////////////////////////////////////////////
				///here's an example of using the getConString method!
				////////////////////////////////////////////////////////////////////////
				MySqlConnection con = new MySqlConnection(getConString());
				////////////////////////////////////////////////////////////////////////

				MySqlCommand cmd = new MySqlCommand(testQuery, con);
				MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
				DataTable table = new DataTable();
				adapter.Fill(table);
				return "Success!";
			}
			catch (Exception e)
			{
				return "Something went wrong, please check your credentials and db name and try again.  Error: "+e.Message;
			}
		}


		[WebMethod(EnableSession = true)]
		public bool UpdateAccount(string userId, string firstName, string lastName, string emailAddress, 
			string username, string currentPword, string newPword)
		{
			string sqlConnectString = getConString();

			// If the current password is wrong, prevent the update from carrying on any further.
			if (!ValidateCurrentPword(userId, currentPword))
			{
				return false;
			}

			string sqlSelect = "update User set FirstName=@firstName, LastName=@lastName, " +
				"Email=@emailAddress, Username=@username, Password=@newPword, DayLastUsed=CURDATE() where ID=@userId";

			MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
			MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

			sqlCommand.Parameters.AddWithValue("@firstName", HttpUtility.UrlDecode(firstName));
			sqlCommand.Parameters.AddWithValue("@lastName", HttpUtility.UrlDecode(lastName));
			sqlCommand.Parameters.AddWithValue("@emailAddress", HttpUtility.UrlDecode(emailAddress));
			sqlCommand.Parameters.AddWithValue("@username", HttpUtility.UrlDecode(username));
			sqlCommand.Parameters.AddWithValue("@newPword", HttpUtility.UrlDecode(newPword));
			sqlCommand.Parameters.AddWithValue("@userId", HttpUtility.UrlDecode(userId));

			sqlConnection.Open();
			
			try
			{
				sqlCommand.ExecuteNonQuery();
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
			finally
			{
				sqlConnection.Close();
			}
		}

		[WebMethod(EnableSession = true)]
		private bool ValidateCurrentPword(string id, string currentPword)
		{
			bool pwordIsMatch = false;

			// Establish a proper SQL query to be executed against the MySQL DB table.
			string sqlQuery = "SELECT ID, Password FROM User WHERE ID=@Id AND Password=@currentPword";
			MySqlConnection sqlConnection = new MySqlConnection(getConString());
			MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);

			// Ensures security of SQL query by preventing SQL injection.
			sqlCommand.Parameters.AddWithValue("@Id", HttpUtility.UrlDecode(id));
			sqlCommand.Parameters.AddWithValue("@currentPword", HttpUtility.UrlDecode(currentPword));

			// Fill data set with query result checking if the user at ID Id has Password currentPword.
			MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
			DataTable queryResults = new DataTable();
			sqlDataAdapter.Fill(queryResults);

			if (queryResults.Rows.Count == 1)
			{
				pwordIsMatch = true;
			}

			return pwordIsMatch;
		}
	}
}
