using Microsoft.AspNetCore.Mvc;
using netcore.api.Factories;
using netcore.api.Models;
using netcore.infrastructure.Entities;
using netcore.infrastructure.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace netcore.api.Controllers
{
    [Produces("application/json+hateoas")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public IUserRepository Repository { get; set; }
        public UsersController(IUserRepository userRepository)
        {
            Repository = userRepository;
        }
        /// <summary>
        /// Obtiene todas las usuarios creados
        /// </summary>
        /// <returns>Retorna una lista de usuarios</returns>
        [HttpGet(Name = "get-users")]
        [ProducesResponseType(200, Type = typeof(RootDto<UserDto>))]
        public async Task<IActionResult> Get()
        {
            var users = await Repository.GetAll();
            var items = users.Select(x => x.ToUserDto()).ToList();
            return Ok(items);
        }
        /// <summary>
        /// Obtiene un usuario solicitado
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Retorna el objeto completo de la usuario solicitado</returns>
        /// <response code="404">Retorna NotFound cuando no existe un usuario con el identificador enviado</response>          
        [HttpGet("{id}", Name = "get-user")]
        [ProducesResponseType(200, Type = typeof(ItemDto<UserDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await Repository.GetById(id);
            UserDto item = user.ToUserDto();
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        /// <summary>
        /// Actualiza un usuario creado anteriormente
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <param name="user"></param>
        /// <returns>Retorna un objeto actualizado del usuario</returns>
        /// <response code="404">Retorna NotFound cuando no existe un usuario con el identificador enviado</response>          
        /// <response code="400">Retorna BadRequest cuando la entidad enviada es inválida</response>          
        [HttpPut("{id}", Name = "update-user")]
        [ProducesResponseType(typeof(ItemDto<UserDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(Guid id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = await Repository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            user.Id = id;
            UserDto updatedItem = (await Repository.Update(user.ToUser())).ToUserDto();
            return CreatedAtAction("Get", new { id = updatedItem.Id }, updatedItem);
        }
        /// <summary>
        /// Crea un usuario nuevo
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Objeto nuevo del usuario creado</returns>
        /// <response code="200">Retorna el nuevo objeto de usuario creado</response> 
        /// <response code="400">Retorna BadRequest cuando la entidad enviada es inválida</response>          
        [ProducesResponseType(200, Type = typeof(ItemDto<UserDto>))]
        [ProducesResponseType(400)]
        [HttpPost(Name = "create-user")]
        public async Task<IActionResult> Post(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            user.Id = Guid.NewGuid();
            UserDto item = (await Repository.Add(user.ToUser())).ToUserDto();
            return CreatedAtAction("Get", new { id = item.Id }, item);
        }
        /// <summary>
        /// Elimina un usuario específico
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <response code="404">Retorna NotFound cuando no existe un usuario con el identificador enviado</response>          
        [HttpDelete("{id}", Name = "delete-user")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await Repository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            await Repository.Remove(id);
            return Ok();
        }
    }
}
