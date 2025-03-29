using Application.DataTransferObjects.HandleLivro;
using Application.DataTransferObjects;

namespace Application.Services.Interfaces;

public interface ILivroPrecoService
{
    ResultGeneric<IEnumerable<GetLivroPrecoDTO>> GetAll();

    ResultGeneric<GetLivroPrecoDTO> GetById(int cod);

    Result Create(LivroPrecoDTO request);

    Result Update(int cod, LivroPrecoDTO request);

    Result Delete(int cod);
}
