﻿using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Mysqlx;
using System;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Xml.Linq;
using static Mysqlx.Notice.Warning.Types;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForestProtectionForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeizuresController : ControllerBase
    {

        private readonly ForestProtectionForceContext _context;

        public SeizuresController(ForestProtectionForceContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seizures_Form_A>>> GetFormA()
        {
            try
            {
                if (_context.Seizures_Form_A == null)
                {
                    return NotFound();
                }
                return await _context.Seizures_Form_A.ToListAsync();
            } catch (Exception ex) {
                return null;
            }

        }
        [HttpGet("GetFormAWithDistrict")]
        public async Task<ActionResult<IEnumerable<Seizures_Form_A>>> GetFormAOnDistrict(int districtId)
        {
            try
            {
                if (_context.Seizures_Form_A == null)
                {
                    return NotFound();
                }
                return await _context.Seizures_Form_A.Where(x=>x.districtId == districtId).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFormA(int id, Seizures_Form_A seizures_Form_A)
        {
            if (id != seizures_Form_A.id)
            {
                return BadRequest();
            }

            _context.Entry(seizures_Form_A).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormAExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("PostFormA")]
        public async Task<ActionResult<Seizures_Form_A>> PostFormA(Seizures_Form_A seizures_Form_A)
        {
            if (_context.Seizures_Form_A == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.FormA'  is null.");
            }

            List<Seizures_Form_A> seizures_Form_As = getSeizureReportA(seizures_Form_A.provinceId, seizures_Form_A.districtId);
            _context.Seizures_Form_A.AddRange(seizures_Form_As);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Seizures_Form_A", new { id = 1 }, seizures_Form_As[0]);
        }



        // DELETE api/<Seizures>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [NonAction]
        public Seizures_Form_A createFormAIfNewMonth()
        {
            //var formA = new List<Seizures_Form_A>(new Seizures_Form_A)
            //);



            return null;// new Seizures_Form_A();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Seizures_Form_A>> CheckSeizureAlreadyExistForDistrictAndMonth(int id)
        {
            Seizures_Form_A? formA = await _context.Seizures_Form_A.FirstOrDefaultAsync(x => x.districtId == id && x.month == DateTime.Now.Month && x.year == DateTime.Now.Year);

            if (formA == null)
            {
                return null;
            }

            return formA;
        }

        [NonAction]
        public bool checkTheCurrentMonthAndYear()
        {
            var result = _context.Seizures_Form_A?.LastOrDefault();

            return result?.month == DateTime.Now.Month && result.year == DateTime.Now.Year;

        }

        private bool FormAExists(int id)
        {
            return (_context.Seizures_Form_A?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [NonAction]
        public List<Seizures_Form_A> getSeizureReportA(int provinceId, int districtId)
        {

            List<NamesAndSeriaalNo> formA_names = new List<NamesAndSeriaalNo>
            {
                        new NamesAndSeriaalNo { SerialNo = "1", Name = "Deodar" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Kail Fir" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Chir" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Spruce" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "others" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Total Conifer Timber" },
                        new NamesAndSeriaalNo { SerialNo = "2", Name = "Broad leaved timber" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Poplar" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Walnut" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Willow Sheesham" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Toon" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Mango" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Khair" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Simbal" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Oak" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Others" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Total Broad Leaved Timber" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "(cfts)" },
                        new NamesAndSeriaalNo { SerialNo = "3", Name = "Poles Of Cheer, conifer others (Kiker) Nos" },
                        new NamesAndSeriaalNo { SerialNo = "4", Name = "Willow Clefts" },
                        new NamesAndSeriaalNo { SerialNo = "5", Name = "Seizure of vehicles (Nos )" },
                        new NamesAndSeriaalNo { SerialNo = "6", Name= "Seizure of Horses/Pones"},
                        new NamesAndSeriaalNo { SerialNo = "7", Name = "Fire wood (in Qtis)" },
                        new NamesAndSeriaalNo { SerialNo = "8", Name = "Charcoal" },
                        new NamesAndSeriaalNo { SerialNo = "9", Name = "Resin" },
                        new NamesAndSeriaalNo { SerialNo = "10", Name = "Plywood veneers" },
                        new NamesAndSeriaalNo { SerialNo = "11", Name = "Eviction of encroachments (Kanals)" },
                        new NamesAndSeriaalNo { SerialNo = "12", Name = "Forest area saved from fire" },
                        new NamesAndSeriaalNo { SerialNo = "13", Name = "Dismentilling of illigal band saw mills" },
                        new NamesAndSeriaalNo { SerialNo = "14", Name = "Destroying of Bungh cultivation(Kanals)" },
                        new NamesAndSeriaalNo { SerialNo = "15", Name = "Wild life protected from poachers" },
                        new NamesAndSeriaalNo { SerialNo = "16", Name = "MFP Seized" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Turpatheri /Doop/Kuth(kgs)" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Walnut Bark"},
                        new NamesAndSeriaalNo {SerialNo = "", Name = "Pine Cones"},
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Guchies (in Kgs) Green" },
                        new NamesAndSeriaalNo { SerialNo = "17", Name = "Planner with motor" },
                        new NamesAndSeriaalNo { SerialNo = "18", Name = "Wooden boat" },
                        new NamesAndSeriaalNo {SerialNo = "19", Name="Hand driven cart/Horse cart"}
                     

            };

            List<Seizures_Form_A> seizures_Form_A = new List<Seizures_Form_A>();

            foreach (var item in formA_names)
            {
                seizures_Form_A.Add(new Seizures_Form_A
                {
                    id = 0,
                    serialNo = item.SerialNo,
                    name = item.Name,
                    ob_independent = "",
                    during_month_independent = "",
                    total_independent = "",
                    ob_joint = "",
                    during_month_joint = "",
                    total_joint = "",
                    provinceId = provinceId,
                    districtId = districtId,
                    DateOfInsertion = DateTime.Now,
                    IsActive = true,
                    LastUpdatedOn = DateTime.Now,
                    month = DateTime.Now.Month,
                    year = DateTime.Now.Year
                });
            }
            return seizures_Form_A;

        }

        [NonAction]
        public Gamma_unit_form_b getSeizureGammaUnitB(int provinceId, int districtId)
        {
            Gamma_unit_form_b formB = new Gamma_unit_form_b
            {
                DistrictId = districtId,
                ProvinceId = provinceId,
                Complaints_Verified = 0,
                Complaints_Received = 0,
                DateOfInsertion = DateTime.Now,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                Gamma_Unit = "K-06, Natnussa, Kupwara",
                JOP_Reports_Received = 0,
                Jungle_Gashts_Performed = 0,
                Nakas_Laid = 0,
                Patrollings_Performed = 0,
                Requisitions_Attended = 0,
                Requisitions_Made = 0,
                SerialNo =1
            } ;
            
            return formB;

        }

        [NonAction]
        public List<Seizure_CasesMonth_Form_C> getSeizureCasesOfMonthC(int? provinceId, int? districtId)
        {
            List<Seizure_CasesMonth_Form_C> formC = new List<Seizure_CasesMonth_Form_C>{
                new Seizure_CasesMonth_Form_C
            {
                DistrictId = districtId,
                Sno = 1,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                Authorized_Officer_FD = 0,
                Balance = 0,
                Cases_Registered_Month = 0,
                Court= 0,
                Disposed_Cases_Month = 0,
                Gamma_Unit = "Individual",
                Id = 0,
                Opening_Balance = 0,
                Pccf   = 0,
                Total = 0,
                Under_Investigation = 0,
                High_Court = 0,
                Others = 0,
                Session_Court = 0

            },new Seizure_CasesMonth_Form_C
            {
                Sno = 2,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                Authorized_Officer_FD = 0,
                Balance = 0,
                Cases_Registered_Month = 0,
                Court= 0,
                Disposed_Cases_Month = 0,
                Gamma_Unit = "Joint",
                Id = 0,
                Opening_Balance = 0,
                Pccf   = 0,
                Total = 0,
                Under_Investigation = 0,
                High_Court = 0,
                Others = 0,
                Session_Court = 0
            },new Seizure_CasesMonth_Form_C
            {
                Sno = 3,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                Authorized_Officer_FD = 0,
                Balance = 0,
                Cases_Registered_Month = 0,
                Court= 0,
                Disposed_Cases_Month = 0,
                Gamma_Unit = "Total",
                Id = 0,
                Opening_Balance = 0,
                Pccf   = 0,
                Total = 0,
                Under_Investigation = 0,
                High_Court = 0,
                Others = 0,
                Session_Court = 0

            }
            };

            return formC;

        }
       

        // Form B Gamma Unit

        [HttpGet("GammaUnitB")]
        public async Task<ActionResult<IEnumerable<Gamma_unit_form_b>>> GetSeizureGammUnitFormB()
        {
            try
            {
                if (_context.Seizures_Form_A == null)
                {
                    return NotFound();
                }
                return await _context.gamma_unit_form_b.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPut("UpdateGammaUnitFromB{id}")]
        public async Task<IActionResult> PutFormB(int id, Gamma_unit_form_b seizures_GammaUnit_Form_B)
        {
            if (id != seizures_GammaUnit_Form_B.Id)
            {
                return BadRequest();
            }

            _context.Entry(seizures_GammaUnit_Form_B).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormAExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpGet("GetGammaUnitFormBWithDistrict")]
        public async Task<ActionResult<IEnumerable<Gamma_unit_form_b>>> GetFormBOnDistrict(int districtId)
        {
            try
            {
                if (_context.gamma_unit_form_b == null)
                {
                    return NotFound();
                }
                return await _context.gamma_unit_form_b.Where(x => x.DistrictId == districtId).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [HttpPost("PostGammaUnitFormB")]
        public async Task<ActionResult<Gamma_unit_form_b>> PostGammaUnitFormB(Gamma_unit_form_b seizures_GammaUnit_Form_B)
        {
            if (_context.gamma_unit_form_b == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.PostGammaUnitFormB'  is null.");
            }

            Gamma_unit_form_b formB = getSeizureGammaUnitB(seizures_GammaUnit_Form_B.ProvinceId, seizures_GammaUnit_Form_B.DistrictId);
            _context.gamma_unit_form_b.Add(formB);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Seizures_Form_B", new { id = 1 }, formB);
        }
        [HttpGet("CheckSeizureBlreadyExistForDistrictAndMonth")]
        public async Task<ActionResult<Gamma_unit_form_b>> CheckSeizureBlreadyExistForDistrictAndMonth(int id)
        {
            Gamma_unit_form_b? formA = await _context.gamma_unit_form_b.FirstOrDefaultAsync(x => x.DistrictId == id && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);

            if (formA == null)
            {
                return null;
            }

            return formA;
        }


        // FORM C - CASES OF MONTH

        [HttpGet("CheckSeizureClreadyExistForDistrictAndMonth")]
        public async Task<ActionResult<IEnumerable<Seizure_CasesMonth_Form_C>>> CheckSeizureClreadyExistForDistrictAndMonth(int id)
        {
            var formC = await _context.status_of_cases_form_c.Where(x => x.DistrictId == id && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year).ToListAsync();

            if (formC == null)
            {
                return null;
            }

            return formC;
        }

        [HttpPut("UpdateCaseOfMonthFromC")]
        public async Task<IActionResult> PutFormC(int id, List<Seizure_CasesMonth_Form_C> seizure_CasesMonth_Form_C)
        {
            foreach (var item in seizure_CasesMonth_Form_C)
            {
              

                _context.Entry(item).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                foreach (var item in seizure_CasesMonth_Form_C)
                {
                    if (!FormAExists(item.Id))
                    {
                        return NotFound();
                    }
                }
                throw;
            }

            return NoContent();
        }


        [HttpPost("PostCasesOfMonthFormC")]
        public async Task<ActionResult<Seizure_CasesMonth_Form_C>> PostGammaUnitFormC(Seizure_CasesMonth_Form_C seizure_CasesMonth_Form_C)
        {
            if (_context.status_of_cases_form_c == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.seizure_CasesMonth_Form_C'  is null.");
            }

            List<Seizure_CasesMonth_Form_C> formC = getSeizureCasesOfMonthC(seizure_CasesMonth_Form_C.ProvinceId, seizure_CasesMonth_Form_C.DistrictId);
            _context.status_of_cases_form_c.AddRange(formC);
            await _context.SaveChangesAsync();
            await _context.SaveChangesAsync();

            return CreatedAtAction("Seizures_Form_C", new { id = 1 }, formC);
        }

    }


    public class NamesAndSeriaalNo
    {
        public string? SerialNo { get; set; }
        public string? Name { get; set; } 
    }
}