using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using mywebapp.Models;

namespace mywebapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MycontrollerController : ControllerBase
    {
        MySqlConnection connection;
        public MycontrollerController()
        {
            connection = new MySqlConnection("Server=localhost;User ID=root;Password=@Gantrum2905;Database=tntra");
            connection.Open();
        }



        [HttpGet(
            "Getdetails")]
        public IActionResult Get()
        {


            using var command = new MySqlCommand("SELECT * FROM eshaan", connection);
            using var reader = command.ExecuteReader();
            List<eshaan> list = new List<eshaan>();
            while (reader.Read())
            {
                eshaan myeshan = new eshaan() { name = reader.GetValue(0).ToString(), id = (int)reader.GetValue(1), email = reader.GetValue(2).ToString() };
                list.Add(myeshan);

                //var value = reader.GetValue(1);
                // do something with 'value'
                //return value.ToString();
            }
            return Ok(list);
        }


        [HttpPost("insertdata")]
        public IActionResult Post(String name, int id, string email)
        {
            String sql = "insert into eshaan values('" + name + "'," + id + ",'" + email + "')";
            using var command = new MySqlCommand(sql, connection);
            command.ExecuteScalar();

            return Ok(command);

        }

        [HttpDelete("deleterecords")]
        public IActionResult Delete(int id)
        {

            String sql = "delete from eshaan where id=" + id;
            using var command = new MySqlCommand(sql, connection);
            command.ExecuteScalar();
            return Ok(command);
        }
        //
    }
}