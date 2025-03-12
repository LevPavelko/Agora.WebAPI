using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface ISubcategoryService
    {
        Task<IQueryable<SubcategoryDTO>> GetAll();
        Task<SubcategoryDTO> Get(int id);
        Task Create(SubcategoryDTO subcategoryDTO);
        Task Update(SubcategoryDTO subcategoryDTO);
        Task Delete(int id);
    }
}
