﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProjectTemplate
{
	/*
	 * Varun S 
	 * 2/9/2021
	 * Added web services to update an account and validate correct password entry by the user for the edit page. 
	 * 
	 * 2/10/2021 
	 * Varun S
	 * Added web services for log on and log off functions.
	 */


	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	[System.Web.Script.Services.ScriptService]
	public class ProjectServices : System.Web.Services.WebService
	{
		private string userID = "group4spring2021";
		private string dbPass = "spring2021group4";
		private string dbName = "group4spring2021";

		////////////////////////////////////////////////////////////////////////
		///call this method anywhere that you need the connection string!
		////////////////////////////////////////////////////////////////////////
		private string GetConString()
		{
			return "SERVER=107.180.1.16; PORT=3306; DATABASE=" + dbName + "; UID=" + userID + "; PASSWORD=" + dbPass;
		}
		////////////////////////////////////////////////////////////////////////

		//TODO - write delete web service.
		[WebMethod(EnableSession = true)]
		public bool DeleteAccount(string id)
		{
			if (Session["ID"] != null)
			{
				string sqlDeletion = "DELETE FROM User WHERE ID=@id";

				MySqlConnection sqlConnection = new MySqlConnection(GetConString());
				MySqlCommand sqlCommand = new MySqlCommand(sqlDeletion, sqlConnection);

				sqlCommand.Parameters.AddWithValue("@id", HttpUtility.UrlDecode(id));

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
			else return false;
		}

		[WebMethod(EnableSession = true)]
		public bool DeleteInactiveAccounts()
		{
			string sqlDeletion = "DELETE FROM User WHERE DATEDIFF(CURDATE(), DayLastUsed) >= 365";

			MySqlConnection sqlConnection = new MySqlConnection(GetConString());
			MySqlCommand sqlCommand = new MySqlCommand(sqlDeletion, sqlConnection);

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
		public Account[][] GetAccounts()
		{
			// Check whether if the user is logged in or not. If not, return nothing.
			if (Session["ID"] != null)
			{
				// Exclude the admin who is performing the view operation.
				string sqlQuery = "SELECT ID, FirstName, LastName, Email, Username, Password, IsAdmin, " +
					"DATE_FORMAT(DayLastUsed, '%m/%d/%Y') AS DayLastUsed2 FROM User " + "WHERE ID != @sessionID";

				int sessionID = Convert.ToInt32(Session["ID"]);

				MySqlConnection sqlConnection = new MySqlConnection(GetConString());
				MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@sessionID", HttpUtility.UrlDecode(Convert.ToString(sessionID)));

				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
				DataTable queryResults = new DataTable("Accounts");
				sqlDataAdapter.Fill(queryResults);

				List<List<Account>> allAccounts = new List<List<Account>>();
				List<Account> activeAccounts = new List<Account>();
				List<Account> inactiveAccounts = new List<Account>();

				for (int i = 0; i < queryResults.Rows.Count; i++)
				{
					// Only admins can view the account list, no one else.
					if (Convert.ToInt32(Session["IsAdmin"]) == 1)
					{
						bool isUserAdmin = false;

						// Converts tiny int storage in database to boolean flag value
						if (Convert.ToInt32(queryResults.Rows[i]["IsAdmin"]) != 0)
						{
							isUserAdmin = true;
						}

						Account tmpAccount = new Account
						{
							Id = Convert.ToInt32(queryResults.Rows[i]["ID"]),
							FirstName = queryResults.Rows[i]["FirstName"].ToString(),
							LastName = queryResults.Rows[i]["LastName"].ToString(),
							Email = queryResults.Rows[i]["Email"].ToString(),
							Username = queryResults.Rows[i]["Username"].ToString(),
							IsAdmin = isUserAdmin,
							DayLastUsed = DateTime.Parse(queryResults.Rows[i]["DayLastUsed2"].ToString())
						};

						if (tmpAccount.DaysInactive >= 10)
						{
							activeAccounts.Add(tmpAccount);
						}
						else inactiveAccounts.Add(tmpAccount);

					}
					else
					{
						allAccounts.Add(null);
					}
				}

				allAccounts.Add(activeAccounts);
				allAccounts.Add(inactiveAccounts);
				try
				{
					return allAccounts.Select(lst => lst.ToArray()).ToArray();
				}
				catch (Exception e)
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}

		[WebMethod(EnableSession = true)]
		public Account GetAccount()
		{
			if (Session["ID"] != null)
			{
				string sqlQuery = "SELECT ID, FirstName, LastName, Email, Username, Password " +
					"FROM User " + "WHERE ID = @sessionID";
				int sessionID = Convert.ToInt32(Session["ID"]);
				MySqlConnection sqlConnection = new MySqlConnection(GetConString());
				MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@sessionID", HttpUtility.UrlDecode(Convert.ToString(sessionID)));

				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
				DataTable queryResults = new DataTable("Accounts");
				sqlDataAdapter.Fill(queryResults);


				if (queryResults.Rows.Count == 1)
				{
					return new Account
					{
						Id = Convert.ToInt32(queryResults.Rows[0]["ID"]),
						FirstName = queryResults.Rows[0]["FirstName"].ToString(),
						LastName = queryResults.Rows[0]["LastName"].ToString(),
						Email = queryResults.Rows[0]["Email"].ToString(),
						Username = queryResults.Rows[0]["Username"].ToString(),
					};
				}
				else
				{
					return null;
				}
			}

			return null;

		}

		[WebMethod(EnableSession = true)]
		public bool CheckAdmin()
		{
			bool admin = false;

			int check = Convert.ToInt32(Session["IsAdmin"]);

			if (check == 1)
			{
				admin = true;
			}
			return admin;
		}

		[WebMethod(EnableSession = true)]
		public bool LogOn(string username, string password)
		{
			bool doCredentialsMatch = false;
			string sqlQuery = "SELECT ID, IsAdmin FROM User WHERE Username=@username AND Password=@password";

			MySqlConnection sqlConnection = new MySqlConnection(GetConString());
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
				UpdateLogOn();

				if (Convert.ToInt32(Session["IsAdmin"]) == 1)
				{
					DeleteInactiveAccounts();
				}
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
				MySqlConnection con = new MySqlConnection(GetConString());
				////////////////////////////////////////////////////////////////////////

				MySqlCommand cmd = new MySqlCommand(testQuery, con);
				MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
				DataTable table = new DataTable();
				adapter.Fill(table);
				return "Success!";
			}
			catch (Exception e)
			{
				return "Something went wrong, please check your credentials and db name and try again.  Error: " + e.Message;
			}
		}

		[WebMethod(EnableSession = true)]
		public bool UpdateLogOn()
		{
			if (Session["ID"] != null)
			{
				string sqlSelect = "update User set DayLastUsed=CURDATE() where ID=@userId";
				MySqlConnection sqlConnection = new MySqlConnection(GetConString());
				MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

				sqlCommand.Parameters.AddWithValue("@userId", HttpUtility.UrlDecode(Convert.ToInt32(Session["ID"]).ToString()));

				sqlConnection.Open();

				try
				{
					sqlCommand.ExecuteNonQuery();
					return true;
				}
				catch (Exception ex)
				{
					return false;
				}
				finally
				{
					sqlConnection.Close();
				}
			}
			else return false;
		}


		[WebMethod(EnableSession = true)]
		public string UpdateAccount(string userId, string firstName, string lastName, string emailAddress,
			string username, string currentPword, string newPword)
		{
			if (Session["ID"] != null)
			{
				string sqlConnectString = GetConString();

				// If the current password is wrong, prevent the update from carrying on any further.
				if (!ValidateCurrentPword(userId, currentPword))
				{
					return "Incorrect password.";
				}

				if (ValidateEmail(userId, emailAddress))
				{
					return "This email is already associated with an account.";
				}

				if (ValidateUsername(userId, username))
				{
					return "This username is already associated with an account.";
				}

				string sqlSelect = "update User set FirstName=@firstName, LastName=@lastName, " +
					"Email=@emailAddress, Username=@username, Password=@newPword where ID=@userId";

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
					return "Successfully completed.";
				}
				catch (Exception e)
				{
					return "Error.";
				}
				finally
				{
					sqlConnection.Close();
				}
			}

			return "Not logged in.";

		}

		[WebMethod(EnableSession = true)]
		private bool ValidateCurrentPword(string id, string currentPword)
		{
			bool pwordIsMatch = false;

			// Establish a proper SQL query to be executed against the MySQL DB table.
			string sqlQuery = "SELECT ID, Password FROM User WHERE ID =@Id AND Password = @currentPword";
			MySqlConnection sqlConnection = new MySqlConnection(GetConString());
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

		[WebMethod(EnableSession = true)]
		private bool ValidateEmail(string id, string email)
		{
			bool emailExists = false;

			// Establish a proper SQL query to be executed against the MySQL DB table.
			string sqlQuery = "SELECT Email FROM User WHERE Email = @email AND ID != @id";
			MySqlConnection sqlConnection = new MySqlConnection(GetConString());
			MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);

			// Ensures security of SQL query by preventing SQL injection.
			sqlCommand.Parameters.AddWithValue("@email", HttpUtility.UrlDecode(email));
			sqlCommand.Parameters.AddWithValue("@id", HttpUtility.UrlDecode(id));

			// Fill data set with query result checking if the email is taken.
			MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
			DataTable queryResults = new DataTable();
			sqlDataAdapter.Fill(queryResults);

			if (queryResults.Rows.Count > 0)
			{
				emailExists = true;
			}

			return emailExists;
		}

		[WebMethod(EnableSession = true)]
		private bool ValidateUsername(string id, string username)
		{
			bool usernameExists = false;

			// Establish a proper SQL query to be executed against the MySQL DB table.
			string sqlQuery = "SELECT Username FROM User WHERE Username=@username AND ID != @id";
			MySqlConnection sqlConnection = new MySqlConnection(GetConString());
			MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);

			// Ensures security of SQL query by preventing SQL injection.
			sqlCommand.Parameters.AddWithValue("@username", HttpUtility.UrlDecode(username));
			sqlCommand.Parameters.AddWithValue("@id", HttpUtility.UrlDecode(id));

			// Fill data set with query result checking if the username is taken.
			MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
			DataTable queryResults = new DataTable();
			sqlDataAdapter.Fill(queryResults);

			if (queryResults.Rows.Count > 0)
			{
				usernameExists = true;
			}

			return usernameExists;
		}

		[WebMethod(EnableSession = true)]
		private bool ValidateNewEmail(string email)
		{
			bool emailExists = false;

			// Establish a proper SQL query to be executed against the MySQL DB table.
			string sqlQuery = "SELECT Email FROM User WHERE Email = @email";
			MySqlConnection sqlConnection = new MySqlConnection(GetConString());
			MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);

			// Ensures security of SQL query by preventing SQL injection.
			sqlCommand.Parameters.AddWithValue("@email", HttpUtility.UrlDecode(email));

			// Fill data set with query result checking if the email is taken.
			MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
			DataTable queryResults = new DataTable();
			sqlDataAdapter.Fill(queryResults);

			if (queryResults.Rows.Count > 0)
			{
				emailExists = true;
			}

			return emailExists;
		}

		[WebMethod(EnableSession = true)]
		private bool ValidateNewUsername(string username)
		{
			bool usernameExists = false;

			// Establish a proper SQL query to be executed against the MySQL DB table.
			string sqlQuery = "SELECT Username FROM User WHERE Username=@username";
			MySqlConnection sqlConnection = new MySqlConnection(GetConString());
			MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);

			// Ensures security of SQL query by preventing SQL injection.
			sqlCommand.Parameters.AddWithValue("@username", HttpUtility.UrlDecode(username));

			// Fill data set with query result checking if the username is taken.
			MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
			DataTable queryResults = new DataTable();
			sqlDataAdapter.Fill(queryResults);

			if (queryResults.Rows.Count > 0)
			{
				usernameExists = true;
			}

			return usernameExists;
		}

		[WebMethod(EnableSession = true)]
		public string CreateAccount(string firstName, string lastName, string emailAddress,
			string username, string Pword)
		{

			string sqlConnectString = GetConString();

			if (ValidateNewEmail(emailAddress))
			{
				return "This email is already associated with an account.";
			}

			if (ValidateNewUsername(username))
			{
				return "This username is already associated with an account.";
			}

			string sqlSelect = "INSERT INTO User (FirstName, LastName, Email, Username, Password, DayLastUsed) " +
				"values (@firstName, @lastName, @emailAddress, @username, @Pword, @currentDate)";


			MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
			MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

			sqlCommand.Parameters.AddWithValue("@firstName", HttpUtility.UrlDecode(firstName));
			sqlCommand.Parameters.AddWithValue("@lastName", HttpUtility.UrlDecode(lastName));
			sqlCommand.Parameters.AddWithValue("@emailAddress", HttpUtility.UrlDecode(emailAddress));
			sqlCommand.Parameters.AddWithValue("@username", HttpUtility.UrlDecode(username));
			sqlCommand.Parameters.AddWithValue("@Pword", HttpUtility.UrlDecode(Pword));
			sqlCommand.Parameters.AddWithValue("@currentDate", DateTime.Now);
			sqlConnection.Open();
			try
			{
				int accountID = Convert.ToInt32(sqlCommand.ExecuteScalar());
				return "Successfully completed.";
			}
			catch (Exception e)
			{
				return "Error.";
			}
			finally
			{
				sqlConnection.Close();
			}

		}

		[WebMethod(EnableSession = true)]
		public string MakeRecommendations(string username, string emailAddress, string soundDesc)
		{
			if (Session["ID"] != null)
			{
				string sqlConnectString = GetConString();

				string sqlQuery = "SELECT ID, FirstName, LastName, Email, Username, Password " +
					"FROM User " + "WHERE ID = @sessionID";
				int sessionID = Convert.ToInt32(Session["ID"]);
				MySqlConnection sqlConnection = new MySqlConnection(GetConString());
				MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@sessionID", HttpUtility.UrlDecode(Convert.ToString(sessionID)));

				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
				DataTable queryResults = new DataTable("Accounts");
				sqlDataAdapter.Fill(queryResults);


				if (queryResults.Rows.Count == 1)
				{
					var Email = queryResults.Rows[0]["Email"].ToString();
					var Username = queryResults.Rows[0]["Username"].ToString();

					string sqlSelect = "INSERT INTO Recommendations (username, Email, Description, DateSubmitted) " +
					"values (@username, @emailAddress, @Description, @currentDate)";

					MySqlConnection sqlConnection2 = new MySqlConnection(sqlConnectString);
					MySqlCommand sqlCommand2 = new MySqlCommand(sqlSelect, sqlConnection2);

					sqlCommand2.Parameters.AddWithValue("@emailAddress", Email);
					sqlCommand2.Parameters.AddWithValue("@username", Username);
					sqlCommand2.Parameters.AddWithValue("@Description", HttpUtility.UrlDecode(soundDesc));
					sqlCommand2.Parameters.AddWithValue("@currentDate", DateTime.Now);
					sqlConnection2.Open();
					try
					{
						int accountID = Convert.ToInt32(sqlCommand2.ExecuteScalar());
						return "Successfully completed.";
					}
					catch (Exception e)
					{
						return "Error.";
					}
					finally
					{
						sqlConnection.Close();
					}
				}

			}

			return "Not logged in.";
		}

		[WebMethod(EnableSession = true)]
		public Rec[] ListRecommendations()
		{
			string sqlQuery = "SELECT username, Email, Description , DateSubmitted FROM Recommendations";

			MySqlConnection sqlConnection = new MySqlConnection(GetConString());
			MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);

			MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
			DataTable queryResults = new DataTable("Recs");
			sqlDataAdapter.Fill(queryResults);

			List<Rec> allRecs = new List<Rec>();

			for (int i = 0; i < queryResults.Rows.Count; i++)
			{

				Rec tmpRec = new Rec
				{

					Email = queryResults.Rows[i]["Email"].ToString(),
					Username = queryResults.Rows[i]["Username"].ToString(),
					Description = queryResults.Rows[i]["Description"].ToString(),
					DateSubmitted = queryResults.Rows[i]["DateSubmitted"].ToString().Split( )[0]
				};

				allRecs.Add(tmpRec);
			}
				try
				{
					return allRecs.ToArray();
				}
				catch (Exception e)
				{
					return null;
				}

			}

		}
	}
