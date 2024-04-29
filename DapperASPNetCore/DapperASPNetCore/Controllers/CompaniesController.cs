using DapperASPNetCore.Contracts;
using DapperASPNetCore.Dto;
using DapperASPNetCore.Para.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
	[Route("api/companies")]
	[ApiController]
	public class CompaniesController : ControllerBase
	{
		private readonly ICompanyRepository _companyRepo;

		public CompaniesController(ICompanyRepository companyRepo)
		{
			_companyRepo = companyRepo;
		}

		[HttpPost("execCmd")]
		public async Task<IActionResult> ExecCmd(paraCore model)
		{
			try
			{
				string q = Helpers.Helper.Decrypt(model.data);
				var result = await _companyRepo.ExecCmd(q);
				if (result.Item1)
					return Ok();
				else
					return StatusCode(500, result.Item2);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("testDs")]
		public async Task<IActionResult> ExecReturnDs(paraCore model)
		{
			try
			{
				var companies = await _companyRepo.ExecReturnDs(model.spName, model.dicPara, model.cmType);
				string outVavlue = JsonConvert.SerializeObject(companies);

				return Ok(outVavlue);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("testDr")]
		public async Task<IActionResult> ExecReturnDr(paraCore model)
		{
			try
			{
				var companies = await _companyRepo.ExecReturnDr(model.spName, model.dicPara, model.cmType);
				//IEnumerable<Dictionary<string, object>> result = Helpers.Helper.ToDictionary(companies.Table);
				//string outVavlue = JsonConvert.SerializeObject(result);

				//string outVavlue = JsonConvert.SerializeObject(companies.Table);
				string json = new JObject(
								companies.Table.Columns.Cast<DataColumn>()
								  .Select(c => new JProperty(c.ColumnName, JToken.FromObject(companies.Table.Rows[0][c])))
							).ToString(Formatting.None);

				return Ok(json);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("testValue")]
		public async Task<IActionResult> ExecReturnValue(paraCore model)
		{
			try
			{
				var companies = await _companyRepo.ExecReturnValue(model.spName, model.dicPara, model.cmType);
				return Ok(new { objValue = companies } );
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("test")]
		public async Task<IActionResult> ExecReturnDt(paraCore model)
		{
			try
			{
				var companies = await _companyRepo.ExecReturnDt(model.spName,model.dicPara,model.cmType);
				string outVavlue = JsonConvert.SerializeObject(companies);
				return Ok(outVavlue);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}


		[HttpGet]
		public async Task<IActionResult> GetCompanies()
		{
			try
			{
				var companies = await _companyRepo.GetCompanies();
				return Ok(companies);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}", Name = "CompanyById")]
		public async Task<IActionResult> GetCompany(int id)
		{
			try
			{
				var company = await _companyRepo.GetCompany(id);
				if (company == null)
					return NotFound();

				return Ok(company);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateCompany(CompanyForCreationDto company)
		{
			try
			{
				var createdCompany = await _companyRepo.CreateCompany(company);
				return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCompany(int id, CompanyForUpdateDto company)
		{
			try
			{
				var dbCompany = await _companyRepo.GetCompany(id);
				if (dbCompany == null)
					return NotFound();

				await _companyRepo.UpdateCompany(id, company);
				return NoContent();
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCompany(int id)
		{
			try
			{
				var dbCompany = await _companyRepo.GetCompany(id);
				if (dbCompany == null)
					return NotFound();

				await _companyRepo.DeleteCompany(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("ByEmployeeId/{id}")]
		public async Task<IActionResult> GetCompanyForEmployee(int id)
		{
			try
			{
				var company = await _companyRepo.GetCompanyByEmployeeId(id);
				if (company == null)
					return NotFound();

				return Ok(company);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}/MultipleResult")]
		public async Task<IActionResult> GetCompanyEmployeesMultipleResult(int id)
		{
			try
			{
				var company = await _companyRepo.GetCompanyEmployeesMultipleResults(id);
				if (company == null)
					return NotFound();

				return Ok(company);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("MultipleMapping")]
		public async Task<IActionResult> GetCompaniesEmployeesMultipleMapping()
		{
			try
			{
				var company = await _companyRepo.GetCompaniesEmployeesMultipleMapping();

				return Ok(company);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("multiple")]
		public async Task<IActionResult> CreateCompany(List<CompanyForCreationDto> companies)
		{
			try
			{
				await _companyRepo.CreateMultipleCompanies(companies);
				return StatusCode(201);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}
	}
}
