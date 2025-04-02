using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IQueryable<UserDTO>> GetAll();
        Task<UserDTO> Get(int id);
        Task<int> CreateReturnId(UserDTO userDTO);
        Task<UserDTO> GetByEmail(string email);
        Task<RoleDTO> GetRoleByUserId(int id);
        Task Create(UserDTO userDTO);
        Task Update(UserDTO userDTO);
        Task Delete(int id);
        Task<bool> CreateGoogle(UserDTO userDTO);
    }
}
