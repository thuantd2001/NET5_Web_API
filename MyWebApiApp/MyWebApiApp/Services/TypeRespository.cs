using MyWebApiApp.Data;
using MyWebApiApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApiApp.Services
{
    public class TypeRespository : ITypeRepository
    {
        private readonly MyDbContext _context;
        public TypeRespository(MyDbContext context)
        {
            _context = context;
        }
        public TypeVM addType(TypeVM type)
        {
            var _type = new Type
            {
                NameType = type.NameType,
            };
            _context.Add(_type);
            _context.SaveChanges();
            return new TypeVM
            {
                IdType = _type.IdType,
                NameType = _type.NameType,
            };
        }

        public void deleteType(int id)
        {
            var data = _context.Types.SingleOrDefault(t => t.IdType == id);
            if (data != null)
            {
                _context.Types.Remove(data);
                _context.SaveChanges();
            }

        }

        public List<TypeVM> getAllTypes()
        {
            var data = _context.Types.Select(t => new TypeVM
            {
                IdType = t.IdType,
                NameType = t.NameType,
            });
            return data.ToList();
        }

        public TypeVM getById(int id)
        {
            var data = _context.Types.SingleOrDefault(t => t.IdType == id);
            if (data != null)
            {
                return new TypeVM
                {
                    IdType = data.IdType,
                    NameType = data.NameType,
                };
            }
            return null;
        }

        public void updateType(TypeVM type)
        {
            var _type = _context.Types.SingleOrDefault(t => t.IdType == type.IdType);
            _type.NameType = type.NameType;
            _context.SaveChanges();
        }
    }
}
