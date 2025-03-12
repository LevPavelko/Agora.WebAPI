﻿using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;
using Agora.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Services
{
    public class BrandService : IBrandService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public BrandService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<BrandDTO>> GetAll()
        {
            var brands = await Database.Brands.GetAll();
            return _mapper.Map<IQueryable<BrandDTO>>(brands.ToList());
        }

        public async Task<BrandDTO> Get(int id)
        {
            var brand = await Database.Brands.Get(id);
            if (brand == null)
                throw new ValidationException("There is no brand with this id", "");
            return new BrandDTO
            {
                Id = brand.Id,
                Name = brand.Name,
            };
        }

        public async Task Create(BrandDTO brandDTO)
        {
            var brand = new Brand
            {
                Id = brandDTO.Id,
                Name= brandDTO.Name,
            };
            await Database.Brands.Create(brand);
            await Database.Save();
        }
        public async Task Update(BrandDTO brandDTO)
        {
            var brand = new Brand
            {
                Id = brandDTO.Id,
                Name = brandDTO.Name,
            };
            Database.Brands.Update(brand);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Brands.Delete(id);
            await Database.Save();
        }
    }
}
