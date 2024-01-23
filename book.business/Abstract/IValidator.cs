using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace book.business.Abstract
{
    public interface IValidator<T>
    {
    //   
    // Dictionary<string , string> ErrorMessage {get; set;}
     string ErrorMessage {get; set;} 

     bool Validation (T entity);
    }
}