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

			//Load data 
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(xmlFilePath);
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
					await tokenManager.InsertUsers(contacs);
					contacs.Clear();
				}
			}
		}
	}
}
