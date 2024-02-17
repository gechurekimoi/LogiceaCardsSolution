using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Domain.DTOs
{
    public class BaseResponseDTO <T>
    {
        public bool IsSuccessful { get; set; }
        public string ResponseMessage { get; set; }
        public T Data { get; set; }
    }
}
