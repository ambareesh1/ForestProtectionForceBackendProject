using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForestProtectionForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ForestProtectionForceContext _context;

        public DashboardController(ForestProtectionForceContext context)
        {
            _context = context;
            
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dashboard>>> GetDashboardDetails()
        {
            var dashboard = new Dashboard();
            var boxModalData = new List<BoxModel>();
            boxModalData.AddRange(new List<BoxModel> {
        new BoxModel { Name = "total", Count = _context.Baseline?.Count() },
        new BoxModel { Name = "open", Count = _context.Baseline.Count(x => x.Status == "open") },
        new BoxModel { Name = "approved", Count = _context.Baseline.Count(x => x.Status == "approved") },
        new BoxModel { Name = "disposed", Count = _context.Baseline.Count(x => x.Status == "closed") }
    });
            var chartDataBar = _context.Baseline
                .GroupBy(b => b.CrimeDate)
                .Select(g => new Chart {name ="bar", xaxis = Convert.ToDateTime(g.Key).Year.ToString(), yaxis = g.Sum(b => b.CircleId).ToString() });
            var chartDatapie = _context.Baseline
                .GroupBy(b => b.CircleName)
                .Select(g => new Chart { name = "pie", xaxis = g.Key.ToString(), yaxis = g.Sum(b => b.CircleId).ToString() });

            dashboard.boxModels = boxModalData;

            // Create a list of charts and add the chart data to it
            var chartsList = new List<Chart>();
            chartsList.AddRange(chartDataBar.ToList().DistinctBy(x => x.xaxis).AsQueryable());
            chartsList.AddRange(chartDatapie.ToList().DistinctBy(x => x.xaxis).AsQueryable());

            dashboard.charts = chartsList;
            dashboard.baseline = _context.Baseline.Take(5).ToList();
            return Ok(dashboard);
        }



    }
}
