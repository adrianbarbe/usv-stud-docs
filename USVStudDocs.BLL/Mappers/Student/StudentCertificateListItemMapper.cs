using USVStudDocs.Entities;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;
using USVStudDocs.Models.Student;

namespace USVStudDocs.BLL.Mappers.Student;

public class StudentCertificateListItemMapper : IMapper<CertificateEntity, StudentCertificateListItem>
{
    private readonly IMapper<FacultyPersonEntity, FacultyPerson> _facultyPersonMapper;

    public StudentCertificateListItemMapper(IMapper<FacultyPersonEntity, FacultyPerson> facultyPersonMapper)
    {
        _facultyPersonMapper = facultyPersonMapper;
    }
    public CertificateEntity Map(StudentCertificateListItem source)
    {
        throw new NotImplementedException();
    }

    public StudentCertificateListItem Map(CertificateEntity source)
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
        
        return new StudentCertificateListItem
        {
            Id = source.Id,
            CertificateReason = source.CertificateReason,
            DenyReason = source.DenyReason,
            RegistrationDate = source.RegistrationDate,
            RegistrationNumber = source.RegistrationNumber,
            Secretary = _facultyPersonMapper.Map(source.Secretary),
            CertificateStatus = certificateStatus,
        };
    }
}