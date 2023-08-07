using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface IGeralPersitence
    {
        //Geral
        void Insert<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        void DeleteRange<T>(T[] entity) where T: class;
        Task<bool> SaveChangesAsync();        

    }
}