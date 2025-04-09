using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class TotalProductsByStoreDTO
    {
        public int StoreId { get; set; }        
        public int TotalQuantitySold { get; set; }
    }
}
