using AutoMapper;
using Business;
using Business.Interfaces;
using Business.Services;
using Data;
using Entity.DTOs.Person;
using Entity.DTOs.User;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;
using ValidationException = Utilities.Exceptions.ValidationException;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IBaseService<PersonDto, Person> _service;
        private readonly IMapper _mapper;
        private readonly IPersonService _personService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(
            IBaseService<PersonDto, Person> service,
            IMapper mapper,
            IPersonService personService,
            ILogger<PersonController> logger)
        {
            _service = service;
            _mapper = mapper;
            _personService = personService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las personas");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new { message = "Persona no encontrada." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener persona por ID");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create([FromBody] PersonDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Aquí no necesitas mapear dto a dto, solo envía el dto al servicio
                await _personService.AddFromCreateDtoAsync(dto);

                return Ok(new { message = "Persona creada correctamente." });
            }
            catch (ValidationException vex)
            {
                // Captura tu excepción de validación para devolver BadRequest
                return BadRequest(new { message = vex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear persona");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }


        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update([FromBody] PersonDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var entity = _mapper.Map<PersonDto>(dto);
                await _service.UpdateAsync(entity);
                return Ok(new { message = "Persona actualizada correctamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar persona");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Persona no encontrada." });

                await _service.DeleteAsync(id);
                return Ok(new { message = "Persona eliminada correctamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar persona");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [HttpPatch("update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PatchPerson([FromBody] PersonUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _personService.PatchPersonAsync(dto);

                if (result)
                    return Ok(new { message = "Persona actualizada parcialmente con éxito." });

                return NotFound(new { message = "Persona no encontrada o actualización fallida." });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación en PatchPerson");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Persona no encontrada en PatchPerson: {Id}", dto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error externo en PatchPerson");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en PatchPerson");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [HttpPatch("active")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SetActive([FromBody] PersonStatusDto dto)
        {
            try
            {
                var result = await _service.SetActiveAsync(dto);
                if (result)
                    return Ok(new { message = "Estado actualizado correctamente." });

                return NotFound(new { message = "No se encontró la entidad." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar el estado activo.");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }
    }
}







// Puedes agregar más endpoints aquí como Create, Update, Delete, etc.



/*
        [HttpPatch]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePartialPerson([FromBody] PersonUpdateDto dto)
        {
            try
            {
                var result = await _personBusiness.UpdateParcialPersonAsync(dto);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación en actualización parcial");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Persona no encontrada en actualización parcial: {Id}", dto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error en actualización parcial de persona");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("person-active")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SetPersonActive([FromBody] PersonStatusDto dto)
        {
            try
            {
                var result = await _personBusiness.SetPersonActiveAsync(dto);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación al cambiar estado");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Persona no encontrada al cambiar estado: {Id}", dto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al cambiar estado activo de persona");
                return StatusCode(500, new { message = ex.Message });
            }
        }*/


