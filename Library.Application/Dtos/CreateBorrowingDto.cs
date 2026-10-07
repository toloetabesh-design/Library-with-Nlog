using System;

namespace Library.Application.DTOs
{
    public class CreateBorrowingDto
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
    }
}