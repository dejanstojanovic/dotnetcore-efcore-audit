using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Auditing.Web.Api.Models
{
    public class ProductEditModel
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public double Price { get; set; }
        public String Version { get; set; }
    }
}
