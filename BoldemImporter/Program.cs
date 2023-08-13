using BoldemImporter;
using System;
using System.Xml;

namespace XMLLoadingExample
{
	class Program
	{
		static async Task Main(string[] args)
		{
			string xmlFilePath = "../userData.xml";
			TokenManager tokenManager = new TokenManager();

			if (!File.Exists(xmlFilePath))
			{
                await Console.Out.WriteLineAsync($"File {xmlFilePath} does not exist!");
				return;
            }

			//Load data 
			XmlDocument xmlDoc = new XmlDocument();
			try
			{
				xmlDoc.Load(xmlFilePath);
                await Console.Out.WriteLineAsync("Xml file loaded!\n");
            }
			catch (Exception ex) {await Console.Out.WriteLineAsync(ex.Message);}

            XmlNodeList userList = xmlDoc.GetElementsByTagName("user");
			List<User> contacs = new List<User>();

			foreach (XmlNode userNode in userList)
			{
				User user = new User();
				user.Firstname = userNode.SelectSingleNode("firstname").InnerText;
				user.Lastname = userNode.SelectSingleNode("lastname").InnerText;
				user.Email = userNode.SelectSingleNode("email").InnerText;
				contacs.Add(user);

				if (contacs.Count == 100)
				{
                    await Console.Out.WriteLineAsync("Inserting data...");
                    await tokenManager.InsertUsers(contacs);
                    await Console.Out.WriteLineAsync("Data inserted successfully!\n");
                    contacs.Clear();
				}
			}
			//Insert remaining records
			if (contacs.Count > 0)
			{
				await Console.Out.WriteLineAsync("Inserting data...");
				await tokenManager.InsertUsers(contacs);
				await Console.Out.WriteLineAsync("Data inserted successfully!\n");
			}
		}
	}
}
