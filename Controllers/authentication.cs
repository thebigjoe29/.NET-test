    using System.Configuration;
    using System.Data.SqlTypes;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.IdentityModel.Tokens;
    using MySql.Data.MySqlClient;
    using mywebapp.Models;

    namespace mywebapp.Controllers
    {
        [ApiController]
        [Route("[controller]")]

        public class Auth : ControllerBase
        {
            MySqlConnection connection;
            public Auth()
            {

                connection = new MySqlConnection("Server=localhost;User ID=root;Password=@Gantrum2905;Database=tntra");
                connection.Open();
            }
            [HttpGet("auth")]
            public IActionResult Post(string username, string password,IConfiguration configuration)
            {
                string count = "select count(*) from userpass where username=@username";
                using var countcon = new MySqlCommand(count, connection);
                countcon.Parameters.AddWithValue("@username", username);
                var num = countcon.ExecuteScalar();
                if ((int.Parse(num.ToString()) > 0))
                {
                    string sql = "select password from userpass where username=@username";
                    using var command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@username", username);
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.GetValue(0).ToString() == password.ToString())
                        {
                            var token = GenerateJwtToken(username, configuration); // Generate JWT token
                            return Ok(new { Token = token, Message = "You have logged in successfully" });
                        }
                        else
                        {
                            return Ok("Invalid password!");
                        }
                    }
                }


                else
                {
                    return Ok("invalid username");
                }

                return BadRequest("");
            }

            private string GenerateJwtToken(string username,IConfiguration configuration)
            {
            var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["JwtSettings:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                
                // You can add more claims here if needed
            }),
            Audience = configuration["JwtSettings:Audience"],
            Issuer = configuration["JwtSettings:Issuer"],
            Expires = DateTime.UtcNow.AddHours(1), // Set token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
            }

            [HttpPost("insertData")]
            public IActionResult AddUser(string username, string password)
            {
                string sql = "insert into userpass values('" + username + "','" + password+ "')";
                using var command = new MySqlCommand(sql, connection);
                command.ExecuteScalar();
                return Ok(command);
            }

            [HttpGet("getData")]
            public IActionResult Get()
            {
                List<userpass> userpasses = new List<userpass>();
                using var command = new MySqlCommand("select * from userpass", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userpass userpass = new userpass() { username = reader.GetValue(0).ToString(), password = reader.GetValue(1).ToString() };
                    userpasses.Add(userpass);
                }
                return Ok(userpasses);
            }

        }


    }