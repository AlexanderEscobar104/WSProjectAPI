using Business;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using PostEntity = DataAccess.Data.Post;
using CustomerEntity = DataAccess.Data.Customer;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Controllers.Post
{
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private BaseService<PostEntity> PostService;
        private BaseService<CustomerEntity> CustomerService;
        public PostController(BaseService<PostEntity> postService, BaseService<CustomerEntity> customerService)
        {
            PostService = postService;
            CustomerService = customerService;
        }

        /// <summary>
        /// metodo para traer todos los post 
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IQueryable<PostEntity> GetAll()
        {
            return PostService.GetAll();
        }

        /// <summary>
        /// metodo para crera el post
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost()]
        public PostEntity Create([FromBodyAttribute]  PostEntity entity)
        {

            //valida el body para cortar a 97 caracteres
            if (entity.Body.Length > 200)
            {
                entity.Body = entity.Body.Substring(1,97) + "...";
            }
            //valida  el tipo de categoria
            if (entity.Category == "Farándula")
            {
                entity.Type = 1;
            }else if(entity.Category == "Política")
            {
                entity.Type = 2;
            }
            else if (entity.Category == "Futbol")
            {
                entity.Type = 3;
            }

            //valida Que el usuario asociado si exista 
            CustomerEntity customer = new CustomerEntity();
            customer =  CustomerService.FindById(entity.CustomerId);
            
            if(customer == null)
            {
                return null;
            }
            
            return PostService.Create(entity);
        }

        /// <summary>
        /// metodo para actualizar el post 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut()]
        public PostEntity Update([FromBodyAttribute] PostEntity entity)
        {
            return PostService.Update(entity.PostId, entity, out bool changed);
        }

        /// <summary>
        ///metodo para eliminar el post
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpDelete()]
        public PostEntity Delete([FromBodyAttribute] PostEntity entity)
        {
            return PostService.Delete(entity);
        }


    }
}
