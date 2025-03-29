using Application.DataTransferObjects.HandleAssunto;
using Application.DataTransferObjects;
using Application.DataTransferObjects.HandleAutor;

namespace Application.Services.Interfaces;

public interface IAutorService 
{
    ResultGeneric<IEnumerable<GetAutorDTO>> GetAll();

    ResultGeneric<GetAutorDTO> GetById(int cod);

    Result Create(AutorDTO request);

    Result Update(int cod, AutorDTO request);

    Result Delete(int cod);
}
