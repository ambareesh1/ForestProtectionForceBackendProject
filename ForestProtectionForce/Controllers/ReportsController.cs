﻿using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Utilities;
using System.Collections.Generic;
using System.Linq;


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

        }

        [HttpGet("GetGammaUnitFormBReport")]
        public async Task<ActionResult<IEnumerable<Gamma_unit_form_b>>> GetGammaUnitFormBReport(int districtId = 0, int month = 0)
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
                return await _context.gamma_unit_form_b.Where(x => x.Month == month && x.Year == DateTime.Now.Year).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetGammaUnitFormCReport")]
        public async Task<ActionResult<IEnumerable<ReportC>>> GetGammaUnitFormCReport(int districtId = 0, int month = 0)
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

        [HttpGet("GetGammaUnitAbstractFormReport")]
        public async Task<ActionResult<IEnumerable<AbstractMonth>>> GetGammaUnitAbstractFormReport(int districtId = 0, int month = 0)
        {
            try
            {

                List<AbstractMonth> abstractMonths = new List<AbstractMonth>
                {
                    new AbstractMonth { Header = "Total Seizures (F/K/D)", Field = "", Cft = "Cft/Qtls" },
                    new AbstractMonth { Header = "Willow", Field = "Willow", Cft = "No" },
                    new AbstractMonth { Header = "Vehicles", Field = "Seizure of vehicles (Nos )", Cft = "No" },
                    new AbstractMonth { Header = "Horses/Ponies", Field = "Seizure of Horses/Pones", Cft = "No" },
                    new AbstractMonth { Header = "Fire wood", Field = "Fire wood (in Qtis)", Cft = "Qtls" },
                    new AbstractMonth { Header = "Charcoal", Field = "Charcoal", Cft = "Bags" },
                    new AbstractMonth { Header = "Eviction of encroachment", Field = "Eviction of encroachments (Kanals)", Cft = "Kanals" },
                    new AbstractMonth { Header = "Dismentlling of Saw Mills", Field = "Dismentilling of illigal band saw mills", Cft = "No" },
                    new AbstractMonth { Header = "Poles of diff. Species", Field = "Poles Of Cheer, conifer others (Kiker) Nos", Cft = "No" },
                    new AbstractMonth { Header = "Forest Area Saved from Fire", Field = "Forest area saved from fire", Cft = "Kanals" },
                    new AbstractMonth {Header = "MFP", Field = "MFP Seized", Cft="Kgs"}

                };

                var result = abstractMonths
                              .Join(_context.Seizures_Form_A,
                                  abstractForm => abstractForm.Field,
                                  entry => entry.name,
                                  (abstractForm, entry) => new AbstractMonth
                                  {
                                      Header = abstractForm.Header,
                                      Field = entry.name,
                                      Cft = abstractForm.Cft,
                                      Prev = _context.Seizures_Form_A
                                          .Where(e => e.month == month && e.name == entry.name)
                                          .Sum(e => e.ob_joint + e.total_joint),
                                      Current = _context.Seizures_Form_A
                                          .Where(e => e.month == month + 1 && e.name == entry.name)
                                          .Sum(e => e.ob_joint + e.total_joint)
                                  }).ToList();

                decimal cumulativeSum = 0;
                foreach (var item in result.Distinct())
                {
                    cumulativeSum += item.Current ?? 0;
                    item.Cumulative = cumulativeSum;
                }

                return result.DistinctBy(x => x.Header).ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetMonthCFFormReport")]
        public async Task<ActionResult<IEnumerable<object>>> GetMonthCFFormReport()
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

            var districts = _context.District
               .Select(entry => new { entry.Id, entry.Name, entry.CircleId })
               .Distinct()
               .ToList();
            var circles = _context.Circle
               .Select(entry => new { entry.Id, entry.Name })
               .Distinct()
               .ToList();
            int currentMonth = DateTime.Now.Month;

            var result = new List<CircleData>();

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
                        decimal prevValue = _context.Seizures_Form_A
                            .Where(entry => entry.month == currentMonth - 1 && entry.name == item && entry.districtId == district.Id)
                            .Sum(entry => entry.ob_independent + entry.ob_joint);

                        decimal durValue = _context.Seizures_Form_A
                            .Where(entry => entry.month == currentMonth && entry.name == item && entry.districtId == district.Id)
                            .Sum(entry => entry.ob_independent + entry.ob_joint);

                        decimal totalValue = prevValue + durValue;

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
    }
    public class CircleData
    {
        public string Circle { get; set; }
        public List<DistrictData> Districts { get; set; }
    }
    public class DistrictData
    {
        public string District { get; set; }
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
}
