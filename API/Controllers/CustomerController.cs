using Business;
using DataAccess.Data;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using CustomerEntity = DataAccess.Data.Customer;
using PostEntity = DataAccess.Data.Post;

namespace API.Controllers.Customer
{
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private BaseService<CustomerEntity> CustomerService;
        private BaseService<PostEntity> PostService;
        public CustomerController(BaseService<CustomerEntity> customerService, BaseService<PostEntity> postService)
        {
            CustomerService = customerService;
            PostService = postService;
        }
        /// <summary>
        /// metodo para retonar una sola entidad por el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("get")]
        public CustomerEntity FindById(int id)
        {
            return CustomerService.FindById(id);
        }

        /// <summary>
        /// metodo que retorna todas las entidades
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Route("list")]
        public IQueryable<CustomerEntity> GetAll()
        {
            return CustomerService.GetAll();
        }

        /// <summary>
        /// metodo para crear la entidad
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost()]
        public CustomerEntity Create([FromBodyAttribute] CustomerEntity entity)
        {
            return CreateCustomer(entity);
        }

        private CustomerEntity CreateCustomer(CustomerEntity entity)
        {
            bool existe  = false;
            //validar que no haya un usuario con el mismo nombre
            List<CustomerEntity> customers = new List<CustomerEntity>(CustomerService.GetAll());
            foreach (CustomerEntity c in customers)
            {
                if(entity.Name == c.Name)
                {
                    existe = true;
                }
            }

            if (!existe)
            {
                return CustomerService.Create(entity);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// metodo para actualizar la entidad 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut()]
        public CustomerEntity Update(CustomerEntity entity)
        {
            return CustomerService.Update(entity.CustomerId, entity, out bool changed);
        }

        /// <summary>
        /// metodo para eliminar la entidad 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpDelete()]
        public CustomerEntity Delete([FromBodyAttribute] CustomerEntity entity)
        {
            //elimina todos los Post asociados a la entidad
            List<PostEntity> customers = new List<PostEntity>(PostService.GetAll().ToList());
            foreach (PostEntity p in customers)
            {
                if (entity.CustomerId == p.CustomerId)
                {
                    PostService.Delete(p);
                }
            }
            return CustomerService.Delete(entity);
        }
    }
}
