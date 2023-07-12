using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForestProtectionForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {  
        private readonly ForestProtectionForceContext _context;
        private readonly IWebHostEnvironment _env;
        public ReportsController(ForestProtectionForceContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/<ReportsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> Get()
        {

            var itemNames = _context.Seizures_Form_A.Select(entry => entry.name).Distinct().ToList();
            var districts = _context.District.Select(entry => entry.Id).Distinct().ToList();

            int currentMonth = DateTime.Now.Month;

            var result = new List<Dictionary<string, object>>();

            // Add headers for Item, Prev, Dur, Total for each district
            //var header = new Dictionary<string, object> { { "Item", "Item" } };
          //  result.Add(header);

    //        foreach (var district in districts)
    //        {
    //            var districtHeader = new Dictionary<string, object>
    //{
    //    { "District", $"District {district}" },
    //    { "Pre", "Pre" },
    //    { "Dur", "Dur" },
    //    { "Total", "Total" }
    //};
    //            result.Add(districtHeader);
    //        }

            foreach (var item in itemNames)
            {
                var row = new Dictionary<string, object> { { "Item", item } };

                foreach (var district in districts)
                {
                    decimal prevValue = _context.Seizures_Form_A
                        .Where(entry => entry.month == currentMonth - 1 && entry.name == item && entry.districtId == district)
                        .Sum(entry => entry.ob_independent + entry.ob_joint);

                    decimal durValue = _context.Seizures_Form_A
                        .Where(entry => entry.month == currentMonth && entry.name == item && entry.districtId == district)
                        .Sum(entry => entry.ob_independent + entry.ob_joint);

                    decimal totalValue = prevValue + durValue;

                    var districtData = new Dictionary<string, object>
        {

            { "Pre", prevValue },
            { "Dur", durValue },
            { "Total", totalValue }
        };
                    row.Add($"District {district}", districtData);
                }

                result.Add(row);
            }

           


            return result;


            //int currentMonth = DateTime.Now.Month;

            //var itemNames = _context.Seizures_Form_A.Select(entry => entry.name).Distinct().ToList();
            ////var districts = _context.District.Select(entry => entry.Name).Distinct().ToList();


            //int currentMonth = DateTime.Now.Month;

            //var districts = _context.District.Select(entry => entry.Id).Distinct().ToList();

            //var result = new List<FormAReportModel>();

            //foreach (var district in districts)
            //{
            //    var items = _context.Seizures_Form_A
            //        .Where(entry => entry.month == currentMonth && entry.districtId == district)
            //        .Select(entry => entry.name)
            //        .Distinct()
            //        .ToList();

            //    foreach (var item in items)
            //    {
            //        decimal prevValue = _context.Seizures_Form_A
            //            .Where(entry => entry.month == currentMonth - 1 && entry.name == item && entry.districtId == district)
            //            .Sum(entry => entry.ob_independent + entry.ob_joint);

            //        decimal durValue = _context.Seizures_Form_A
            //            .Where(entry => entry.month == currentMonth && entry.name == item && entry.districtId == district)
            //            .Sum(entry => entry.ob_independent + entry.ob_joint);

            //        decimal totalValue = prevValue + durValue;

            //        var reportModel = new FormAReportModel
            //        {
            //            Item = item,
            //            Prev = prevValue,
            //            Dur = durValue,
            //            Total = totalValue,
            //            District = district
            //        };

            //        result.Add(reportModel);
            //    }
            //}

            //return result;


        }

        // GET api/<ReportsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReportsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ReportsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReportsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
