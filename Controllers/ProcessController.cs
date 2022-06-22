using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SelfHostedServer.Models.Entities;
using SelfHostedServer.ModelsDTO.ModelsDto;
using SelfHostedServer.Services;
using System.Threading.Tasks;

namespace SelfHostedServer.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/{controller}")]
    [ApiVersion("2.0")]
    public class ProcessController : ControllerBase
    {
        private readonly IProcessService Service;
        private readonly IMapper Mapper;
        public ProcessController(IProcessService service, IMapper mapper)
        {
            this.Service = service;
            this.Mapper = mapper;
        }

        [HttpPost]
        [Route("{action}")]
        public async Task<IActionResult> Sale([FromBody] SaleDto dto)
        {
            var sale = Mapper.Map<Ticket>(dto);

            await Service.SaleAsync(sale);

            return Ok();
        }

        [HttpPost]
        [Route("{action}")]
        public async Task<IActionResult> Refund([FromBody] RefundDto dto)
        {
            var refund = Mapper.Map<Refund>(dto);

            await Service.RefundAsync(refund);

            return Ok();
        }
    }
}
