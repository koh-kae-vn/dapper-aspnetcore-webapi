using DapperASPNetCore.Contracts;
using DapperASPNetCore.Dto;
using DapperASPNetCore.Para.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DapperASPNetCore.Controllers
{
    [Route("api/CREDIT")]
    [ApiController]
    public class CREDITController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly ILogger<CREDITController> _logger;
        public CREDITController(ICompanyRepository companyRepo, ILogger<CREDITController> logger)
        {
            _companyRepo = companyRepo;
            _logger = logger;
        }

        [HttpPost("ExecCmdAdv")]
        public async Task<IActionResult> ExecCmdAdv(paraCoreQuery model)
        {
            var re = Request;
            var headers = re.Headers;
            string reqId = string.Empty;
            StringValues x = default(StringValues);
            if (headers.ContainsKey("reqId"))
            {
                var m = headers.TryGetValue("reqId", out x);
                if (x.Count > 0)
                {
                    reqId = x.First();
                }
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string q = Helpers.Helper.Decrypt(model.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }

                (bool?, string) result;
                result = await _companyRepo.ExecCmdAdv(q);

                if (result.Item1 == true || result.Item1 == null)
                {
                    stopwatch.Stop();

                    if (result.Item1 == null)
                    {
                        _logger.LogWarning("{0},{1} ms,q: ", reqId, stopwatch.ElapsedMilliseconds, result.Item2);
                    }
                    else
                    {
                        _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                    }
                    return Ok();
                }
                else
                {
                    throw new Exception(result.Item2);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError("{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecCmd")]
        public async Task<IActionResult> ExecCmd(paraCoreQuery paraCore)
        {
            var re = Request;
            var headers = re.Headers;
            string reqId = string.Empty;
            StringValues x = default(StringValues);
            if (headers.ContainsKey("reqId"))
            {
                var m = headers.TryGetValue("reqId", out x);
                if (x.Count > 0)
                {
                    reqId = x.First();
                }
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string q = Helpers.Helper.Decrypt(paraCore.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }

                var model = JsonConvert.DeserializeObject<paraCore>(q);

                (bool, string) result;
                result = await _companyRepo.ExecCmd(model.spName, model.dicPara, model.cmType);
                if (result.Item1)
                {
                    stopwatch.Stop();
                    _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                    return Ok();
                }
                else
                {
                    throw new Exception(result.Item2);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError("{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecCmdWithQuery")]
        public async Task<IActionResult> ExecCmdWithQuery(paraCoreQuery model)
        {
            var re = Request;
            var headers = re.Headers;
            string reqId = string.Empty;
            StringValues x = default(StringValues);
            if (headers.ContainsKey("reqId"))
            {
                var m = headers.TryGetValue("reqId", out x);
                if (x.Count > 0)
                {
                    reqId = x.First();
                }
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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
                {
                    stopwatch.Stop();
                    _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                    return Ok();
                }
                else
                {
                    throw new Exception(result.Item2);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError("{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnDsWithQuery")]
        public async Task<IActionResult> ExecReturnDsWithQuery(paraCoreQuery model)
        {
            string reqId = string.Empty;

            if (Request.Headers.TryGetValue("reqId", out StringValues headerValue))
            {
                if (headerValue.Count > 0)
                {
                    reqId = headerValue.First();
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string q = Helpers.Helper.Decrypt(model.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }

                var outVavlue = await _companyRepo.ExecReturnDs(q);
                if (outVavlue.Item2 != "")
                {
                    throw new Exception(outVavlue.Item2);
                }
                string strOutVavlue = JsonConvert.SerializeObject(outVavlue.Item1);

                stopwatch.Stop();
                _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);

                return Content(strOutVavlue);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex,"{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnDs")]
        public async Task<IActionResult> ExecReturnDs(paraCoreReq para)
        {
            string reqId = string.Empty;

            if (Request.Headers.TryGetValue("reqId", out StringValues headerValue))
            {
                if (headerValue.Count > 0)
                {
                    reqId = headerValue.First();
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string q = Helpers.Helper.Decrypt(para.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }

                var model = JsonConvert.DeserializeObject<paraCore>(q);

                var outVavlue = await _companyRepo.ExecReturnDs(model.spName, model.dicPara, model.cmType);
                if (outVavlue.Item2 != "")
                {
                    throw new Exception(outVavlue.Item2);
                }
                string strOutVavlue = JsonConvert.SerializeObject(outVavlue.Item1);
                
                stopwatch.Stop();
                _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);

                return Content(strOutVavlue);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnDrWithQuery")]
        public async Task<IActionResult> ExecReturnDrWithQuery(paraCoreQuery model)
        {
            string reqId = string.Empty;

            if (Request.Headers.TryGetValue("reqId", out StringValues headerValue))
            {
                if (headerValue.Count > 0)
                {
                    reqId = headerValue.First();
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string q = Helpers.Helper.Decrypt(model.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }
                var companies = await _companyRepo.ExecReturnDr(q);

                if (companies.Item2 != "")
                {
                    throw new Exception(companies.Item2);
                }

                if (companies.Item1 == null)
                {
                    stopwatch.Stop();
                    _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                    return Ok(null);
                }
                stopwatch.Stop();
                _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                return Content(JsonConvert.SerializeObject(companies.Item1.Table));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);

                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("ExecReturnDr")]
        public async Task<IActionResult> ExecReturnDr(paraCoreReq para)
        {
            string reqId = string.Empty;

            if (Request.Headers.TryGetValue("reqId", out StringValues headerValue))
            {
                if (headerValue.Count > 0)
                {
                    reqId = headerValue.First();
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string q = Helpers.Helper.Decrypt(para.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }

                var model = JsonConvert.DeserializeObject<paraCore>(q);

                var companies = await _companyRepo.ExecReturnDr(model.spName, model.dicPara, model.cmType);

                if (companies.Item2 != "")
                {
                    throw new Exception(companies.Item2);
                }

                if (companies.Item1 == null)
                {
                    stopwatch.Stop();
                    _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                    return Ok(null);
                }

                stopwatch.Stop();
                _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                return Content(JsonConvert.SerializeObject(companies.Item1.Table));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnJsonObject")]
        public async Task<IActionResult> ExecReturnJsonObject(paraCoreReq para)
        {
            string reqId = string.Empty;

            if (Request.Headers.TryGetValue("reqId", out StringValues headerValue))
            {
                if (headerValue.Count > 0)
                {
                    reqId = headerValue.First();
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string q = Helpers.Helper.Decrypt(para.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }

                var model = JsonConvert.DeserializeObject<paraCore>(q);

                var companies = await _companyRepo.ExecReturnDr(model.spName, model.dicPara, model.cmType);
                if (companies.Item2 != "")
                {
                    throw new Exception(companies.Item2);
                }

                if (companies.Item1 == null)
                {
                    stopwatch.Stop();
                    _logger.LogWarning("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                    return Ok(null);

                }

                string json = new JObject(
                                companies.Item1.Table.Columns.Cast<DataColumn>()
                                  .Select(c => new JProperty(c.ColumnName, JToken.FromObject(companies.Item1.Table.Rows[0][c])))
                            ).ToString(Formatting.None);

                stopwatch.Stop();
                _logger.LogWarning("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                return Content(json);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnJsonObjectWithQuery")]
        public async Task<IActionResult> ExecReturnJsonObjectWithQuery(paraCoreQuery model)
        {
            string reqId = string.Empty;

            if (Request.Headers.TryGetValue("reqId", out StringValues headerValue))
            {
                if (headerValue.Count > 0)
                {
                    reqId = headerValue.First();
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string q = Helpers.Helper.Decrypt(model.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }

                var companies = await _companyRepo.ExecReturnDr(q);
                if (companies.Item2 != "")
                {
                    throw new Exception(companies.Item2);
                }

                if (companies.Item1 == null)
                {
                    stopwatch.Stop();
                    _logger.LogWarning("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                    return Ok(null);

                }

                string json = new JObject(
                                companies.Item1.Table.Columns.Cast<DataColumn>()
                                  .Select(c => new JProperty(c.ColumnName, JToken.FromObject(companies.Item1.Table.Rows[0][c])))
                            ).ToString(Formatting.None);

                stopwatch.Stop();
                _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                return Content(json);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnValueWithQuery")]
        public async Task<IActionResult> ExecReturnValueWithQuery(paraCoreQuery model)
        {
            string reqId = string.Empty;

            if (Request.Headers.TryGetValue("reqId", out StringValues headerValue))
            {
                if (headerValue.Count > 0)
                {
                    reqId = headerValue.First();
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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

                stopwatch.Stop();
                _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                return Ok(new { objValue = outVavlue.Item1 });
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnValue")]
        public async Task<IActionResult> ExecReturnValue(paraCoreReq para)
        {
            string reqId = string.Empty;

            if (Request.Headers.TryGetValue("reqId", out StringValues headerValue))
            {
                if (headerValue.Count > 0)
                {
                    reqId = headerValue.First();
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string q = Helpers.Helper.Decrypt(para.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }

                var model = JsonConvert.DeserializeObject<paraCore>(q);

                var outVavlue = await _companyRepo.ExecReturnValue(model.spName, model.dicPara, model.cmType);
                if (outVavlue.Item2 != "")
                {
                    throw new Exception(outVavlue.Item2);
                }
                stopwatch.Stop();
                _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);
                return Ok(new { objValue = outVavlue.Item1 });
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnDtWithQuery")]
        public async Task<IActionResult> ExecReturnDtWithQuery(paraCoreQuery model)
        {
            string reqId = string.Empty;

            if (Request.Headers.TryGetValue("reqId", out StringValues headerValue))
            {
                if (headerValue.Count > 0)
                {
                    reqId = headerValue.First();
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string q = Helpers.Helper.Decrypt(model.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }
                var outVavlue = await _companyRepo.ExecReturnDt(q);
                if (outVavlue.Item2 != "")
                {
                    throw new Exception(outVavlue.Item2);
                }

                string result = JsonConvert.SerializeObject(outVavlue.Item1);

                stopwatch.Stop();
                _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);

                return Content(result);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecReturnDt")]
        public async Task<IActionResult> ExecReturnDt(paraCoreReq para)
        {
            string reqId = string.Empty;

            if (Request.Headers.TryGetValue("reqId", out StringValues headerValue))
            {
                if (headerValue.Count > 0)
                {
                    reqId = headerValue.First();
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string q = Helpers.Helper.Decrypt(para.dataContent);
                if (q == "")
                {
                    throw new Exception("Decrypt Fail");
                }

                var model = JsonConvert.DeserializeObject<paraCore>(q);

                var outVavlue = await _companyRepo.ExecReturnDt(model.spName, model.dicPara, model.cmType);
                if (outVavlue.Item2 != "")
                {
                    throw new Exception(outVavlue.Item2);
                }

                string result = JsonConvert.SerializeObject(outVavlue.Item1);

                stopwatch.Stop();
                _logger.LogInformation("{0},{1} ms", reqId, stopwatch.ElapsedMilliseconds);

                return Content(result);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "{0},{1} ms,ex: {2}", reqId, stopwatch.ElapsedMilliseconds, ex.Message);

                return StatusCode(500, ex.Message);
            }
        }

    }
}
