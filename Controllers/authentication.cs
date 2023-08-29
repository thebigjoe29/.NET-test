using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MySql.Data.MySqlClient;
using mywebapp.Models;

namespace mywebapp.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class auth : ControllerBase
    {
        MySqlConnection connection;
        public auth()
        {

            connection = new MySqlConnection("Server=localhost;User ID=root;Password=@Gantrum2905;Database=tntra");
            connection.Open();
        }
        [HttpGet("auth")]
        public IActionResult Post(string name, int price)
        {
            string count = "select count(*) from computer where name=@name";
            using var countcon = new MySqlCommand(count, connection);
            countcon.Parameters.AddWithValue("@name", name);
            var num = countcon.ExecuteScalar();
            if ((int.Parse(num.ToString()) > 0))
            {
                string sql = "select price from computer where name=@name";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@name", name);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetValue(0).ToString() == price.ToString())
                    {
                        return Ok("you have logged in successfully");
                    }
                    else
                    {
                        return Ok("Invalid price");
                    }
                }
            }


            else
            {
                return Ok("invalid name");
            }

            return BadRequest("");
        }
    }


}