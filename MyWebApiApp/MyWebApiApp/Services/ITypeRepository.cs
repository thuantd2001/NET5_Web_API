using MyWebApiApp.Models;
using System.Collections.Generic;

namespace MyWebApiApp.Services
{
    public interface ITypeRepository
    {
        List<TypeVM> getAllTypes();
        TypeVM getById(int id);
        TypeVM addType(TypeModel type);
        void updateType(TypeVM type);
        void deleteType(int id);

    }
}
