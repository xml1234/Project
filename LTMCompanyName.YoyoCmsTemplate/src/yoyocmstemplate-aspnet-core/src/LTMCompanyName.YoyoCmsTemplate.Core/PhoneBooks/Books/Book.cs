using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace LTMCompanyName.YoyoCmsTemplate.PhoneBooks.Books
{
    public class Book:Entity<long>
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
    }
    public enum BookType : byte
    {
        Undefined,
        Advanture,
        Biography,
        Dystopia,
        Fantastic,
        Horror,
        Science,
        ScienceFiction,
        Poetry
    }
}