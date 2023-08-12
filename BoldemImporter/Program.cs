using BoldemImporter;
using System;
using System.Xml;

namespace XMLLoadingExample
{
	class Program
	{
		static async Task Main(string[] args)
		{
			string xmlFilePath = @"C:\Users\adamk\OneDrive\Plocha\Programming\C#\BoldemImporter\userData.xml";
			TokenManager tokenManager = new TokenManager();


			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(xmlFilePath);
			XmlNodeList userList = xmlDoc.GetElementsByTagName("user");
			List<User> users = new List<User>();

			foreach (XmlNode userNode in userList)
			{
				User user = new User();
				user.Firstname = userNode.SelectSingleNode("firstname").InnerText;
				user.Lastname = userNode.SelectSingleNode("lastname").InnerText;
				user.Email = userNode.SelectSingleNode("email").InnerText;
				users.Add(user);
			}
		}
	}
}
