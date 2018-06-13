﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericUOW.Domain.ViewModel
{
    public class TestUOWViewModel : BaseViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is mandatory.")]
        public string Name { get; set; }
    }
}
