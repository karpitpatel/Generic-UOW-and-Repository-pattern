using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericUOW.Core.Data.Model;

namespace GenericUOW.Data.Model
{
    public class TestUOW : BaseEntity
    {
        public int Id { get; set;}
        public string Name { get; set; }
    }
}
