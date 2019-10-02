using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBHelper;
using Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace WebService.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class IoTController : ControllerBase
    {
        // GET: api/IoT
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/IoT/5
        [HttpGet("{sensor}", Name = "Get")]
        public string Get(string sensor)
        {
            if (sensor == "dht")
            {
                var DataTable = SQLOperation.ExecuteCommand(Startup.ConnectionString, "GetClimate", true);
                return JsonConvert.SerializeObject(DataTable);
            }
            else
            {
                var DataTable = SQLOperation.ExecuteCommand(Startup.ConnectionString, "GetAirQuality", true);
                return JsonConvert.SerializeObject(DataTable);
            }
        }

        [HttpGet("data/{sensor}", Name = "GetP")]
        public void PostG(string sensor)
        {
            //var abcd= JsonConvert.DeserializeObject(sensor);
            var data = (SensorData)JsonConvert.DeserializeObject(sensor, typeof(SensorData));
            if (data.From == "dht")
            {
                //if (Convert.ToUInt32(data.Temparature) > 30);
                var Parameters = new Dictionary<string, string> {
                    {"@temperature",data.Temparature },
                    {"@humadity",data.Humadity }
                };
                var DataTable = SQLOperation.ExecuteCommand(Startup.ConnectionString, "InsertTemp", false, Parameters);
                //DataTable.Merge(SQLOperation.ExecuteCommand(Startup.ConnectionString, "getlastfive", Parameters));
            }
            else
            {
                var Parameters = new Dictionary<string, string> {
                    {"@gasvalue",data.GasValue }
                };
                var DataTable = SQLOperation.ExecuteCommand(Startup.ConnectionString, "InsertAirQual", false, Parameters);
            }
        }


        // POST: api/IoT
        [HttpPost]
        public void Post([FromBody] string value)
        {
            var data = (SensorData)JsonConvert.DeserializeObject(value);
            var time = DateTime.Now;
            if (data.From == "dht")
            {
                //if (Convert.ToUInt32(data.Temparature) > 30);
                var Parameters = new Dictionary<string, string> {
                    {"@time", time.ToString() },
                    {"@temperature",data.Temparature },
                    {"@humadity",data.Humadity }
                };
                var DataTable = SQLOperation.ExecuteCommand(Startup.ConnectionString, "InsertTemp", false, Parameters);
                //DataTable.Merge(SQLOperation.ExecuteCommand(Startup.ConnectionString, "getlastfive", Parameters));
            }
            else
            {
                var Parameters = new Dictionary<string, string> {
                    {"@time", time.ToString() },
                    {"@gasvalue",data.GasValue }
                };
                var DataTable = SQLOperation.ExecuteCommand(Startup.ConnectionString, "InsertAirQual", false, Parameters);
            }
        }

        // PUT: api/IoT/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
