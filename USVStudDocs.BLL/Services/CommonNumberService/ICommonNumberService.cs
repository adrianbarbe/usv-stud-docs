using USVStudDocs.Models.Secretary;

namespace USVStudDocs.BLL.Services.CommonNumberService;

public interface ICommonNumberService
{
    CommonNumber GetLatest();

    CommonNumber GetToday();

    void Save(CommonNumber model);
}