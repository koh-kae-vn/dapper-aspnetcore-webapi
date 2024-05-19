using DapperASPNetCore.Contracts;
using DapperASPNetCore.Para.Models;
using DapperASPNetCore.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Controllers
{
    [Route("api/TEST_SERVICE")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly ICreditRepository _creditRepo;

        private readonly ILogger<TestController> _logger;

        public TestController(ICompanyRepository companyRepo, ICreditRepository creditRepo, ILogger<TestController> logger)
        {
            _companyRepo = companyRepo;
            _creditRepo = creditRepo;
            _logger = logger;
        }

        [HttpPost("ExecCmd")]
        public async Task<IActionResult> ExecCmd(paraCore model)
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
                //string q = Helpers.Helper.Decrypt(paraCore.dataContent);
                //if (q == "")
                //{
                //    throw new Exception("Decrypt Fail");
                //}

                //var model = JsonConvert.DeserializeObject<paraCore>(q);

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
        [HttpPost("ExecCmdCredit")]
        public async Task<IActionResult> ExecCmdCredit(paraCore model)
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
                //string q = Helpers.Helper.Decrypt(paraCore.dataContent);
                //if (q == "")
                //{
                //    throw new Exception("Decrypt Fail");
                //}

                //var model = JsonConvert.DeserializeObject<paraCore>(q);

                (bool, string) result;
                result = await _creditRepo.ExecCmd(model.spName, model.dicPara, model.cmType);
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
    }
}
