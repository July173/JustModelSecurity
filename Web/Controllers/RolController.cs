﻿using Business;
using Data;
using Entity.DTOs.Rol;
using Entity.DTOs.UserRol;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de permisos en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolController : ControllerBase
    {
        private readonly RolBusiness _RolBusiness;
        private readonly ILogger<RolController> _logger;

        /// <summary>
        /// Constructor del controlador de permisos
        /// </summary>
        /// <param name="RolBusiness">Capa de negocio de permisos</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public RolController(RolBusiness RolBusiness, ILogger<RolController> logger)
        {
            _RolBusiness = RolBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los permisos del sistema (get)
        /// </summary>
        /// <returns>Lista de permisos</returns>
        /// <response code="200">Retorna la lista de permisos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRols()
        {
            try
            {
                var Rols = await _RolBusiness.GetAllRolesAsync();
                return Ok(Rols);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permisos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un permiso específico por su ID (GetById)
        /// </summary>
        /// <param name="id">ID del permiso</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el permiso solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolData), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolById(int id)
        {
            try
            {
                var Rol = await _RolBusiness.GetRolByIdAsync(id);
                return Ok(Rol);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {RolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo permiso en el sistema (post)
        /// </summary>
        /// <param name="RolDto">Datos del permiso a crear</param>
        /// <returns>Permiso creado</returns>
        /// <response code="201">Retorna el permiso creado</response>
        /// <response code="400">Datos del permiso no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(RolDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]


        public async Task<IActionResult> CreateRol([FromBody] RolDto RolDto)
        {
            try
            {
                var createdRol = await _RolBusiness.CreateRolAsync(RolDto);
                return CreatedAtAction(nameof(GetRolById), new { id = createdRol.Id }, createdRol);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear permiso");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear permiso");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        ///<summary>
        ///Modifica parcialmente los datos de un rol (patch)
        /// </summary>
        /// <param name="id">Id del rol</param>
        /// <param name="dto">datos del rol para actualizar</param>
        /// <returns>actualiza los datos</returns>
        /// <response code="200">Retorna el permiso solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor</response>

        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> PatchRol(int id, [FromBody] RolUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "El ID de la ruta no coincide con el del cuerpo " });

            try
            {
                var result = await _RolBusiness.UpdateParcialRolAsync(dto);
                return result ? Ok() : NotFound();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al modificar rol con ID {RolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error interno al modificar el rol con ID {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }

        }

        ///<summary>
        ///actualiza todos los datos del rol (put)
        /// </summary>
        /// /// <param name="id">ID del rol a actualizar</param>
        /// <param name="dto">DTO con todos los datos del rol</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="200">Rol actualizado correctamente</response>
        /// <response code="400">ID no coincide o datos inválidos</response>
        /// <response code="404">Rol no encontrado</response>
        /// <response code="500">Error interno del servidor</response>

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutRol(int id, [FromBody] RolUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "El ide la ruta no coincide con el del cuerpo" });
            try
            {
                var result = await _RolBusiness.UpdateRolAsync(dto);
                return result ? Ok() : NotFound();

            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validacion fallida al actualizar rol con id {RolId}", id);
                return BadRequest(new { message = ex.Message });

            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con id {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error interno al actualizar el rol con id {Rolid}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        ///<summary>
        ///Elimina permanenentemente los datos del rol (delete permanente)
        /// </summary>
        /// <param name="id">ID del rol</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="200">Rol eliminado exitosamente</response>
        /// <response code="400">ID inválido</response>
        /// <response code="404">Rol no encontrado</response>
        /// <response code="500">Error interno</response>   

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRol(int id)
        {
            try
            {
                var result = await _RolBusiness.DeleteRolAsync(id);
                return result ? Ok() : NotFound();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar el rol con ID {RolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con ID {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }



        ///<summary>
        ///elimina el rol logicamente todos los datos del rol (delete)
        /// </summary>
        /// /// <param name="id">ID del rol</param>
        /// <param name="active">Nuevo estado activo</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="200">Estado del rol actualizado</response>
        /// <response code="400">ID inválido</response>
        /// <response code="404">Rol no encontrado</response>
        /// <response code="500">Error interno</response>

        [HttpDelete("active")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> SetActive( [FromBody] RolStatusDto dto)
        {

            try
            {
                var result = await _RolBusiness.SetRolActiveAsync(dto);
                return result ? Ok() : NotFound();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al cambiar estado activo del rol");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID: {RolId}", dto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al cambiar estado activo del rol");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una relación específica por su ID
        /// </summary>
        /// <param name="id">ID de la relación</param>
        /// <returns>Relación solicitada</returns>
        /// <response code="200">Retorna la relación solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Relación no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("User/{idUser}")]
        [ProducesResponseType(typeof(UserRolDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolUserByIduser(int idUser)
        {
            try
            {
                var rolUser = await _RolBusiness.GetRolUserByIdUserAsync(idUser);
                return Ok(rolUser);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la relación con ID: {RolUserId}", idUser);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Relación no encontrada con ID: {RolUserId}", idUser);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener la relación con ID: {RolUserId}", idUser);
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}