using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;

namespace Agora.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public UserService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<UserDTO>> GetAll()
        {
            var users = await Database.Users.GetAll();
            return _mapper.Map<IQueryable<UserDTO>>(users.ToList());

        }
        public async Task<UserDTO> Get(int id)
        {
            var user = await Database.Users.Get(id);
            if (user == null)
                throw new ValidationException("There is no user with this id", "");
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Password = user.Password,
                GoogleId = user.GoogleId

            };
        }
        public async Task<UserDTO> GetByEmail(string email)
        {
            var user = await Database.Users.GetByEmail(email);
            if (user == null)
                throw new ValidationException("There is no user with this email", "");
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Password = user.Password,
                GoogleId = user.GoogleId

            };
        }
        public async Task Create(UserDTO userDTO)
        {
            var user = new User
            {
                Name = userDTO.Name,
                Surname = userDTO.Surname,
                PhoneNumber = userDTO.PhoneNumber,
                Email = userDTO.Email,
                Password = userDTO.Password
            };
            await Database.Users.Create(user);
            await Database.Save();            
        }

        public async Task<int> CreateReturnId(UserDTO userDTO)
        {            
            var user = new User
            {
                Name = userDTO.Name,
                Surname = userDTO.Surname,
                PhoneNumber = userDTO.PhoneNumber,
                Email = userDTO.Email,
                Password = userDTO.Password
            };
            await Database.Users.Create(user);
            await Database.Save();
            return user.Id;
        }
        public async Task Update(UserDTO userDTO)
        {

            var user = new User
            {
                Id = userDTO.Id,
                Name = userDTO.Name,
                Surname = userDTO.Surname,
                PhoneNumber = userDTO.PhoneNumber,
                Email = userDTO.Email,
                Password = userDTO.Password
            };
            Database.Users.Update(user);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Users.Delete(id);
            await Database.Save();
        }

    }
}
