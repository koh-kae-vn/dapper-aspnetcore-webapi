using DapperASPNetCore.Contracts;
using DapperASPNetCore.Dto;
using DapperASPNetCore.Para.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DapperASPNetCore.Controllers
{
    [Route("api/Common")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly ILogger<CompaniesController> _logger;
        public CommonController(ICompanyRepository companyRepo, ILogger<CompaniesController> logger)
        {
            _companyRepo = companyRepo;
            _logger = logger;
        }

        [HttpPost("ExecCmd")]
        public async Task<IActionResult> ExecCmd(paraCore model)
        {
            try
            {
                (bool, string) result;
                result = await _companyRepo.ExecCmd(model.spName, model.dicPara, model.cmType);
                if (result.Item1)
                    return Ok();
                else
                {
                    _logger.LogError(result.Item2);
                    return StatusCode(500, result.Item2);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecCmdWithQuery")]
        public async Task<IActionResult> ExecCmdWithQuery(paraCoreQuery model)
        {
            try
            {
                string q = Helpers.Helper.Decrypt(model.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }

                (bool, string) result;
                result = await _companyRepo.ExecCmd(q);

                if (result.Item1)
                    return Ok();
                else
                {
                    _logger.LogError(result.Item2);
                    return StatusCode(500, result.Item2);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnDsWithQuery")]
        public async Task<IActionResult> ExecReturnDsWithQuery(paraCoreQuery model)
        {
            try
            {
                string q = Helpers.Helper.Decrypt(model.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }

                var outVavlue = await _companyRepo.ExecReturnDs(q);
                if(outVavlue.Item2 != "")
                {
                    throw new Exception(outVavlue.Item2);
                }
                string strOutVavlue = JsonConvert.SerializeObject(outVavlue.Item1);
                return Content(strOutVavlue);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnDs")]
        public async Task<IActionResult> ExecReturnDs(paraCore model)
        {
            try
            {
                var outVavlue = await _companyRepo.ExecReturnDs(model.spName, model.dicPara, model.cmType);
                if (outVavlue.Item2 != "")
                {
                    throw new Exception(outVavlue.Item2);
                }
                string strOutVavlue = JsonConvert.SerializeObject(outVavlue.Item1);
                return Content(strOutVavlue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnDrWithQuery")]
        public async Task<IActionResult> ExecReturnDrWithQuery(paraCoreQuery model)
        {
            try
            {
                string q = Helpers.Helper.Decrypt(model.dataContent);
                if(q == "")
                {
                    throw new Exception("Decrypt Fail");
                }
                var companies = await _companyRepo.ExecReturnDr(q);

                if (companies.Item1 == null)
                    return Ok(null);

                return Content(JsonConvert.SerializeObject(companies.Item1.Table));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("ExecReturnDr")]
        public async Task<IActionResult> ExecReturnDr(paraCore model)
        {
            try
            {
                var companies = await _companyRepo.ExecReturnDr(model.spName, model.dicPara, model.cmType);
                //IEnumerable<Dictionary<string, object>> result = Helpers.Helper.ToDictionary(companies.Table);
                //string outVavlue = JsonConvert.SerializeObject(result);

                //string outVavlue = JsonConvert.SerializeObject(companies.Table);

                return Content(JsonConvert.SerializeObject(companies.Item1.Table));
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnJsonObject")]
        public async Task<IActionResult> ExecReturnJsonObject(paraCore model)
        {
            try
            {
                var companies = await _companyRepo.ExecReturnDr(model.spName, model.dicPara, model.cmType);

                if (companies.Item1 == null)
                    return Ok(null);

                string json = new JObject(
                                companies.Item1.Table.Columns.Cast<DataColumn>()
                                  .Select(c => new JProperty(c.ColumnName, JToken.FromObject(companies.Item1.Table.Rows[0][c])))
                            ).ToString(Formatting.None);

                return Content(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnJsonObjectWithQuery")]
        public async Task<IActionResult> ExecReturnJsonObjectWithQuery(paraCoreQuery model)
        {
            try
            {
                string q = Helpers.Helper.Decrypt(model.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }

                var companies = await _companyRepo.ExecReturnDr(q);

                if (companies.Item1 == null)
                    return Ok(null);

                string json = new JObject(
                                companies.Item1.Table.Columns.Cast<DataColumn>()
                                  .Select(c => new JProperty(c.ColumnName, JToken.FromObject(companies.Item1.Table.Rows[0][c])))
                            ).ToString(Formatting.None);

                return Content(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnValueWithQuery")]
        public async Task<IActionResult> ExecReturnValueWithQuery(paraCoreQuery model)
        {
            try
            {
                string q = Helpers.Helper.Decrypt(model.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }
                var outVavlue = await _companyRepo.ExecReturnValue(q);
                if (outVavlue.Item2 != "")
                {
                    throw new Exception(outVavlue.Item2);
                }
                return Ok(new { objValue = outVavlue.Item1 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnValue")]
        public async Task<IActionResult> ExecReturnValue(paraCore model)
        {
            try
            {
                var outVavlue = await _companyRepo.ExecReturnValue(model.spName, model.dicPara, model.cmType);
                if (outVavlue.Item2 != "")
                {
                    throw new Exception(outVavlue.Item2);
                }
                return Ok(new { objValue = outVavlue.Item1 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnDtWithQuery")]
        public async Task<IActionResult> ExecReturnDtWithQuery(paraCoreQuery model)
        {
            try
            {
                string q = Helpers.Helper.Decrypt(model.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }
                var outVavlue = await _companyRepo.ExecReturnDt(q);
                if(outVavlue.Item2 != "")
                {
                    throw new Exception(outVavlue.Item2);
                }
                return Ok(outVavlue.Item1);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnDt")]
        public async Task<IActionResult> ExecReturnDt(paraCore model)
        {
            try
            {
                var outVavlue = await _companyRepo.ExecReturnDt(model.spName, model.dicPara, model.cmType);
                if (outVavlue.Item2 != "")
                {
                    throw new Exception(outVavlue.Item2);
                }
                return Ok(outVavlue.Item1);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
