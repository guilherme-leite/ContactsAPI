using ContactsAPI.Data;
using ContactsAPI.DTOS;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
    }
}
