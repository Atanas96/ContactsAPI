using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactAPIDbContext dbContext;
        public ContactsController(ContactAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]

        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact != null)
                return Ok(contact);
            return NotFound("The contact is not found");
        }

        [HttpPost]

        public async Task<IActionResult> AddContact(AddContactRequest contact)
        {
            var newContact = new Contact()
            {
                Id = Guid.NewGuid(),
                FullName = contact.FullName,
                Email = contact.Email,
                Phone = contact.Phone,
                Address = contact.Address,
            };
            await dbContext.Contacts.AddAsync(newContact); 
            await dbContext.SaveChangesAsync();

            return Ok(newContact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContact)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact != null)
            {
                contact.FullName = updateContact.FullName;
                contact.Email = updateContact.Email;
                contact.Phone = updateContact.Phone;
                contact.Address = updateContact.Address;

                await dbContext.SaveChangesAsync();
                return Ok(contact);

            }
            return NotFound("The contact is not found.");
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var deleteContact = await dbContext.Contacts.FindAsync(id);

            if (deleteContact == null) return NotFound("The contact is not found. ");

            dbContext.Contacts.Remove(deleteContact);
            await dbContext.SaveChangesAsync();
            return Ok(deleteContact);
           
        }
    }
}
