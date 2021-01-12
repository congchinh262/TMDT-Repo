using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobieStoreWeb.DTO
{
    public class ProductCommentDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Content { get; set; }

        public byte Rating { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
