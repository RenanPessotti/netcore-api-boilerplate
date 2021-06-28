using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boilerplate.Application.DTOs.Person;
using Boilerplate.Application.Filters;
using Boilerplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonAppService _PersonAppService;

        public PersonController(IPersonAppService PersonAppService)
        {
            _PersonAppService = PersonAppService;
        }


        /// <summary>
        /// Returns all Persons in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<GetPersonDto>>> GetPersons([FromQuery] GetPersonsFilter filter)
        {
            return Ok(await _PersonAppService.GetAllPersons(filter));
        }


        /// <summary>
        /// Get one Person by id from the database
        /// </summary>
        /// <param name="id">The Person's ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetPersonDto), 200)]
        public async Task<ActionResult<GetPersonDto>> GetPersonById(Guid id)
        {
            var Person = await _PersonAppService.GetPersonById(id);
            if (Person == null) return NotFound();
            return Ok(Person);
        }

        /// <summary>
        /// Insert one Person in the database
        /// </summary>
        /// <param name="dto">The Person information</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<GetPersonDto>> Create([FromBody] InsertPersonDto dto)
        {
            var newPerson = await _PersonAppService.CreatePerson(dto);
            return CreatedAtAction(nameof(GetPersonById), new { id = newPerson.Id }, newPerson);

        }

        /// <summary>
        /// Update a Person from the database
        /// </summary>
        /// <param name="id">The Person's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<GetPersonDto>> Update(Guid id, [FromBody] UpdatePersonDto dto)
        {

            var updatedPerson = await _PersonAppService.UpdatePerson(id, dto);

            if (updatedPerson == null) return NotFound();

            return NoContent();
        }


        /// <summary>
        /// Delete a Person from the database
        /// </summary>
        /// <param name="id">The Person's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {

            var deleted = await _PersonAppService.DeletePerson(id);
            if (deleted) return NoContent();
            return NotFound();

        }
    }
}
