using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SelfHostedServer.Models.Entities;
using SelfHostedServer.ModelsDTO.ModelsDto;
using SelfHostedServer.Services;
using System.Threading.Tasks;

namespace SelfHostedServer.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/{controller}/{action}")]
    [ApiVersion("2.0")]
    public class ProcessController : ControllerBase
    {
        private readonly IProcessService Service;
        public ProcessController(IProcessService service)
        {
            this.Service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Sale([FromBody] SaleDto dto)
        {
            await Service.SaleAsync(dto);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Refund([FromBody] RefundDto dto)
        {
            await Service.RefundAsync(dto);

            return Ok();
        }
    }
}
