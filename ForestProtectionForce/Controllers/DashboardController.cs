using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ForestProtectionForce.utilities;
using System;
using Org.BouncyCastle.Crypto.Tls;
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
            string xUserData = HttpContext.Request.Headers["X-User-Data"];
            var user = LogicConvertions.getUserDetails(xUserData??"");
            bool isSuperAdmin = user.username == "superadmin";
            var userDetails = _context.UserDetails?.FirstOrDefault(x => x.Username == user.username) ?? new UserDetails();
            var dashboard = new Dashboard();
            var boxModalData = new List<BoxModel>();
            boxModalData.AddRange(new List<BoxModel> {
        new BoxModel { Name = "total", Count = _context.Baseline?.Count(predicateLogicForStatus(userDetails,isSuperAdmin,"Total")) },
        new BoxModel { Name = "open", Count = _context.Baseline?.Count(predicateLogicForStatus(userDetails,isSuperAdmin, "Open")) },
        new BoxModel { Name = "approved", Count = _context.Baseline?.Count(predicateLogicForStatus(userDetails,isSuperAdmin, "Approved")) },
        new BoxModel { Name = "disposed", Count = _context.Baseline?.Count(predicateLogicForStatus(userDetails,isSuperAdmin, "Rejected")) }
    });
            var chartDataBar = _context.Baseline?
                .Where(predicateLogicForData(userDetails, isSuperAdmin))
                .GroupBy(b => b.CrimeDate)
                .Select(g => new Chart {name ="bar", xaxis = Convert.ToDateTime(g.Key).Year.ToString(), yaxis = g.Sum(b => b.Id).ToString() });
            var chartDatapie = _context.Baseline?
                .Where(predicateLogicForData(userDetails, isSuperAdmin))
                .GroupBy(b => b.ForestDivisionName)
                .Select(g => new Chart { name = "pie", xaxis = g.Key.ToString(), yaxis = g.Sum(b => b.ForestDivisionId).ToString() });

            dashboard.boxModels = boxModalData;

            // Create a list of charts and add the chart data to it
            var chartsList = new List<Chart>();
            chartsList.AddRange(chartDataBar.ToList().DistinctBy(x => x.xaxis).AsQueryable());
            chartsList.AddRange(chartDatapie.ToList().DistinctBy(x => x.xaxis).AsQueryable());

            dashboard.charts = chartsList;
            dashboard.baseline = _context.Baseline?.Where(predicateLogicForData(userDetails, isSuperAdmin)).Take(5).ToList();
            return Ok(dashboard);
        }

        [NonAction]
        public Func<Baseline, bool> predicateLogicForStatus(UserDetails userData, bool isSuperAdmin, string status)
        {
            Func<Baseline, bool> condition;

                if (userData.UserType_Id == 2)
                {
                   return condition = status == "Total" ? x => x.ForestDivisionId == userData.ProvinceId : x => x.Status == status && x.ForestDivisionId == userData.ProvinceId;
                }
                else if (userData.UserType_Id == 3 || userData.UserType_Id == 4)
                {
                return condition = status == "Total" ? x => x.ForestDivisionId == userData.ProvinceId : x => x.Status == status && x.ForestDivisionId == userData.ProvinceId;
                }
                else
                {
                return condition = status == "Total" ? x => true : x => x.Status == status;
            }
          
        }
        

        [NonAction]
        public Func<Baseline, bool> predicateLogicForData(UserDetails userData, bool isSuperAdmin)
        {
            Func<Baseline, bool>? condition = null;
           
            if (userData.UserType_Id == 2)
            {
               return  condition = x =>  x.ForestDivisionId == userData.ProvinceId;
            }
            else if (userData.UserType_Id == 3 || userData.UserType_Id == 4)
            {
               return condition = x =>  x.ForestDivisionId == userData.ProvinceId;
            }
            else
            {
                return condition = x => true;
            }
           
        }
    }
}
