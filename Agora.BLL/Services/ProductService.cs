﻿using System;
using System.Collections.Generic;
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
    public class ProductService : IProductService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public ProductService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<ProductDTO>> GetAll()
        {
            var products = await Database.Products.GetAll();
            return _mapper.Map<IQueryable<ProductDTO>>(products.ToList());

        }
        public async Task<IEnumerable<ProductDTO>> GetFilteredByName(string filter) //????
        {
            
            var filteredProducts = await Database.Products.Find(p => p.Name.Contains(filter));
            return _mapper.Map<IEnumerable<ProductDTO>>(filteredProducts);
        }
        public async Task<ProductDTO> Get(int id)
        {
            var product = await Database.Products.Get(id);
            if (product == null)
                throw new ValidationException("There is no product with this id", "");
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Rating = product.Rating,
                ImagesPath = product.ImagesPath,
                IsAvailable = product.IsAvailable

            };
        }

        public async Task Create(ProductDTO productDTO)
        {
            var product = new Product
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                StockQuantity = productDTO.StockQuantity,
                Rating = productDTO.Rating,
                ImagesPath = productDTO.ImagesPath,
                IsAvailable = productDTO.IsAvailable

            };
            await Database.Products.Create(product);
            await Database.Save();
        }
        public async Task Update(ProductDTO productDTO)
        {

            var product = new Product
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                StockQuantity = productDTO.StockQuantity,
                Rating = productDTO.Rating,
                ImagesPath = productDTO.ImagesPath,
                IsAvailable = productDTO.IsAvailable

            };
            Database.Products.Update(product);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Products.Delete(id);
            await Database.Save();
        }

    }
}
