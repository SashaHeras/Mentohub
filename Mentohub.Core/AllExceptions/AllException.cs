using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.AllExceptions
{
    public class AllException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IActionResult NotFoundObjectResult(string v)
        {
            throw new NotImplementedException(v);
        }
        public CurrentUser NotFoundObject(string v)
        {
            throw new NotImplementedException(v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool RankException(string v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string ArgumentNullException(string v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public void NewException(string v) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public UserDTO NullException(string v)
        {
            throw new NotImplementedException();
        }
        public ChangeRoleDTO NullObject(string v)
        {
            throw new NotImplementedException();
        }
        public string NotificationMessage(string v)
        {
            throw new NotImplementedException();
        }
    }
}
