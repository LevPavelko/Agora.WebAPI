﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.Interfaces;
using Agora.BLL.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Agora.DAL.Interfaces;
using AutoMapper;
using Agora.BLL.DTO;
using Agora.DAL.Entities;

namespace Agora.BLL.Services
{
    public class StoreService : IStoreService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;        
        public StoreService(IUnitOfWork database, IMapper mapper)
        {
            Database = database;
            _mapper = mapper;           
        }
        public async Task<IQueryable<StoreDTO>> GetAll()
        {
            var stores = await Database.Stores.GetAll();
            return _mapper.Map<IQueryable<StoreDTO>>(stores.ToList());
        }
        public async Task<StoreDTO> Get(int id)
        {
            var store = await Database.Stores.Get(id);
            if (store == null)
                throw new ValidationException("There is no store with this id", "");
            return new StoreDTO
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                CreatedAt = store.CreatedAt,
                UpdatedAt = store.UpdatedAt,
                SellerId = store.SellerId
            };
        }
        public async Task Create(StoreDTO storeDTO)
        {        
            var store = new Store
            {
                Name = storeDTO.Name,
                Description = storeDTO.Description,
                CreatedAt = storeDTO.CreatedAt,
                UpdatedAt = storeDTO.UpdatedAt,
                SellerId = storeDTO.SellerId
            };
            await Database.Stores.Create(store);
            await Database.Save();
        }
        public async Task Update(StoreDTO storeDTO)
        {
            var store = new Store
            {
                Id = storeDTO.Id,
                Name = storeDTO.Name,
                Description = storeDTO.Description,
                CreatedAt = storeDTO.CreatedAt,
                UpdatedAt = storeDTO.UpdatedAt,
                SellerId = storeDTO.SellerId
            };
            Database.Stores.Update(store);
            await Database.Save();
        }
        public async Task Delete(int id)
        {
            await Database.Stores.Delete(id);
            await Database.Save();
        }
    }
}
