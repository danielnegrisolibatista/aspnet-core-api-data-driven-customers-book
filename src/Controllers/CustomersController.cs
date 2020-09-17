﻿using System.Collections.Generic;
using System.Threading.Tasks;
using aspnet_core_api_data_driven_customers_book.Data.Repositories;
using aspnet_core_api_data_driven_customers_book.Models;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_core_api_data_driven_customers_book.Controllers
{
    [ApiController]
    [Route("v1/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Customer>>> Get()
        {
            List<Customer> customers = await _customerRepository.Get();

            return Ok(customers);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            Customer customer = await _customerRepository.GetById(id);

            if (customer == null)
            {
                return NotFound(); 
            }

            return Ok(customer);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<CustomerInputModel>> Post([FromBody] CustomerInputModel customerInputModel)
        {
            if (customerInputModel == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                Customer customer = new Customer(customerInputModel.FirstName, customerInputModel.LastName, customerInputModel.Birthday);

                await _customerRepository.SaveAsync(customer);

                return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<CustomerInputModel>> Put(int id, [FromBody] CustomerInputModel customerInputModel)
        {
            if (customerInputModel == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                Customer customer = await _customerRepository.GetById(id);

                if (customer == null)
                {
                    return NotFound();
                }

                customer.FirstName = customerInputModel.FirstName;
                customer.LastName = customerInputModel.LastName;
                customer.Birthday = customerInputModel.Birthday;

                await _customerRepository.UpdateAsync(customer);

                return NoContent();
            } 
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            Customer customer = await _customerRepository.GetById(id);

            if (customer == null)
            {
                return NotFound();
            }

            await _customerRepository.DeleteAsync(customer);

            return NoContent();
        }
    }
}