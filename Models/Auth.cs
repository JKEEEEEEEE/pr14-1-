namespace diplom_api.Models
{
	[Serializable]
	public class Auth
	{
		public Auth(string userName, string token)
		{
			this.userName = userName;
			this.token = token;
		}

		string userName { get; set; }
		string token { get; set; }
		DateTime dateNow { get => DateTime.Now; }

		// Логин 
		// Токен
		// Дата и время

	}
}
