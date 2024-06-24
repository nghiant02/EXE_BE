﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.DTOs.ProductDTOs
{
    public class AddProductDTO
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image is required!")]
        public IEnumerable<string> ProductImage { get; set; }

        [Required(ErrorMessage = "Price is required!")]
        public double? Price { get; set; }

        [Required(ErrorMessage = "Category is required!")]
        public int? CategoryId { get; set; }

        public IEnumerable<ColorDetailDTO> ProductColors { get; set; }

        public IEnumerable<string> ProductSize { get; set; }
    }

    public class ColorDetailDTO
    {
        public string ColorName { get; set; }
        public string HexCode { get; set; }
        public string ColorImage { get; set; }
    }
}
