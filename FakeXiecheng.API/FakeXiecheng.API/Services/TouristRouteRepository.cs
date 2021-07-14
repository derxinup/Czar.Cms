﻿using FakeXiecheng.API.Database;
using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    public class TouristRouteRepository:ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext context) {
            _context = context;
        }

        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            return _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefault(n => n.Id == touristRouteId);
        }

        public IEnumerable<TouristRoute> GetTouristRoutes(string keyword,string ratingOperator, int? ratingValue)
        {
            IQueryable<TouristRoute> result= _context
                .TouristRoutes
                .Include(t => t.TouristRoutePictures);
            if (!string.IsNullOrWhiteSpace(keyword)) {
                keyword = keyword.Trim();
                result=result.Where(t => t.Title.Contains(keyword));
            }
            if (ratingValue >= 0) {
                switch (ratingOperator) {
                    case "largerThan":
                        result = result.Where(t=>t.Rating>= ratingValue);
                        break;
                    case "lessThan":
                        result = result.Where(t => t.Rating <= ratingValue);
                        break;
                    case "equalTo":
                        result = result.Where(t => t.Rating == ratingValue);
                        break;
                }
            }
            return result.ToList();
        }

        public bool TouristRouteExists(Guid touristRouteId) {
            return _context.TouristRoutes.Any(t => t.Id == touristRouteId);
        }

        public IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouteId) {
            return _context.TouristRoutePictures.Where(p => p.TouristRouteId == touristRouteId).ToList();
        }

        public TouristRoutePicture GetPicture(int pictureId) {
            return _context.TouristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefault();
        }

        public void AddTouristRoute(TouristRoute touristRoute) {
            if (touristRoute == null) {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
        }

        public void AddTouristRoutePicturre(Guid touristRouteId, TouristRoutePicture touristRoutePicture) {
            if (touristRouteId == Guid.Empty) {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if (touristRoutePicture==null) {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.TouristRoutePictures.Add(touristRoutePicture);
        }

        public void DeleteTouristRoute(TouristRoute touristRoute) {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes) {
            _context.TouristRoutes.RemoveRange(touristRoutes);
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture) {
            _context.TouristRoutePictures.Remove(touristRoutePicture);
        }

        public IEnumerable<TouristRoute> GetTouristRoutesByIDList(IEnumerable<Guid> ids) {
            return _context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToList();
        }

        public bool Save() {
            return (_context.SaveChanges() >= 0);
        }
    }
}
