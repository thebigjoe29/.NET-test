using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        public IActionResult Post(string username, string password)
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
                        return Ok("you have logged in successfully");
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