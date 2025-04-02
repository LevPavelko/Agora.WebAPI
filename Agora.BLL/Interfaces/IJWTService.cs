using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IJWTService
    {
        string GenerateJwtToken(UserDTO userDTO, RoleDTO roleDTO);
       RoleDTO DecryptJwtToken(string token);
    }
}
