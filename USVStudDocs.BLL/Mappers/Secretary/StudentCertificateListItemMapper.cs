using USVStudDocs.Entities;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;
using USVStudDocs.Models.Secretary;
using USVStudDocs.Models.Student;

namespace USVStudDocs.BLL.Mappers.Secretary;

public class SecretaryCertificateListItemMapper : IMapper<CertificateEntity, SecretaryCertificateListItem>
{
    private readonly IMapper<FacultyPersonEntity, FacultyPerson> _facultyPersonMapper;
    private readonly IMapper<StudentEntity, Models.Admin.Student> _studentMapper;

    public SecretaryCertificateListItemMapper(IMapper<FacultyPersonEntity, 
        FacultyPerson> facultyPersonMapper,
        IMapper<StudentEntity, Models.Admin.Student> studentMapper)
    {
        _facultyPersonMapper = facultyPersonMapper;
        _studentMapper = studentMapper;
    }
    public CertificateEntity Map(SecretaryCertificateListItem source)
    {
        throw new NotImplementedException();
    }

    public SecretaryCertificateListItem Map(CertificateEntity source)
    {
        CertificateStatus certificateStatus = CertificateStatus.New;

        switch (source.Status)
        {
            case Entities.Constants.CertificateStatus.New:
                certificateStatus = CertificateStatus.New;
                break;
            
            case Entities.Constants.CertificateStatus.Approved:
                certificateStatus = CertificateStatus.Approved;
                break;
            
            case Entities.Constants.CertificateStatus.Denied:
                certificateStatus = CertificateStatus.Denied;
                break;
            
            case Entities.Constants.CertificateStatus.Printed:
                certificateStatus = CertificateStatus.Printed;
                break;
            
            case Entities.Constants.CertificateStatus.Signed:
                certificateStatus = CertificateStatus.Signed;
                break;
        }
        
        return new SecretaryCertificateListItem
        {
            Id = source.Id,
            CertificateReason = source.CertificateReason,
            DenyReason = source.DenyReason,
            RegistrationDate = source.RegistrationDate,
            ApprovedDate = source.ApprovedDate,
            RegistrationNumber = source.RegistrationNumber,
            Secretary = _facultyPersonMapper.Map(source.Secretary),
            CertificateStatus = certificateStatus,
            Student = _studentMapper.Map(source.Student),
        };
    }
}