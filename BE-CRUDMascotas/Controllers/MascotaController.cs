using AutoMapper;
using BE_CRUDMascotas.Models;
using BE_CRUDMascotas.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_CRUDMascotas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MascotaController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMascotas()
        {
            try
            {
                var listMascotas = await _context.Mascotas.ToListAsync();

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
                var mascota = await _context.Mascotas.FindAsync(id);

                if(mascota == null)
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
                var mascota = await _context.Mascotas.FindAsync(id);
                if (mascota == null)
                {
                    return NotFound();
                }

                _context.Mascotas.Remove(mascota);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostMascota(MascotaDTO mascotaDTO)
        {
            try
            {
                var mascota = _mapper.Map<Mascota>(mascotaDTO);

                mascota.FechaCreacion = DateTime.Now;
                _context.Add(mascota);
                await _context.SaveChangesAsync();

                var mascotaItemDTO = _mapper.Map<MascotaDTO>(mascota);

                return CreatedAtAction("GetMascota", new {id = mascota.Id}, mascotaItemDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMascota(int id, Mascota mascotaDTO)
        {
            try
            {
                var mascota = _mapper.Map<Mascota>(mascotaDTO);

                if (id != mascota.Id)
                {
                    return BadRequest();
                }

                var mascotaItem = await _context.Mascotas.FindAsync(id);

                if (mascotaItem == null)
                {
                    return NotFound();
                }

                mascotaItem.Nombre = mascota.Nombre;
                mascotaItem.Edad = mascota.Edad;
                mascotaItem.Raza = mascota.Raza;
                mascotaItem.Peso = mascota.Peso;
                mascotaItem.Color = mascota.Color;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }


}
