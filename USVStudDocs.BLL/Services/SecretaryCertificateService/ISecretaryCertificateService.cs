using ePlato.CoreApp.Models.Shared.DataGrid;
using USVStudDocs.Models.Constants;
using USVStudDocs.Models.Secretary;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.BLL.Services.SecretaryCertificateService;

public interface ISecretaryCertificateService
{
    SecretaryCertificatePrint GetPrint(int id);

    List<SecretaryCertificateListItem> GetArchive();
    DataGridModel<SecretaryCertificateListItem> GetList(RequestQueryModel requestQueryModel, CertificateStatus status);
    SecretaryCertificateSummary GetCount();
    void ConfirmItem(int id);
    void ConfirmPrint(int id);
    void ConfirmSignature(int id);
    void RejectItem(int id, SecretaryCertificateUpdateItem item);
}