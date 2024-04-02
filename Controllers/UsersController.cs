using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using diplom_api.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static diplom_api.Controllers.UsersController;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Drawing.Printing;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Packaging.Signing;

namespace diplom_api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly kursach_diplomContext _context;

		public UsersController(kursach_diplomContext context)
		{
			_context = context;
		}


		// Генирация токена
		public static string generateToken(User user)
		{

			if (user != null)
			{

				var claims = new List<Claim>
				{

					new Claim(ClaimsIdentity.DefaultNameClaimType, user.IdUsers.ToString()),
					new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleId.ToString())
				};
				ClaimsIdentity claimsIdentity =
				new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
					ClaimsIdentity.DefaultRoleClaimType);
				var identity = claimsIdentity;
				var jwt = new JwtSecurityToken(
					issuer: AuthOptions.ISSUER,
				audience: AuthOptions.AUDIENCE,
				notBefore: DateTime.UtcNow,
				claims: identity.Claims,
					expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
					signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
				return new JwtSecurityTokenHandler().WriteToken(jwt);
			}
			return null;
		}
		// Обновление токена после истечения срока его работы
		[HttpPost("new_token")]
		public async Task<ActionResult> RefreshToken(string token)
		{
			User user = _context.Users.FirstOrDefault(x => x.IdUsers == GetTokenInfo(token));
			var t = generateToken(user);

			return Ok(t);

		}
		// Получение информации из токена
		private int GetTokenInfo(string token)
		{
			var t = new JwtSecurityTokenHandler().ReadJwtToken(token);

			return Convert.ToInt32(t.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
		}
		// Генерация соли для пользователя
		private string generateSalt()
		{
			byte[] bytes = new byte[32];

			Random.Shared.NextBytes(bytes);
			return Convert.ToBase64String(bytes).ToString();
		}
		// Генерирование хешированного пароля
		private string generateHashPassword(string password, string salt)
		{

			string passwordSalt = $"{password}{salt}";

			var passwordByte = Encoding.UTF8.GetBytes(passwordSalt);

			var sha = new SHA256Managed();

			var hashPasswordByte = sha.ComputeHash(passwordByte);

			return Convert.ToBase64String(hashPasswordByte);
		}

		// Метод сравнения хешированых паролей
		private bool isCheckPassword(string salt, string oldPassword, string newPassword)
		{
			string hasNewPassword = generateHashPassword(newPassword, salt);
			return oldPassword == hasNewPassword;
		}







		// [Authorize(AuthenticationSchemes = "Bearer", Roles = "1")]
		// GET: api/Users
		// Вывод пользоватей с пагинацией
		[HttpGet]
		// [Authorize(AuthenticationSchemes = "Bearer", Roles = "1, 4")]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers()
		{
			if (_context.Users == null)
			{
				return NotFound();
			}
			return await _context.Users.ToListAsync();
		}
		public class Aut
		{
			public string userName { get; set; }
			public string password { get; set; }
		}

		[HttpPost("sign_in")]
		public async Task<ActionResult<Auth>> SignIn(
			[FromBody]
			Aut test
		  )
		{

			var resultUser = _context.Users.FirstOrDefault(x => x.LoginUsers == test.userName);

			if (resultUser == null)
			{
				return BadRequest("Такого пользователя не существует");
			}


			var saltpassword = generateHashPassword(test.password, resultUser.Salt);

			if (!isCheckPassword(resultUser.Salt, resultUser.PasswordUsers, test.password))
			{
				return BadRequest("Пароль не совпадает");
			}


			var token = generateToken(resultUser);


			return Ok(token);

		}
		private bool UserExists(int? id)
		{
			return (_context.Users?.Any(e => e.IdUsers == id)).GetValueOrDefault();
		}
		// [Authorize(AuthenticationSchemes = "Bearer", Roles = "1")]
		// GET: api/Users/5
		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetUser(int? id)
		{
			if (_context.Users == null)
			{
				return NotFound();
			}
			var user = await _context.Users.FindAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			return user;
		}

		// PUT: api/Users/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		// [Authorize(AuthenticationSchemes = "Bearer", Roles = "1")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutUser(int? id, User user)
		{
			if (id != user.IdUsers)
			{
				return BadRequest();
			}

			_context.Entry(user).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Users
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		// [Authorize(AuthenticationSchemes = "Bearer", Roles = "1")]
		[HttpPost]
		public async Task<ActionResult<User>> PostUser(User user)
		{
			byte[] Salt = GenerateSalts(20);
			user.Salt = Convert.ToBase64String(Salt);
			byte[] passwordBytes = Encoding.UTF8.GetBytes(user.PasswordUsers);
			byte[] hashedBytes = new Rfc2898DeriveBytes(passwordBytes, Salt, 10000).GetBytes(32);
			user.PasswordUsers = Convert.ToBase64String(hashedBytes);

			if (_context.Users == null)
			{
				return Problem("Entity set 'MyDbContext.Users'  is null.");
			}
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetUser", new { id = user.PasswordUsers }, user);
		}


		// DELETE: api/Users/5
		// [Authorize(AuthenticationSchemes = "Bearer", Roles = "1")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(int? id)
		{
			if (_context.Users == null)
			{
				return NotFound();
			}
			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		// Логическое удаление и востановление одного пользователя

		
		

		[HttpGet("{Login}/{Password}")]
		public async Task<ActionResult<string>> Authorization(string Login, string Password)
		{
			var users = await _context.Users.Where(u => u.LoginUsers == Login).ToListAsync();

			if (users.Count == 0)
			{
				// пользователь не найден
				return BadRequest("No usernames detected");
			}
			else if (users.Count > 1)
			{
				// обнаружено несколько пользователей с таким именем
				return BadRequest("Multiple usernames detected");
			}

			var user = users[0];
			// преобразовываем строку Salt в массив байтов
			byte[] saltBytes = Convert.FromBase64String(user.Salt);

			// преобразовываем строку Password в массив байтов
			byte[] passwordBytes = Encoding.UTF8.GetBytes(Password);

			// вычисляем хеш пароля с помощью соли и 10000 итераций
			byte[] hashBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 10000).GetBytes(32);
			string hashedPassword = Convert.ToBase64String(hashBytes);

			if (hashedPassword == user.PasswordUsers)
			{
				// пароль совпадает, генерируем случайный токен и добавляем его в базу данных
				string token;
				Models.Token existingToken;

				do
				{
					token = Guid.NewGuid().ToString();
					existingToken = await _context.Tokens.FirstOrDefaultAsync(t => t.Token1 == token);
				}
				while (existingToken != null);

				Models.Token tok = new Models.Token();
				tok.Token1 = token;
				tok.TokenDateTime = DateTime.Now;
				_context.Tokens.Add(tok);
				await _context.SaveChangesAsync();

				return token;
			}
			else
			{
				// пароль не совпадает
				return BadRequest("Неправильный пароль");
			}
		}

		public static byte[] GenerateSalts(int length)
		{
			byte[] salt = new byte[length];
			using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}
			return salt;
		}

		// GET: api/Users
		[HttpGet("auth_key")]
		public async Task<ActionResult<string>> GetAuthKey(string LoginUser)
		{
			var user = await _context.Users.SingleOrDefaultAsync(u => u.LoginUsers == LoginUser);
			if (user == null)
			{
				return NotFound($"Admin with login '{LoginUser}' was not found.");
			}
			else if (_context.Users.Count(u => u.LoginUsers == LoginUser) > 1)
			{
				return BadRequest($"Multiple admins with login '{LoginUser}' were found.");
			}

			string salt = user.Salt;
			if (string.IsNullOrEmpty(salt))
			{
				return BadRequest($"Salt for user with login '{LoginUser}' is missing or empty.");
			}

			byte[] saltBytes = Encoding.UTF8.GetBytes(salt.Substring(0, Math.Min(salt.Length, 5)));
			byte[] reverseSalt = saltBytes.Reverse().ToArray();
			string hashedReverse = Convert.ToBase64String(reverseSalt);

			return hashedReverse;
		}


		// GET: api/Users
		[HttpGet("authentication")]
		public async Task<ActionResult<string>> GetAuthentication(string LoginUser, string AuthKey)
		{
			// Retrieve the user's password salt from the database
			var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginUsers == LoginUser);
			if (user == null)
			{
				return BadRequest("Invalid LoginUser");
			}
			var salt = user.Salt;

			// Compute the AuthKey from the password salt
			byte[] saltBytes = Encoding.UTF8.GetBytes(salt.Substring(0, Math.Min(salt.Length, 5)));
			byte[] reverseSalt = saltBytes.Reverse().ToArray();
			string hashedReverse = Convert.ToBase64String(reverseSalt);

			// Check if the computed AuthKey matches the provided AuthKey
			if (hashedReverse != AuthKey)
			{
				return BadRequest("Invalid AuthKey");
			}

			// Generate a random token and add it to the database
			string token;
			Models.Token existingToken;

			do
			{
				token = Guid.NewGuid().ToString();
				existingToken = await _context.Tokens.FirstOrDefaultAsync(t => t.Token1 == token);
			}
			while (existingToken != null);

			Models.Token tok = new Models.Token();
			tok.Token1 = token;
			tok.TokenDateTime = DateTime.Now;
			_context.Tokens.Add(tok);
			await _context.SaveChangesAsync();

			return token;


		}


	}
}