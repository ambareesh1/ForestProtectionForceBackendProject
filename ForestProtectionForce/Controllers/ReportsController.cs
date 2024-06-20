using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Utilities;
using System.Collections.Generic;
using System.Linq;
using static iText.IO.Util.IntHashtable;


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
        [HttpGet("GetFormAReport")]
        public async Task<ActionResult<IEnumerable<object>>> GetFormAReport(int districtId = 0, int month = 0, int year = 0, bool isJoint = true, int province = 0)
        {
            var itemNames = _context.Seizures_Form_A.Select(entry => entry.name).Distinct().ToList();
            var districts = districtId == 0  ? _context.District.Select(entry => entry.Id).Distinct().OrderByDescending(x => x).ToList() :
                _context.District.Where(x => x.Id == districtId).Select(entry => entry.Id).Distinct().OrderByDescending(x => x).ToList();
            if (province > 0 && districtId == 0)
            {
                var circles = _context.Circle.Where(x => x.ProvinceId == province);
                districts = _context.District
                    .Where(d => circles.Any(c => c.Id == d.CircleId))
                    .Select(entry => entry.Id)
                    .Distinct()
                    .OrderByDescending(x => x)
                    .ToList();
            }
            var result = new List<Dictionary<string, object>>();

            foreach (var item in itemNames)
            {
                var row = new Dictionary<string, object> { { "Item", item } };
                decimal prevValue = 0.00m;
                decimal durValue = 0.00m;
                foreach (var district in districts)
                {
                     prevValue = isJoint ?  _context.Seizures_Form_A
                        .Where(entry => (entry.month < month && entry.year == year) && entry.name == item && entry.districtId == district)
                        .Sum(entry =>  entry.during_month_joint) :
                         _context.Seizures_Form_A
                        .Where(entry => (entry.month < month && entry.year == year) && entry.name == item && entry.districtId == district)
                        .Sum(entry => entry.during_month_independent);

                     durValue = isJoint ? _context.Seizures_Form_A
                        .Where(entry => entry.month == month && entry.name == item && entry.districtId == district)
                        .Sum(entry =>  entry.during_month_joint) :
                        _context.Seizures_Form_A
                        .Where(entry => entry.month == month && entry.name == item && entry.districtId == district)
                        .Sum(entry => entry.during_month_independent );

                    //if(districtId != 0)
                    //{
                    //    prevValue = isJoint ? _context.Seizures_Form_A
                    //   .Where(entry => (entry.month <= month && entry.year == year) && entry.name == item && entry.districtId == districtId)
                    //   .Sum(entry => entry.ob_joint) :
                    //    _context.Seizures_Form_A
                    //   .Where(entry => (entry.month <= month && entry.year == year) && entry.name == item && entry.districtId == districtId)
                    //   .Sum(entry => entry.ob_independent);

                    //    durValue = isJoint ? _context.Seizures_Form_A
                    //       .Where(entry => entry.month == month && entry.name == item && entry.districtId == districtId)
                    //       .Sum(entry => entry.ob_joint) : _context.Seizures_Form_A
                    //       .Where(entry => entry.month == month && entry.name == item && entry.districtId == districtId)
                    //       .Sum(entry => entry.ob_independent);
                    //}

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

        }

        [HttpGet("GetGammaUnitFormBReport")]
        public async Task<ActionResult<IEnumerable<Gamma_unit_form_b>>> GetGammaUnitFormBReport(int districtId = 0, int month = 0, int year = 0, int province = 0)
        {
            try
            {
                if (_context.gamma_unit_form_b == null)
                {
                    return NotFound();
                }
                month = month == 0 ? DateTime.Now.Month : month;
                if (districtId != 0)
                {
                    return await _context.gamma_unit_form_b.Where(x => x.DistrictId == districtId && x.Month == month && x.Year == DateTime.Now.Year).ToListAsync();
                }
                if (province > 0 && districtId == 0)
                {
                    var circles = _context.Circle.Where(x => x.ProvinceId == province);
                    var districts = _context.District
                        .Where(d => circles.Any(c => c.Id == d.CircleId));
                        
                    return await _context.gamma_unit_form_b.Where(x => districts.Any(c => c.Id == x.DistrictId) && x.Month == month && x.Year == DateTime.Now.Year).ToListAsync();
                }
                return await _context.gamma_unit_form_b.Where(x => x.Month == month && x.Year == DateTime.Now.Year).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetGammaUnitFormCReport")]
        public async Task<ActionResult<IEnumerable<ReportC>>> GetGammaUnitFormCReport(int districtId = 0, int month = 0, int year = 0, int province = 0)
        {
            try
            {
                if (_context.status_of_cases_form_c == null)
                {
                    return NotFound();
                }
                month = month == 0 ? DateTime.Now.Month : month;
                if (districtId != 0)
                {
                    var districts = _context.District
                            .Where(d => d.Id ==districtId);

                    var result = await (
                         from status in _context.status_of_cases_form_c
                         join district in districts on status.DistrictId equals district.Id
                         where status.Month == month && status.Year == DateTime.Now.Year && status.Gamma_Unit == "Total"
                         select new ReportC
                         {
                             seizure_CasesMonth_Form_Cs = status, // Assuming you have a List<Seizure_CasesMonth_Form_C> property in the status_of_cases_form_c entity
                             GammaUnit = district.Name
                         })
                         .ToListAsync();
                    return result;
                }
                else if(province > 0 && districtId == 0)
                {
                   
                        var circles = _context.Circle.Where(x => x.ProvinceId == province);
                        var districts = _context.District
                           .Where(d => circles.Any(c => c.Id == d.CircleId));
                    
                    var result = await (
                                           from status in _context.status_of_cases_form_c
                                           join district in districts on status.DistrictId equals district.Id
                                           where status.Month == month && status.Year == DateTime.Now.Year && status.Gamma_Unit == "Total"
                                           select new ReportC
                                           {
                                               seizure_CasesMonth_Form_Cs = status, // Assuming you have a List<Seizure_CasesMonth_Form_C> property in the status_of_cases_form_c entity
                                               GammaUnit = district.Name
                                           })
                                           .ToListAsync();
                    return result;
                }
                else
                {
                    var result = await (
                                            from status in _context.status_of_cases_form_c
                                            join district in _context.District on status.DistrictId equals district.Id
                                            where status.Month == month && status.Year == DateTime.Now.Year && status.Gamma_Unit == "Total"
                                            select new ReportC
                                            {
                                                seizure_CasesMonth_Form_Cs = status, // Assuming you have a List<Seizure_CasesMonth_Form_C> property in the status_of_cases_form_c entity
                                                GammaUnit = district.Name
                                            })
                                            .ToListAsync();
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpPost("GetGammaUnitAbstractFormReport")]
        public async Task<ActionResult<IEnumerable<AbstractMonth>>> GetGammaUnitAbstractFormReport([FromBody] RequestModel requestModel)
        {
            try
            {
                List< AbstractMonth> result = new();
                List<AbstractMonth> abstractMonths = new List<AbstractMonth>();
                int currentMonth = DateTime.Now.Month;
                int previousYear = requestModel.Year;
                int financialYearStartMonth = 4; // April
                int financialYearEndMonth = 3;

                foreach (var item in requestModel.SelectedOptions)
                {
                    abstractMonths.Add(new AbstractMonth { Header = item, Field = item, Cft = "" });
                }

                if(requestModel.TypeOfSelection == "month")
                {
                     result = abstractMonths
                             .Join(_context.Seizures_Form_A,
                                 abstractForm => abstractForm.Field,
                                 entry => entry.name,
                                 (abstractForm, entry) => new AbstractMonth
                                 {
                                     Header = abstractForm.Header,
                                     Field = entry.name,
                                     Cft = abstractForm.Cft,
                                     Prev = GetAbstractOfSeizure(requestModel,entry).Prev,
                                     Current = GetAbstractOfSeizure(requestModel, entry).Current
                                 }).ToList();
                }
                else if(requestModel.TypeOfSelection == "calendaryear")
                {
                    result = abstractMonths
                             .Join(_context.Seizures_Form_A,
                                 abstractForm => abstractForm.Field,
                                 entry => entry.name,
                                 (abstractForm, entry) => new AbstractMonth
                                 {
                                     Header = abstractForm.Header,
                                     Field = entry.name,
                                     Cft = abstractForm.Cft,
                                     Prev = GetAbstractOfSeizure(requestModel, entry).Prev,
                                     Current = GetAbstractOfSeizure(requestModel, entry).Current
                                 }).ToList();
                }
                else
                {
                     result = abstractMonths
                             .Join(_context.Seizures_Form_A,
                                 abstractForm => abstractForm.Field,
                                 entry => entry.name,
                                 (abstractForm, entry) => new AbstractMonth
                                 {
                                     Header = abstractForm.Header,
                                     Field = entry.name,
                                     Cft = abstractForm.Cft,
                                     Prev = GetAbstractOfSeizure(requestModel, entry).Prev,

                                     Current = GetAbstractOfSeizure(requestModel, entry).Current
                                 }).ToList();
                }

               

                decimal cumulativeSum = 0;
                foreach (var item in result.Distinct())
                {
                    cumulativeSum = (decimal)(item.Current + item.Prev);
                    item.Cumulative = cumulativeSum;
                }

                return result.DistinctBy(x => x.Header).ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetItemNamesFromSeizureA")]

        public async Task<ActionResult<List<string>>> GetItemsFromSeizureA()
        {
            List<string?> result = await _context.Seizures_Form_A?.Select(x => x.name).Distinct().ToListAsync();
            return result;
        }


        [HttpGet("GetMonthCFFormReport")]
        public ActionResult<IEnumerable<object>> GetMonthCFFormReport(int districtId = 0, int month = 0, int year = 0, bool isFinancialYearSelected = false, string typeOfSelection = "month", int province = 0)
        {
            List<AbstractMonth> abstractMonths = new List<AbstractMonth>
            {
                new AbstractMonth { Header = "Timber", Field = "Total Conifer Timber", Cft = "Cft/Qtls" },
                new AbstractMonth { Header = "Fire wood (in Qtis)", Field = "Fire wood (in Qtis)", Cft = "No" },
                new AbstractMonth { Header = "Seizure of vehicles (Nos )", Field = "Seizure of vehicles (Nos )", Cft = "No" },
                new AbstractMonth { Header = "Seizure of Horses/Pones", Field = "Seizure of Horses/Pones", Cft = "No" },
                new AbstractMonth { Header = "MFP", Field = "MFP Seized", Cft = "Kgs" }
            };

            var itemNames = abstractMonths.Select(x => x.Field).ToList();

            var districts = (districtId == 0)
     ? _context.District
         .Select(entry => new { entry.Id, entry.Name, entry.CircleId })
         .Distinct()
         .ToList()
     : _context.District
         .Where(x => x.Id == districtId)
         .Select(entry => new { entry.Id, entry.Name, entry.CircleId })
         .Distinct()
         .ToList();

            var circles = (districtId == 0)
                ? _context.Circle
                    .Select(entry => new { entry.Id, entry.Name })
                    .Distinct()
                    .ToList()
                : _context.Circle
                    .Where(x => x.Id == districts.FirstOrDefault().CircleId)
                    .Select(entry => new { entry.Id, entry.Name })
                    .Distinct()
                    .ToList();

            if (province > 0 && districtId == 0)
            {
                circles = _context.Circle
                    .Where(x => x.ProvinceId == province)
                    .Select(entry => new { entry.Id, entry.Name })
                    .Distinct()
                    .ToList();

                // Load circle IDs for filtering districts
                var circleIds = circles.Select(c => c.Id).ToList();

                districts = _context.District
                    .Where(d => circleIds.Contains(d.CircleId))
                    .Select(entry => new { entry.Id, entry.Name, entry.CircleId })
                    .Distinct()
                    .ToList();
            }


            var result = new List<CircleData>();
            int currentMonth = DateTime.Now.Month;
            int previousYear = year;
            int currentYear = year + 1;
            int financialYearStartMonth = 4; // April
            int financialYearEndMonth = 3;
            int prePreviousYear = previousYear - 1;
            int preCurrentYear = currentYear - 1;
            foreach (var circle in circles)
            {
                var circleName = circle.Name ?? "";
                var circleData = new CircleData { Circle = circleName, Districts = new List<DistrictData>() };

                foreach (var district in districts.Where(x => x.CircleId == circle.Id))
                {
                    var districtName = district.Name ?? "";
                    var districtData = new DistrictData { District = districtName, Items = new Dictionary<string, decimal>() };

                    foreach (var item in itemNames)
                    {
                        decimal prevValue = 0.0m;
                        decimal durValue = 0.0m;
                        decimal totalValue = 0.0m;

                        if (typeOfSelection == "financialyear")
                        {
                             prevValue = _context.Seizures_Form_A
                            .Where(entry => entry.name == item && entry.districtId == district.Id &&
                                    ((entry.year == previousYear && entry.month >= financialYearStartMonth) || // From April of the previous year
                                    (entry.year == currentYear && entry.month <= financialYearEndMonth)) // Up to the specified month of the current year
                                )
                            .Sum(entry =>  entry.during_month_joint);

                             durValue = _context.Seizures_Form_A
                                .Where(entry => entry.name == item &&  entry.districtId == district.Id &&
                                        (entry.year == previousYear && entry.month >= financialYearStartMonth) || // From April of the previous year
                                        (entry.year == currentYear && entry.month == financialYearEndMonth) // Up to the specified month of the current year
                                    )
                                .Sum(entry =>  entry.during_month_independent);
                        }
                        else if (typeOfSelection == "calendaryear")
                        {
                            prevValue = _context.Seizures_Form_A
                           .Where(entry =>  entry.year < year && entry.name == item && entry.districtId == district.Id)
                           .Sum(entry => entry.during_month_joint);

                            durValue = _context.Seizures_Form_A
                               .Where(entry =>  entry.year == year && entry.name == item && entry.districtId == district.Id)
                               .Sum(entry => entry.during_month_independent);
                        }
                        else
                        {
                             prevValue = _context.Seizures_Form_A
                            .Where(entry => entry.month == month && entry.year == year && entry.name == item && entry.districtId == district.Id)
                            .Sum(entry =>  entry.during_month_joint);

                             durValue = _context.Seizures_Form_A
                                .Where(entry => entry.month == month && entry.year == year && entry.name == item && entry.districtId == district.Id)
                                .Sum(entry =>  entry.during_month_independent);
                        }
                        

                        districtData.Items.Add(item, prevValue);
                        districtData.Items.Add(item + "_Current", durValue);
                        districtData.Items.Add(item + "_Cumulative", totalValue);
                    }

                    circleData.Districts.Add(districtData);
                }

                result.Add(circleData);
            }

            // ... Your existing code to retrieve and process the data ...

            // Calculate district-wise subtotals for each item and circle-wise totals
            var circleTotals = new Dictionary<string, ItemTotals>();
            var grandTotal = new ItemTotals();

            foreach (var circleData in result)
            {
                var circleName = circleData.Circle;
                var circleTotal = new ItemTotals();

                foreach (var districtData in circleData.Districts)
                {
                    foreach (var item in itemNames)
                    {
                        decimal prevValue = districtData.Items[item];
                        decimal durValue = districtData.Items[item + "_Current"];
                        decimal totalValue = prevValue + durValue;

                        districtData.Items[item + "_Cumulative"] = totalValue;

                        circleTotal.Previous += prevValue;
                        circleTotal.Current += durValue;
                        circleTotal.Cumulative += totalValue;

                        grandTotal.Previous += prevValue;
                        grandTotal.Current += durValue;
                        grandTotal.Cumulative += totalValue;
                    }
                }

                // Add the circle-wise totals to the result
                circleTotals[circleName] = circleTotal;
            }

            // Add the subtotals to each circle and district
            int subCount = 1;
            foreach (var circleData in result)
            {
                var circleName = circleData.Circle;
                var circleTotal = circleTotals[circleName];

                var districtSubTotal = new Dictionary<string, decimal>();

                foreach(var district in circleData.Districts)
                {
                    foreach (var item in itemNames)
                    {
                        var subTotalPrevious = circleData.Districts.Sum(district => district.Items[item]);
                        districtSubTotal[item] = subTotalPrevious;
                        var subTotalCurrent = circleData.Districts.Sum(district => district.Items[item + "_Current"]);
                        districtSubTotal[item + "_Current"] = subTotalCurrent;
                        var subTotalCumulative = circleData.Districts.Sum(district => district.Items[item + "_Cumulative"]);
                        districtSubTotal[item + "_Cumulative"] = subTotalCumulative;
                    }
                }

               

                // Add the subtotal of all items for the circle
                circleData.Districts.Add(new DistrictData
                {
                    District = "Sub Total (" + subCount + ")",
                    Items = districtSubTotal
                });
                ++subCount;
            }
            // Calculate grand totals for all circles
            var grandTotalOfAll = new Dictionary<string, decimal>();

            foreach (var item in itemNames)
            {
                decimal grandTotalPrevious = result.Sum(circleData => circleData.Districts.Where(x=>x.District.Contains("Sub Total")).Sum(district => district.Items[item]));
                decimal grandTotalCurrent = result.Sum(circleData => circleData.Districts.Where(x => x.District.Contains("Sub Total")).Sum(district => district.Items[item + "_Current"]));
                decimal grandTotalCumulative = result.Sum(circleData => circleData.Districts.Where(x => x.District.Contains("Sub Total")).Sum(district => district.Items[item + "_Cumulative"]));

                grandTotalOfAll[item] = grandTotalPrevious;
                grandTotalOfAll[item + "_Current"] = grandTotalCurrent;
                grandTotalOfAll[item + "_Cumulative"] = grandTotalCumulative;
            }

            // Add the grand total row to the result
            result.Add(new CircleData
            {
                Circle = "GRAND TOTAL",
                Districts = new List<DistrictData>
                {
                    new DistrictData
                    {
                        District = "GRAND TOTAL",
                        Items = grandTotalOfAll
                    }
                }
            });
           

            return result;
        }
        [NonAction]
        public AbstractMonth GetAbstractOfSeizure(RequestModel requestModel, Seizures_Form_A entry)
        {
           
            AbstractMonth abstractMonth = new();
            int currentMonth = DateTime.Now.Month;
            int previousYear = requestModel.Year;
            int currentYear = requestModel.Year + 1;
            int financialYearStartMonth = 4; // April
            int financialYearEndMonth = 3;
            int prePreviousYear = previousYear - 1;
            int preCurrentYear = currentYear - 1;
            if(requestModel.TypeOfReport == "Joint")
            {
                if (requestModel.TypeOfSelection == "month")
                {
                    if (requestModel.Province > 0 && requestModel.DistrictId == 0)
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e => e.month < requestModel.Month && e.year == requestModel.Year && e.name == entry.name && e.provinceId == requestModel.Province)
                                                 .Sum(e => e.during_month_joint);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e => e.month == requestModel.Month && e.year == requestModel.Year && e.name == entry.name && e.provinceId == requestModel.Province)
                                                 .Sum(e => e.during_month_joint);
                    }
                    else
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e => e.month < requestModel.Month && e.year == requestModel.Year && e.name == entry.name )
                                                 .Sum(e => e.during_month_joint);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e => e.month == requestModel.Month && e.year == requestModel.Year && e.name == entry.name )
                                                 .Sum(e => e.during_month_joint);
                    }

                    if (requestModel.DistrictId != 0)
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e => e.month < requestModel.Month && e.year == requestModel.Year && e.name == entry.name && e.districtId == requestModel.DistrictId)
                                                 .Sum(e => e.during_month_joint);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e => e.month == requestModel.Month && e.year == requestModel.Year && e.name == entry.name && e.districtId == requestModel.DistrictId)
                                                 .Sum(e => e.during_month_joint);
                    }
                }
                else if (requestModel.TypeOfSelection == "calendaryear")
                {
                    if (requestModel.Province > 0 && requestModel.DistrictId == 0)
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                             .Where(e => e.year < requestModel.Year && e.name == entry.name && e.provinceId == requestModel.Province)
                             .Sum(e => e.during_month_joint);
                        abstractMonth.Current = _context.Seizures_Form_A
                              .Where(e => e.year == requestModel.Year && e.name == entry.name && e.provinceId == requestModel.Province)
                              .Sum(e => e.during_month_joint);
                    }
                    else
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e => e.year < requestModel.Year && e.name == entry.name )
                                                 .Sum(e => e.during_month_joint);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e => e.year == requestModel.Year && e.name == entry.name )
                                                 .Sum(e => e.during_month_joint);
                    }

                    if (requestModel.DistrictId != 0)
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e => e.year < requestModel.Year && e.name == entry.name && e.districtId == requestModel.DistrictId)
                                                 .Sum(e => e.during_month_joint);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e => e.year == requestModel.Year && e.name == entry.name && e.districtId == requestModel.DistrictId)
                                                 .Sum(e => e.during_month_joint);
                    }
                }
                else
                {
                    if (requestModel.Province > 0 && requestModel.DistrictId == 0)
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                             .Where(e =>  e.name == entry.name && ((e.year == previousYear && e.month >= financialYearStartMonth) || // From April of the previous year
                                        (e.year == currentYear && e.month <= financialYearEndMonth)) && e.provinceId == requestModel.Province)
                             .Sum(e => e.during_month_joint);
                        abstractMonth.Current = _context.Seizures_Form_A
                              .Where(e =>  e.name == entry.name && ((e.year == currentYear && e.month >= financialYearStartMonth) || // From April of the previous year
                                        (e.year == currentYear && e.month <= financialYearEndMonth)) && e.provinceId == requestModel.Province)
                              .Sum(e => e.during_month_joint);
                    }
                    else
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e =>  e.name == entry.name  && ((e.year == previousYear && e.month >= financialYearStartMonth) || // From April of the previous year
                                        (e.year == currentYear && e.month <= financialYearEndMonth)))
                                                 .Sum(e => e.during_month_joint);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e =>  e.name == entry.name && ((e.year == currentYear && e.month >= financialYearStartMonth) || // From April of the previous year
                                        (e.year == currentYear && e.month <= financialYearEndMonth)))
                                                 .Sum(e => e.during_month_joint);
                    }

                    if (requestModel.DistrictId != 0)
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e =>  e.name == entry.name && e.districtId == requestModel.DistrictId && ((e.year == previousYear && e.month >= financialYearStartMonth) || // From April of the previous year
                                        (e.year == currentYear && e.month <= financialYearEndMonth)))
                                                 .Sum(e => e.during_month_joint);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e =>  e.name == entry.name && e.districtId == requestModel.DistrictId && ((e.year == currentYear && e.month >= financialYearStartMonth) || // From April of the previous year
                                        (e.year == currentYear && e.month <= financialYearEndMonth)))
                                                 .Sum(e => e.during_month_joint);
                    }

                }

            }
            else
            {
                if (requestModel.TypeOfSelection == "month")
                {
                    if (requestModel.Province > 0 && requestModel.DistrictId == 0)
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e => e.month < requestModel.Month && e.year == requestModel.Year && e.name == entry.name && e.provinceId == requestModel.Province)
                                                 .Sum(e => e.during_month_independent);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e => e.month == requestModel.Month && e.year == requestModel.Year && e.name == entry.name && e.provinceId == requestModel.Province)
                                                 .Sum(e => e.during_month_independent);
                    }
                    else
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e => e.month < requestModel.Month && e.year == requestModel.Year && e.name == entry.name )
                                                 .Sum(e => e.during_month_independent);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e => e.month == requestModel.Month && e.year == requestModel.Year && e.name == entry.name)
                                                 .Sum(e => e.during_month_independent);
                    }

                    if (requestModel.DistrictId != 0)
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e => e.month < requestModel.Month && e.year == requestModel.Year && e.name == entry.name && e.districtId == requestModel.DistrictId)
                                                 .Sum(e => e.during_month_independent);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e => e.month == requestModel.Month && e.year == requestModel.Year && e.name == entry.name && e.districtId == requestModel.DistrictId)
                                                 .Sum(e => e.during_month_independent);
                    }
                }
                else if (requestModel.TypeOfSelection == "calendaryear")
                {
                    if (requestModel.Province > 0 && requestModel.DistrictId == 0)
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                             .Where(e => e.year < requestModel.Year && e.name == entry.name && e.provinceId == requestModel.Province)
                             .Sum(e => e.during_month_independent);
                        abstractMonth.Current = _context.Seizures_Form_A
                              .Where(e => e.year == requestModel.Year && e.name == entry.name && e.provinceId == requestModel.Province)
                              .Sum(e => e.during_month_independent);
                    }
                    else
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e => e.year < requestModel.Year && e.name == entry.name )
                                                 .Sum(e => e.during_month_independent);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e => e.year == requestModel.Year && e.name == entry.name )
                                                 .Sum(e => e.during_month_independent);
                    }

                    if (requestModel.DistrictId != 0)
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e => e.year < requestModel.Year && e.name == entry.name && e.districtId == requestModel.DistrictId)
                                                 .Sum(e => e.during_month_independent);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e => e.year == requestModel.Year && e.name == entry.name && e.districtId == requestModel.DistrictId)
                                                 .Sum(e => e.during_month_independent);
                    }
                }
                else
                {
                    if (requestModel.Province > 0 && requestModel.DistrictId == 0)
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                             .Where(e =>  e.name == entry.name && ((e.year == previousYear && e.month >= financialYearStartMonth) || // From April of the previous year
                                        (e.year == currentYear && e.month <= financialYearEndMonth)) && e.provinceId == requestModel.Province)
                             .Sum(e => e.during_month_independent);
                        abstractMonth.Current = _context.Seizures_Form_A
                              .Where(e =>  e.name == entry.name && ((e.year == currentYear && e.month >= financialYearStartMonth) || // From April of the previous year
                                        (e.year == currentYear && e.month <= financialYearEndMonth)) && e.provinceId == requestModel.Province)
                              .Sum(e => e.during_month_independent);
                    }
                    else
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e =>  e.name == entry.name  && ((e.year == previousYear && e.month >= financialYearStartMonth) || // From April of the previous year
                                        (e.year == currentYear && e.month <= financialYearEndMonth)))
                                                 .Sum(e => e.during_month_independent);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e =>  e.name == entry.name  && ((e.year == currentYear && e.month >= financialYearStartMonth) || // From April of the previous year
                                        (e.year == currentYear && e.month <= financialYearEndMonth)))
                                                 .Sum(e => e.during_month_independent);
                    }

                    if (requestModel.DistrictId != 0)
                    {
                        abstractMonth.Prev = _context.Seizures_Form_A
                                                 .Where(e =>  e.name == entry.name && e.districtId == requestModel.DistrictId && ((e.year == previousYear && e.month >= financialYearStartMonth) || // From April of the previous year
                                        (e.year == preCurrentYear && e.month <= financialYearEndMonth)))
                                                 .Sum(e => e.during_month_independent);
                        abstractMonth.Current = _context.Seizures_Form_A
                                                 .Where(e =>  e.name == entry.name && e.districtId == requestModel.DistrictId && ((e.year == currentYear && e.month >= financialYearStartMonth) || // From April of the previous year
                                        (e.year == preCurrentYear && e.month <= financialYearEndMonth)))
                                                 .Sum(e => e.during_month_independent);
                    }

                }

            }


            return abstractMonth;
        }
    }
    public class CircleData
    {
        public string? Circle { get; set; }
        public List<DistrictData> Districts { get; set; }
    }
    public class DistrictData
    {
        public string? District { get; set; }
        public Dictionary<string, decimal> Items { get; set; }
    }
    public enum DataType
    {
        Previous,
        Current,
        Cumulative
    }

    public class ItemTotals
    {
        public decimal Previous { get; set; }
        public decimal Current { get; set; }
        public decimal Cumulative { get; set; }
    }

    public class RequestModel
    {
        public int DistrictId { get; set; }
        public int Province { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public List<string>? SelectedOptions { get; set; }
        public string? TypeOfSelection { get; set; } 
        public string? TypeOfReport { get; set; }
    }
}
