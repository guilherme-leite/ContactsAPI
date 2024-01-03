using ContactsAPI.Data;
using ContactsAPI.DTOS;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Numerics;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext _dbContext;

        public ContactsController(ContactsAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_dbContext.Contacts.ToList());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);
            
            if (contact == null) return NotFound();

            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ContactDto contact)
        {
            var newContact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = contact.Address,
                Email = contact.Email,
                FullName = contact.FullName,
                Phone = contact.Phone,
            };

            await _dbContext.Contacts.AddAsync(newContact);
            await _dbContext.SaveChangesAsync();

            return Ok(newContact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, ContactDto contact)
        {
            var updatedContact = await _dbContext.Contacts.FindAsync(id);

            if (updatedContact == null) return NotFound();

            updatedContact.FullName = contact.FullName;
            updatedContact.Address = contact.Address;
            updatedContact.Phone = contact.Phone;
            updatedContact.Email = contact.Email;

            await _dbContext.SaveChangesAsync();

            return Ok(updatedContact);
        }
    }
}
