using Application.DataTransferObjects.HandleAssunto;
using Application.DataTransferObjects;

namespace Application.Services.Interfaces;

public interface IAssuntoService
{
    ResultGeneric<IEnumerable<GetAssuntoDTO>> GetAll();

    ResultGeneric<GetAssuntoDTO> GetById(int cod);

    Result Create(AssuntoDTO request);

    Result Update(int cod, AssuntoDTO request);

    Result Delete(int cod);
}