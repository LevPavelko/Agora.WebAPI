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
    public class SellerReviewService : ISellerReviewService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public SellerReviewService(IUnitOfWork database, IMapper mapper)
        {
            Database = database;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SellerReviewDTO>> GetAll()
        {
            var sellerReviews = await Database.SellerReviews.GetAll();
            return _mapper.Map<IQueryable<SellerReview>, IEnumerable<SellerReviewDTO>>(sellerReviews);
        }
        public async Task<SellerReviewDTO> Get(int id)
        {
            var sellerReview = await Database.SellerReviews.Get(id);
            if (sellerReview == null)
                throw new ValidationExceptionFromService("There is no seller review with this id", "");
            return new SellerReviewDTO
            {
                Id = sellerReview.Id,
                Comment = sellerReview.Comment,
                Rating = sellerReview.Rating,
                Date = sellerReview.Date,
                SellerId = sellerReview.SellerId
            };
        }

        public async Task Create(SellerReviewDTO sellerReviewDTO)
        {
            var sellerReview = new SellerReview
            {
                Comment = sellerReviewDTO.Comment,
                Rating = sellerReviewDTO.Rating,
                Date = sellerReviewDTO.Date,
                SellerId = sellerReviewDTO.SellerId
            };
            await Database.SellerReviews.Create(sellerReview);
            await Database.Save();
        }
        public async Task Update(SellerReviewDTO sellerReviewDTO)
        {
            var sellerReview = new SellerReview
            {
                Id = sellerReviewDTO.Id,
                Comment = sellerReviewDTO.Comment,
                Rating = sellerReviewDTO.Rating,
                Date = sellerReviewDTO.Date,
                SellerId = sellerReviewDTO.SellerId
            };
            Database.SellerReviews.Update(sellerReview);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.SellerReviews.Delete(id);
            await Database.Save();
        }
    }
}
