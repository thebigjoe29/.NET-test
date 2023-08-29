//using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MySql.Data.MySqlClient;
using mywebapp.Models;

namespace mywebapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class m2ndcontroller : ControllerBase
        
    {
        MySqlConnection mySqlConnection1;
        public m2ndcontroller()
        {
             mySqlConnection1 = new MySqlConnection("Server=localhost;User ID=root;Password=@Gantrum2905;Database=tntra");
            mySqlConnection1.Open();
        }
        [HttpGet("getData")]
        public IActionResult Get()
        {
            List<computer> computers = new List<computer>();
            using var command=new MySqlCommand("select * from computer",mySqlConnection1);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                computer computer = new computer() { name = reader.GetValue(0).ToString(), price = (int)reader.GetValue(1) };
                computers.Add(computer);
            }
            return Ok(computers);
        }

        [HttpPost("insertData")]
        public IActionResult Post(string name,int price)
        {
            string sql="insert into computer values('"+name+"',"+price+")";
            using var command = new MySqlCommand(sql, mySqlConnection1);
            command.ExecuteScalar();
            return Ok(command);
        }
    }
}
