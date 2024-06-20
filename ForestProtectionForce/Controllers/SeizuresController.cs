using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;

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
                var data = await _context.Seizures_Form_A.ToListAsync();
                var formA_names = FormA_names();
                foreach (var item in formA_names)
                {
                    decimal obIndependentSum = _context.Seizures_Form_A
                      .Where(x => x.name == item.Name && x.month <= 0 && x.year == 0)
                      .Sum(x => x.during_month_independent);
                    decimal obJointSum = _context.Seizures_Form_A
                         .Where(x => x.name == item.Name && x.month <= 0 && x.year == 0)
                         .Sum(x => x.during_month_joint);
                }
                return data;
            } catch (Exception ex) {
                return null;
            }

        }
        [HttpGet("GetFormAWithDistrict")]
        public async Task<ActionResult<IEnumerable<Seizures_Form_A>>> GetFormAOnDistrict(int id, int month = 0, int year = 0)
        {
            try
            {
                if (_context.Seizures_Form_A == null)
                {
                    return NotFound();
                }
                month = month == 0 ? DateTime.Now.Month : month;
                year = year == 0 ? DateTime.Now.Year : year;
                var data = await _context.Seizures_Form_A.Where(x => x.districtId == id && x.month == month).ToListAsync();
                var formA_names = FormA_names();
                foreach (var item in formA_names)
                {
                    decimal obIndependentSum =  _context.Seizures_Form_A
                      .Where(x => x.name == item.Name && x.month < month && x.year == year && x.districtId == id)
                      .Sum(x => x.during_month_independent);
                    decimal obJointSum = _context.Seizures_Form_A
                         .Where(x => x.name == item.Name && x.month < month && x.year == year && x.districtId == id)
                         .Sum(x => x.during_month_joint);

                    decimal ob_total = (_context.Seizures_Form_A
                                      .FirstOrDefault(x => x.name == item.Name && x.month == month && x.year == year && x.districtId == id)
                                      ?.during_month_joint ?? 0) + obJointSum;

                    decimal independent_total = (_context.Seizures_Form_A
                     .FirstOrDefault(x => x.name == item.Name && x.month == month && x.year == year && x.districtId == id)?.during_month_independent ?? 0) + obIndependentSum;
                    // Get the rows that correspond to the current item and update their properties
                    var rowsToUpdate = data.Where(x => x.name == item.Name).ToList();
                    foreach (var row in rowsToUpdate)
                    {
                        // Update the properties of the row
                        row.ob_independent = obIndependentSum;
                        row.ob_joint = obJointSum;
                        row.total_joint = ob_total;
                        row.total_independent = independent_total;
                    }

                }
                return data;
               
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

            List<Seizures_Form_A> seizures_Form_As = getSeizureReportA(seizures_Form_A.provinceId, seizures_Form_A.districtId,seizures_Form_A.month, seizures_Form_A.year??0);
            _context.Seizures_Form_A.AddRange(seizures_Form_As);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostFormA", new { id = 1 }, seizures_Form_As[0]);
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
        public async Task<ActionResult<Seizures_Form_A>> CheckSeizureAlreadyExistForDistrictAndMonth(int id, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.Now.Month : month;
            year = year == 0 ? DateTime.Now.Year : year;
            Seizures_Form_A? formA = await _context.Seizures_Form_A.FirstOrDefaultAsync(x => x.districtId == id && x.month == month && x.year == year);

            if (formA == null)
            {
                return null;
            }

            return formA;
        }

        
        private bool FormAExists(int id)
        {
            return (_context.Seizures_Form_A?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [NonAction]
        public List<Seizures_Form_A> getSeizureReportA(int provinceId, int districtId, int month, int year = 0)
        {
            year = year == 0 ? DateTime.Now.Year : year;
           

            List<Seizures_Form_A> seizures_Form_A = new List<Seizures_Form_A>();
            List<NamesAndSeriaalNo> formA_names = FormA_names();
            foreach (var item in formA_names)
            {
                seizures_Form_A.Add(new Seizures_Form_A
                {
                    id = 0,
                    serialNo = item.SerialNo,
                    name = item.Name,
                    ob_independent = 0.00m,
                    during_month_independent = 0.00m,
                    total_independent = 0.00m,
                    ob_joint = 0.00m,
                    during_month_joint = 0.00m,
                    total_joint = 0.00m,
                    provinceId = provinceId,
                    districtId = districtId,
                    DateOfInsertion = DateTime.Now,
                    IsActive = true,
                    LastUpdatedOn = DateTime.Now,
                    month = month,
                    year =year
                });
            }
            return seizures_Form_A;

        }

        [NonAction]
        public List<NamesAndSeriaalNo> FormA_names()
        {
            List<NamesAndSeriaalNo> formA_names = new List<NamesAndSeriaalNo>
            {
                        new NamesAndSeriaalNo { SerialNo = "1", Name = "Deodar" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Kail" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Fir" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Chir" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Spruce" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "others" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Total Conifer Timber" },
                        new NamesAndSeriaalNo { SerialNo = "2", Name = "Broad leaved timber" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Poplar" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Walnut" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Willow" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Sheesham" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Toon" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Mango" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Khair" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Simbal" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Oak" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Others" },
                        new NamesAndSeriaalNo { SerialNo = "", Name = "Total Broad Leaved Timber (cfts)" },
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
            return formA_names;
        }

        [NonAction]
        public Gamma_unit_form_b getSeizureGammaUnitB(int provinceId, int districtId, int month, int year, string gammaUnit)
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
                Month = month,
                Year = year,
                Gamma_Unit = gammaUnit,
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
        public List<Seizure_CasesMonth_Form_C> getSeizureCasesOfMonthC(int? provinceId, int? districtId, int month, int year)
        {
            List<Seizure_CasesMonth_Form_C> formC = new List<Seizure_CasesMonth_Form_C>{
                new Seizure_CasesMonth_Form_C
            {
                DistrictId = districtId,
                Sno = 1,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,
                Month = month,
                Year = year,
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
                Month = month,
                Year = year,
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
                Month =month,
                Year = year,
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

        [NonAction]
        public seizure_man_animal_conflict ManAnimalConflict(int provinceId, int districtId, int month, int year, string districtName)
        {
            seizure_man_animal_conflict seizure_Man_Animal_Conflict = new seizure_man_animal_conflict
            {
                DistrictId = districtId,
                ProvinceId = provinceId,
                Id  = 0,
                NameOfGammaUnit = districtName,
                NoOfFPFPersonnelDeployed = 0,
                PlaceOfOccurrence= "",
                Remarks = "",
                IsActive = true,
                LastUpdatedOn = DateTime.Now,
                Month =month,
                Year = year,
                SNo = 1
            };

            return seizure_Man_Animal_Conflict;

        }

        [NonAction]
        public ForestFire FirestFireAdd(int provinceId, int districtId, int month, int year, string districtName)
        {
            ForestFire forestFire = new()
            {
                district_id = districtId,
                province_id = provinceId,
                id = 0,
                gamma_unit_name = districtName,
                date_of_insertion = DateTime.Now,
                fire_datetime = DateTime.Now,
                fire_spot = "",
                forest_crop_damaged = "",
                forest_damage_area = 0.00m,
                forest_division_name = "",
                fpf_personnel_name = "",
                ob_total_cases = _context.forest_Fire
                  .Where(x => x.month < month && x.year == year && x.district_id == districtId)
                  .Count(),
               total_fire_cases = 1,
                is_active = true,
                last_updated_on = DateTime.Now,
                month = month,
                year = year,
                sno = 1
            };

            return forestFire;

        }


        [NonAction]
        public Complaints_Registered ComplaintsRegisteredAdd(int provinceId, int districtId, int month, int year)
        {
            Complaints_Registered complaintsRegistered = new()
            {
                ActionTaken = "",
                BriefDescription = "",
                CognizanceUnderSection = "",
                ComplaintArea = "",
                ComplaintNo = "",
                DateOfInsertion = DateTime.Now,
                DateTimeOfReceipt = DateTime.Now,
                DistrictId = districtId,
                Id = 0,
                sno = 1,
                IsActive=true,
                LastUpdatedOn = DateTime.Now,
                Month = month,
                NameSignMunshiMoharir = "",
                ProvinceId = provinceId,
                SourceOfComplaint = "",
                UpdatedBy = "",
                Year = year

            };

            return complaintsRegistered;
        }

        [NonAction]
        public ForestOffender ForestOffenderAdd(int provinceId, int districtId, int month, int year)
        {
            ForestOffender forestOffender = new()
            {
                Id = 0,
                ActiveDormant = "",
                AreaOfOperations = "",
                CasesRegistered = 0,
                CasesStatus = "",
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,
                ModusOperandi = "",
                Month = month,
                Year = year,
                NameOfForestOffender = "",
                Sno = 1, 
                UpdatedBy = ""
            };

            return forestOffender;
        }

        [NonAction]
        public List<AntiPochingFormA> AntiPochingFormAAdd(int provinceId, int districtId, int month, int year)
        {
            List<AntiPochingFormA> formA = new List<AntiPochingFormA>{

            new AntiPochingFormA
            {
                Id = 0,
                Activity = "Jungle Gasht/Aabhi Gasht",
                Details = "",
                Unit = 0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month,
                Year = year,
                Sno = 1,
                UpdatedBy = ""
            },
            new AntiPochingFormA
            {

                Id = 0,
                Activity = "Vehicle/Vessel Checking",
                Details = "",
                Unit = 0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month,
                Year = year,
                Sno = 2,
                UpdatedBy = ""
            },
            new AntiPochingFormA
            {

                Id = 0,
                Activity = "Houses/Buildings Search (with details)",
                Details = "",
                Unit =0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month,
                Year = year,
                Sno = 3,
                UpdatedBy = ""
            },
          
            new AntiPochingFormA
            {

                Id = 0,
                Activity = "Foot Patroling (with details of location)",
                Details = "",
                Unit = 0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month,
                Year = year,
                Sno = 4,
                UpdatedBy = ""
            },
             new AntiPochingFormA
            {

                Id = 0,
                Activity = "Vehicle Patroling (with details of location)",
                Details = "",
                Unit = 0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month,
                Year = year,
                Sno = 5,
                UpdatedBy = ""
            },
             new AntiPochingFormA
            {

                Id = 0,
                Activity = "Naka (with details of location)",
                Details = "",
                Unit = 0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month,
                Year = year,
                Sno = 6,
                UpdatedBy = ""
            } };
            return formA;
        }


        [NonAction]
        public List<AntiPochingFormB> AntiPochingFormBAdd(int provinceId, int districtId, int month, int year)
        {
            List<AntiPochingFormB> formB = new List<AntiPochingFormB>{

            new AntiPochingFormB
            {
                Id = 0,
                Article = "Boat",
                Details = "",
                Unit = 0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month,
                Year = year,
                Sno = 1,
                UpdatedBy = ""
            },
            new AntiPochingFormB
            {

                Id = 0,
                Article = "Gun (Type)",
                Details = "",
                Unit = 0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month,
                Year = year,
                Sno = 2,
                UpdatedBy = ""
            },
            new AntiPochingFormB
            {

                Id = 0,
                Article = "Decoys",
                Details = "",
                Unit = 0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month,
                Year = year,
                Sno = 3,
                UpdatedBy = ""
            },

            new AntiPochingFormB
            {

                Id = 0,
                Article = "Vehicles",
                Details = "",
                Unit = 0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month ,
                Year = year,
                Sno = 4,
                UpdatedBy = ""
            },
             new AntiPochingFormB
            {

                Id = 0,
                Article = "Carcass (Scheduled Animal)",
                Details = "",
                Unit = 0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month,
                Year = year,
                Sno = 5,
                UpdatedBy = ""
            },
             new AntiPochingFormB
            {

                Id = 0,
                Article = "Trophy, Hide, Nail, Fur, Glands etc",
                Details = "",
                Unit = 0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month,
                Year = year,
                Sno = 6,
                UpdatedBy = ""
            },
             new AntiPochingFormB
            {

                Id = 0,
                Article = "Any Other article (Please specify)",
                Details = "",
                Unit = 0,
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,

                Month = month ,
                Year = year,
                Sno = 6,
                UpdatedBy = ""
            }};
            return formB;
        }


        [NonAction]
        public AntiPochingFormC AntiPochingFormCAdd(int provinceId, int districtId, int month, int year)
        {
            AntiPochingFormC forestOffender = new()
            {
                Id = 0,
               Details ="",
               FIRRegistered ="",
               NoDate = "",
                DateOfInsertion = DateTime.Now,
                DistrictId = districtId,
                ProvinceId = provinceId,
                IsActive = true,
                LastUpdatedOn = DateTime.Now,
                Month = month,
                Year = year,
                Sno = 1,
                UpdatedBy = ""
            };

            return forestOffender;
        }

        // Form B Gamma Unit

        [HttpGet("GammaUnitB")]
        public async Task<ActionResult<IEnumerable<Gamma_unit_form_b>>> GammaUnitB()
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
        public async Task<IActionResult> UpdateGammaUnitFromB(int id, Gamma_unit_form_b seizures_GammaUnit_Form_B)
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
        public async Task<ActionResult<IEnumerable<Gamma_unit_form_b>>> GetGammaUnitFormBWithDistrict(int districtId, int month = 0, int year = 0)
        {
            try
            {
                if (_context.gamma_unit_form_b == null)
                {
                    return NotFound();
                }
                month = month == 0 ? DateTime.Now.Month : month;
                year = year == 0 ? DateTime.Now.Year : year;
                return await _context.gamma_unit_form_b.Where(x => x.DistrictId == districtId && x.Month == month && x.Year == year).ToListAsync();
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

            Gamma_unit_form_b formB = getSeizureGammaUnitB(seizures_GammaUnit_Form_B.ProvinceId, seizures_GammaUnit_Form_B.DistrictId, seizures_GammaUnit_Form_B.Month, seizures_GammaUnit_Form_B.Year, seizures_GammaUnit_Form_B.Gamma_Unit);
            _context.gamma_unit_form_b.Add(formB);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Seizures_Form_B", new { id = 1 }, formB);
        }
        [HttpGet("CheckSeizureBlreadyExistForDistrictAndMonth")]
        public async Task<ActionResult<Gamma_unit_form_b>> CheckSeizureBlreadyExistForDistrictAndMonth(int id, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.Now.Month : month;
            year = year == 0 ? DateTime.Now.Year : year;
            Gamma_unit_form_b? formA = await _context.gamma_unit_form_b.FirstOrDefaultAsync(x => x.DistrictId == id && x.Month == month && x.Year == year);

            if (formA == null)
            {
                return null;
            }

            return formA;
        }


        // FORM C - CASES OF MONTH

        [HttpGet("CheckSeizureClreadyExistForDistrictAndMonth")]
        public async Task<ActionResult<IEnumerable<Seizure_CasesMonth_Form_C>>> CheckSeizureClreadyExistForDistrictAndMonth(int id, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.Now.Month : month;
            year = year == 0 ? DateTime.Now.Year : year;
            var formC = await _context.status_of_cases_form_c.Where(x => x.DistrictId == id && x.Month == month && x.Year == year && x.IsActive == true).ToListAsync();

            if (formC == null)
            {
                return null;
            }

            return formC;
        }

        [HttpPut("UpdateCaseOfMonthFromC")]
        public async Task<IActionResult> UpdateCaseOfMonthFromC(int id, List<Seizure_CasesMonth_Form_C> seizure_CasesMonth_Form_C)
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

            List<Seizure_CasesMonth_Form_C> formC = getSeizureCasesOfMonthC(seizure_CasesMonth_Form_C.ProvinceId, seizure_CasesMonth_Form_C.DistrictId, seizure_CasesMonth_Form_C.Month??0, seizure_CasesMonth_Form_C.Year??0);
            _context.status_of_cases_form_c.AddRange(formC);
            await _context.SaveChangesAsync();
          

            return CreatedAtAction("Seizures_Form_C", new { id = 1 }, formC);
        }

        // Man & Animal Conflict 


        [HttpGet("CheckManAnimalConflictAlreadyExistForDistrictAndMonth")]
        public async Task<ActionResult<IEnumerable<seizure_man_animal_conflict>>> CheckManAnimalConflictAlreadyExistForDistrictAndMonth(int id, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.Now.Month : month;
            year = year == 0 ? DateTime.Now.Year : year;
            var formC = await _context.seizure_man_animal_conflict.Where(x => x.DistrictId == id && x.Month == month && x.Year == year && x.IsActive).ToListAsync();

            if (formC == null)
            {
                return null;
            }

            return formC;
        }

        [HttpPost("PostCasesOfMonthManAnimalConflict")]
        public async Task<ActionResult<seizure_man_animal_conflict>> PostCasesOfMonthManAnimalConflict(seizure_man_animal_conflict seizure_Man_Animal_Conflicts)
        {
            if (_context.seizure_man_animal_conflict == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.seizure_Man_Animal_Conflict'  is null.");
            }

          seizure_man_animal_conflict manAnimalConflicts = ManAnimalConflict(seizure_Man_Animal_Conflicts.ProvinceId??0 , seizure_Man_Animal_Conflicts.DistrictId??0, seizure_Man_Animal_Conflicts.Month, seizure_Man_Animal_Conflicts.Year, seizure_Man_Animal_Conflicts.NameOfGammaUnit);
            _context.seizure_man_animal_conflict.Add(manAnimalConflicts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostCasesOfMonthManAnimalConflict", new { id = 1 }, manAnimalConflicts);
        }

        [HttpPut("UpdateMonthManAnimalConflict")]
        public async Task<IActionResult> UpdateMonthManAnimalConflict(int id, seizure_man_animal_conflict seizure_Man_Animal_Conflict)
        {
            if(id == 0) // id = 0 means insertion 
            {
                _ = _context.seizure_man_animal_conflict?.Add(seizure_Man_Animal_Conflict);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            if (id != seizure_Man_Animal_Conflict.Id)
            {
                return BadRequest();
            }

            _context.Entry(seizure_Man_Animal_Conflict).State = EntityState.Modified;

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

        // Forest Fire Incident 

        [HttpGet("CheckFireIncidentAlreadyExistForDistrictAndMonth")]
        public async Task<ActionResult<IEnumerable<ForestFire>>> CheckFireIncidentAlreadyExistForDistrictAndMonth(int id, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.Now.Month : month;
            year = year == 0 ? DateTime.Now.Year : year;
            var formC = await _context.forest_Fire.Where(x => x.district_id == id && x.month == month && x.year == year && x.is_active).ToListAsync();

            if (formC == null)
            {
                return null;
            }

            return formC;
        }

        [HttpPost("PostFireIncident")]
        public async Task<ActionResult<seizure_man_animal_conflict>> PostFireIncident(ForestFire forestFire)
        {
            if (_context.forest_Fire == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.ForestFire'  is null.");
            }

            ForestFire forestFireData = FirestFireAdd(forestFire.province_id , forestFire.district_id, forestFire.month, forestFire.year, forestFire.gamma_unit_name);
            _context.forest_Fire.Add(forestFireData);
            await _context.SaveChangesAsync();
           
            return CreatedAtAction("PostFireIncident", new { id = 1 }, forestFireData);
        }

        [HttpPut("UpdateFireIncident")]
        public async Task<IActionResult> UpdateFireIncident(int id, ForestFire forestFire)
        {
            if (id == 0) // id = 0 means insertion 
            {
                _ = _context.forest_Fire?.Add(forestFire);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            if (id != forestFire.id)
            {
                return BadRequest();
            }

            _context.Entry(forestFire).State = EntityState.Modified;

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

        // Complaints Registered 

        [HttpGet("CheckComplaintsRegisteredAlreadyExistForDistrictAndMonth")]
        public async Task<ActionResult<IEnumerable<Complaints_Registered>>> CheckComplaintsRegisteredAlreadyExistForDistrictAndMonth(int id, int month = 0, int year = 0)
        {

            month = month == 0 ? DateTime.Now.Month : month;
            year = year == 0 ? DateTime.Now.Year : year;
            var formC = await _context.complaints_registered.Where(x => x.DistrictId == id && x.Month == month && x.Year == year && x.IsActive).ToListAsync();

            if (formC == null)
            {
                return null;
            }

            return formC;
        }

        [HttpPost("PostComplaintsRegistered")]
        public async Task<ActionResult<Complaints_Registered>> PostComplaintsRegistered(Complaints_Registered complaints_Registereds)
        {
            if (_context.complaints_registered == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.Complaints_Registereds'  is null.");
            }

            Complaints_Registered complaints = ComplaintsRegisteredAdd(complaints_Registereds.ProvinceId, complaints_Registereds.DistrictId, complaints_Registereds.Month, complaints_Registereds.Year);
            _context.complaints_registered.Add(complaints);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Man Animal Conflicts", new { id = 1 }, complaints);
        }

        [HttpPut("PostComplaintsRegistered")]
        public async Task<IActionResult> PostComplaintsRegistered(int id, Complaints_Registered complaints)
        {
            if (id == 0) // id = 0 means insertion 
            {
                _ = _context.complaints_registered?.Add(complaints);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            if (id != complaints.Id)
            {
                return BadRequest();
            }

            _context.Entry(complaints).State = EntityState.Modified;

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

        // Forest Offenders

        [HttpGet("CheckForestOffendersAlreadyExistForDistrictAndMonth")]
        public async Task<ActionResult<IEnumerable<ForestOffender>>> CheckForestOffendersAlreadyExistForDistrictAndMonth(int id, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.Now.Month : month;
            year = year == 0 ? DateTime.Now.Year : year;
            var formC = await _context.ForestOffenders.Where(x => x.DistrictId == id && x.Month == month && x.Year == year && x.IsActive).ToListAsync();

            if (formC == null)
            {
                return null;
            }

            return formC;
        }

        [HttpPost("PostForestOffenders")]
        public async Task<ActionResult<ForestOffender>> PostForestOffenders(ForestOffender forestOffenderData)
        {
            if (_context.ForestOffenders == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.Forest_Offenders'  is null.");
            }

            ForestOffender forestOffender = ForestOffenderAdd(forestOffenderData.ProvinceId, forestOffenderData.DistrictId, forestOffenderData.Month, forestOffenderData.Year);
            _context.ForestOffenders.Add(forestOffender);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostForestOffenders", new { id = 1 }, forestOffender);
        }

        [HttpPut("UpdateForestOffenders")]
        public async Task<IActionResult> UpdateForestOffenders(int id, ForestOffender forestOffender)
        {
            if (id == 0) // id = 0 means insertion 
            {
                _ = _context.ForestOffenders?.Add(forestOffender);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            if (id != forestOffender.Id)
            {
                return BadRequest();
            }

            _context.Entry(forestOffender).State = EntityState.Modified;

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

        // Anti-Poching Form A
       
        [HttpGet("CheckAntiPochingAlreadyExistForDistrictAndMonth")]
        public async Task<ActionResult<IEnumerable<AntiPochingFormA>>> CheckAntiPochingAlreadyExistForDistrictAndMonth(int id, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.Now.Month : month;
            year = year == 0 ? DateTime.Now.Year : year;
            var formC = await _context.AntiPochingFormA.Where(x => x.DistrictId == id && x.Month == month && x.Year == year && x.IsActive).ToListAsync();

            if (formC == null)
            {
                return null;
            }

            return formC;
        }

        [HttpPost("PostAntiPochingFormA")]
        public async Task<ActionResult<AntiPochingFormA>> PostAntiPochingFormA(AntiPochingFormA  antiPochingFormA)
        {
            if (_context.AntiPochingFormA == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.antiPochingFormA'  is null.");
            }

            List<AntiPochingFormA> antiPochingForm = AntiPochingFormAAdd(antiPochingFormA.ProvinceId, antiPochingFormA.DistrictId, antiPochingFormA.Month, antiPochingFormA.Year);
            _context.AntiPochingFormA.AddRange(antiPochingForm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostAntiPochingFormA", new { id = 1 }, antiPochingForm[0]);
        }

        [HttpPut("UpdateAntiPochingFormA")]
        public async Task<IActionResult> UpdateAntiPochingFormA(int id, AntiPochingFormA antiPochingFormA)
        {
            

            if (id != antiPochingFormA.Id)
            {
                return BadRequest();
            }

            _context.Entry(antiPochingFormA).State = EntityState.Modified;

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

        // Anti-Poching Form B

        [HttpGet("CheckAntiPochingBAlreadyExistForDistrictAndMonth")]
        public async Task<ActionResult<IEnumerable<AntiPochingFormB>>> CheckAntiPochingBAlreadyExistForDistrictAndMonth(int id, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.Now.Month : month;
            var formC = await _context.AntiPochingFormB.Where(x => x.DistrictId == id && x.Month == month && x.Year == year && x.IsActive).ToListAsync();

            if (formC == null)
            {
                return null;
            }
            
            return formC;
        }

        [HttpPost("PostAntiPochingFormB")]
        public async Task<ActionResult<AntiPochingFormA>> PostAntiPochingFormB(AntiPochingFormB antiPochingFormB)
        {
            if (_context.AntiPochingFormB == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.antiPochingFormB'  is null.");
            }

            List<AntiPochingFormB> antiPochingForm = AntiPochingFormBAdd(antiPochingFormB.ProvinceId, antiPochingFormB.DistrictId, antiPochingFormB.Month, antiPochingFormB.Year);
            _context.AntiPochingFormB.AddRange(antiPochingForm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostAntiPochingFormB", new { id = 1 }, antiPochingForm[0]);
        }

        [HttpPut("UpdateAntiPochingFormB")]
        public async Task<IActionResult> UpdateAntiPochingFormB(int id, AntiPochingFormB antiPochingFormB)
        {


            if (id != antiPochingFormB.Id)
            {
                return BadRequest();
            }

            _context.Entry(antiPochingFormB).State = EntityState.Modified;

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

        // Anti-Poching Form C

        [HttpGet("CheckAntiPochingCAlreadyExistForDistrictAndMonth")]
        public async Task<ActionResult<IEnumerable<AntiPochingFormC>>> CheckAntiPochingCAlreadyExistForDistrictAndMonth(int id, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.Now.Month : month;
            var formC = await _context.AntiPochingFormC.Where(x => x.DistrictId == id && x.Month == month && x.Year == year && x.IsActive).ToListAsync();

            if (formC == null)
            {
                return null;
            }

            return formC;
        }

        [HttpPost("PostAntiPochingFormC")]
        public async Task<ActionResult<AntiPochingFormC>> PostAntiPochingFormC(AntiPochingFormC antiPochingFormC)
        {
            if (_context.AntiPochingFormB == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.antiPochingFormC'  is null.");
            }

            AntiPochingFormC antiPochingForm = AntiPochingFormCAdd(antiPochingFormC.ProvinceId, antiPochingFormC.DistrictId, antiPochingFormC.Month, antiPochingFormC.Year);
            _context.AntiPochingFormC.Add(antiPochingForm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostAntiPochingFormC", new { id = 1 }, antiPochingForm);
        }


        [HttpPut("UpdateAntiPochingFormC")]
        public async Task<IActionResult> UpdateAntiPochingFormC(int id, AntiPochingFormC antiPochingFormC)
        {
            if (id == 0) // id = 0 means insertion 
            {
                _ = _context.AntiPochingFormC?.Add(antiPochingFormC);
                await _context.SaveChangesAsync();
                return NoContent();
            }


            if (id != antiPochingFormC.Id)
            {
                return BadRequest();
            }

            _context.Entry(antiPochingFormC).State = EntityState.Modified;

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

    }


    public class NamesAndSeriaalNo
    {
        public string? SerialNo { get; set; }
        public string? Name { get; set; } 
    }

    public class FormDistrictMonth
    {
        public int id { get; set; }
        public int month { get; set; } 
    }
}
