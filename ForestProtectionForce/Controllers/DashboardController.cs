using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ForestProtectionForce.utilities;
using System;
using Org.BouncyCastle.Crypto.Tls;
using System.Linq;
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
            int provinceOfSuperAdmins = LogicConvertions.getSuperAdminOfProvince(user);
            var userDetails = _context.UserDetails?.FirstOrDefault(x => x.Username == user.username) ?? new UserDetails();
            var baselineDatails = _context.Baseline;
            var dashboard = new Dashboard();
            var boxModalData = new List<BoxModel>();
            boxModalData.AddRange(new List<BoxModel> {
        new BoxModel { Name = "total", Count = baselineDatails?.Count(predicateLogicForStatus(userDetails,provinceOfSuperAdmins,"Total")) },
        new BoxModel { Name = "open", Count = baselineDatails?.Count(predicateLogicForStatus(userDetails,provinceOfSuperAdmins, "Open")) },
        new BoxModel { Name = "approved", Count = baselineDatails?.Count(predicateLogicForStatus(userDetails,provinceOfSuperAdmins, "Approved")) },
        new BoxModel { Name = "disposed", Count = baselineDatails?.Count(predicateLogicForStatus(userDetails,provinceOfSuperAdmins, "Disposed")) }
    });
            var chartDataBar = baselineDatails
                ?.Where(predicateLogicForData(userDetails, provinceOfSuperAdmins))
                .GroupBy(b => b.CrimeDate.Year)
                .Select(g => new Chart
                {
                    name = "bar",
                    xaxis = g.Key.ToString(),
                    yaxis = g.Count().ToString()
                });
            var chartDatapie = baselineDatails
                .Where(predicateLogicForData(userDetails, provinceOfSuperAdmins))
                .GroupBy(b => b.ForestDivisionName)
                .Select(g => new Chart { name = "pie", xaxis = g.Key.ToString(), yaxis = g.Count().ToString() });

            dashboard.boxModels = boxModalData;

            // Create a list of charts and add the chart data to it
            var chartsList = new List<Chart>();
            chartsList.AddRange(chartDataBar.ToList().DistinctBy(x => x.xaxis).AsQueryable());
            chartsList.AddRange(chartDatapie.ToList().DistinctBy(x => x.xaxis).AsQueryable());

            dashboard.charts = chartsList;
            dashboard.baseline = baselineDatails?.Where(predicateLogicForData(userDetails, provinceOfSuperAdmins)).Take(5).ToList();
            return Ok(dashboard);
        }

        [NonAction]
        public Func<Baseline, bool> predicateLogicForStatus(UserDetails userData, int provinceOfSuperAdmins, string status)
        {
            Func<Baseline, bool> condition;

                if (userData.UserType_Id == 2)
                {
                   return condition = status == "Total" ? x => x.ProvinceId == userData.ProvinceId : x => x.Status == status && x.ProvinceId == userData.ProvinceId;
                }
                else if (userData.UserType_Id == 3 || userData.UserType_Id == 4)
                {
                return condition = status == "Total" ? x => x.ForestDivisionId == userData.DistrictId : x => x.Status == status && x.ForestDivisionId == userData.DistrictId;
                }
                else if (provinceOfSuperAdmins == 1 || provinceOfSuperAdmins == 2) //jammu or kashmir
                  {
                return condition = status == "Total" ? x => x.ProvinceId == provinceOfSuperAdmins : x => x.Status == status && x.ProvinceId == provinceOfSuperAdmins;
              }
               
            else
                {
                return condition = status == "Total" ? x => true : x => x.Status == status;
            }
          
        }
        

        [NonAction]
        public Func<Baseline, bool> predicateLogicForData(UserDetails userData, int provinceOfSuperAdmins)
        {
            Func<Baseline, bool>? condition = null;

            if (userData.UserType_Id == 2)
            {
                return condition = x => x.ProvinceId == userData.ProvinceId;
            }
            else if (userData.UserType_Id == 3 || userData.UserType_Id == 4)
            {
                return condition = x => x.ForestDivisionId == userData.DistrictId;
            }
            else if (provinceOfSuperAdmins == 1 || provinceOfSuperAdmins == 2) //jammu or kashmir
            {
                return condition = x => x.ProvinceId ==provinceOfSuperAdmins;
            }
            else
            {
                return condition = x => true;
            }
           
        }
    }
}
