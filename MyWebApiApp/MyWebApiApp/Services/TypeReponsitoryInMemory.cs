using MyWebApiApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApiApp.Services
{
    public class TypeReponsitoryInMemory : ITypeRepository
    {

        static List<TypeVM> types = new List<TypeVM>
        {
                new TypeVM{IdType=1, NameType="tivi" },
                new TypeVM{IdType=2, NameType="phone" },
                new TypeVM{IdType=3, NameType="ipad" },
                new TypeVM{IdType=4, NameType="computer" },
                new TypeVM{IdType=4, NameType="table" },
        };
        public TypeVM addType(TypeModel type)
        {
            var _type = new TypeVM
            {
                IdType = types.Max(i => i.IdType) + 1,
                NameType = type.Name

            };
            types.Add(_type);
            return _type;
        }
        public void deleteType(int id)
        {
            var _type = types.SingleOrDefault(t => t.IdType == id);
            types.Remove(_type);

        }

        public List<TypeVM> getAllTypes()
        {
            return types;
        }

        public TypeVM getById(int id)
        {
            return types.SingleOrDefault(t => t.IdType == id);
        }

        public void updateType(TypeVM type)
        {
            var _type = types.SingleOrDefault(t => t.IdType == type.IdType);
            if (_type != null)
            {
                _type.NameType = type.NameType;
            }
        }
    }
}
