using Application.DataTransferObjects;
using Application.DataTransferObjects.HandleLivro;

namespace Application.Services.Interfaces;

public interface ILivroService {
    ResultGeneric<IEnumerable<GetLivroDTO>> GetAll();

    ResultGeneric<GetLivroDTO> GetById(int cod);

    Result Create(LivroDTO request);

    Result Update(int cod, LivroDTO request);

    Result Delete(int cod);
}
