using AutoMapper;
using BE_CRUDMascotas.Models;
using BE_CRUDMascotas.Models.DTO;
using BE_CRUDMascotas.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_CRUDMascotas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMascotaRepository _repository;

        public MascotaController(IMapper mapper, IMascotaRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMascotas()
        {
            try
            {
                var listMascotas = await _repository.GetListMascotas();

                var listMascotasDTO = _mapper.Map<IEnumerable<MascotaDTO>>(listMascotas);

                return Ok(listMascotasDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMascota(int id)
        {
            try
            {
                var mascota = await _repository.GetMascota(id);

                if (mascota == null)
                {
                    return NotFound();
                }

                var mascotaDTO = _mapper.Map<MascotaDTO>(mascota);

                return Ok(mascota);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMascota(int id)
        {
            try
            {
                var mascota = await _repository.GetMascota(id);
                if (mascota == null)
                {
                    return NotFound();
                }

                await _repository.DeleteMascota(mascota);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostMascota(MascotaDTO mascotaDto)
        {
            try
            {
                var mascota = _mapper.Map<Mascota>(mascotaDto);

                mascota.FechaCreacion = DateTime.Now;

                mascota = await _repository.AddMascota(mascota);

                var mascotaItemDto = _mapper.Map<MascotaDTO>(mascota);

                return CreatedAtAction("GetMascota", new { id = mascotaItemDto.Id }, mascotaItemDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMascota(int id, MascotaDTO mascotaDto)
        {
            try
            {
                var mascota = _mapper.Map<Mascota>(mascotaDto);

                if (id != mascota.Id)
                {
                    return BadRequest();
                }

                var mascotaItem = await _repository.GetMascota(id);

                if (mascotaItem == null)
                {
                    return NotFound();
                }

                await _repository.UpdateMascota(mascota);

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }


}
