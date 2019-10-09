using Diploma.DataProcessing;
using Diploma.Files;
using Diploma.Models;
using Diploma.Tests.Builder;
using Diploma.Utils;
using Diploma.Validators;
using GemBox.Document;
////using NSubstitute;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Diploma.Tests.UnitTests
{
    public class StudentDataProcessorTests
    {

        [Fact]
        public void LoadData_CallsImportData_OneTimeWithPath()
        {
            string path = "path";

            Mock <IFileReader> fileReader = new Mock<IFileReader>();
            Mock<ICustomMapper> mapper = new Mock<ICustomMapper>();
            Mock<IFileWriter> fileWriter = new Mock<IFileWriter>();
            Mock<IValidator> validator = new Mock<IValidator>();

            StudentDataProcessor studentDataProcessor = new StudentDataProcessor(fileReader.Object, mapper.Object, fileWriter.Object, validator.Object);

            studentDataProcessor.LoadData(path);

            fileReader.Verify(x => x.ImportData(path), Times.Once);
        }

        [Fact]
        public void LoadData_CallsMapToStudent_WithStudentsFromImportData()
        {
            string path = "path";

            Mock<IFileReader> fileReader = new Mock<IFileReader>();
            Mock<ICustomMapper> mapper = new Mock<ICustomMapper>();
            Mock<IFileWriter> fileWriter = new Mock<IFileWriter>();
            Mock<IValidator> validator = new Mock<IValidator>();

            StudentDataProcessor studentDataProcessor = new StudentDataProcessor(fileReader.Object, mapper.Object, fileWriter.Object, validator.Object);

            fileReader.Setup(s => s.ImportData(path)).Returns(new List<StudentRawModel>() { new StudentRawModel() });

            studentDataProcessor.LoadData(path);

            mapper.Verify(x => x.MapToStudent(It.IsAny<IEnumerable<StudentRawModel>>()), Times.Once);
        }

        [Fact]
        public void LoadData_CallsValidateStudentRecord_WithStudentFromImportData()
        {
            string path = "path";

            Mock<IFileReader> fileReader = new Mock<IFileReader>();
            Mock<ICustomMapper> mapper = new Mock<ICustomMapper>();
            Mock<IFileWriter> fileWriter = new Mock<IFileWriter>();
            Mock<IValidator> validator = new Mock<IValidator>();

            StudentDataProcessor studentDataProcessor = new StudentDataProcessor(fileReader.Object, mapper.Object, fileWriter.Object, validator.Object);

            var finalStudents = new List<StudentModel>()
            {
                new StudentModel (), new StudentModel()
            };

            mapper.Setup(x => x.MapToStudent(It.IsAny<IEnumerable<StudentRawModel>>())).Returns(finalStudents);

            studentDataProcessor.LoadData(path);

            validator.Verify(v => v.ValidateStudentRecord(It.IsAny<StudentModel>()), Times.Exactly(finalStudents.Count));
        }

        [Fact]
        public void LoadData_CallsCreateDiplomas_WithStudentFromImportData_IfValidateStudentRecordReturnsTrue()
        {
            string path = "path";

            Mock<IFileReader> fileReader = new Mock<IFileReader>();
            Mock<ICustomMapper> mapper = new Mock<ICustomMapper>();
            Mock<IFileWriter> fileWriter = new Mock<IFileWriter>();
            Mock<IValidator> validator = new Mock<IValidator>();

            StudentDataProcessor studentDataProcessor = new StudentDataProcessor(fileReader.Object, mapper.Object, fileWriter.Object, validator.Object);

            var finalStudents = new List<StudentModel>()
            {
                new StudentModel (), new StudentModel(), new StudentModel()
            };

            mapper.Setup(x => x.MapToStudent(It.IsAny<IEnumerable<StudentRawModel>>())).Returns(finalStudents);
            validator.Setup(v => v.ValidateStudentRecord(finalStudents[0])).Returns(true);
            validator.Setup(v => v.ValidateStudentRecord(finalStudents[1])).Returns(true);
            validator.Setup(v => v.ValidateStudentRecord(finalStudents[2])).Returns(false);

            studentDataProcessor.LoadData(path);

            fileWriter.Verify(f => f.CreateDiplomas(It.IsAny<StudentModel>()), Times.Exactly(2));
        }

        [Fact]
        public void LoadData_DoesntCallsCreateDiplomas__IfValidateStudentRecordReturnsFalse()
        {
            string path = "path";

            Mock<IFileReader> fileReader = new Mock<IFileReader>();
            Mock<ICustomMapper> mapper = new Mock<ICustomMapper>();
            Mock<IFileWriter> fileWriter = new Mock<IFileWriter>();
            Mock<IValidator> validator = new Mock<IValidator>();

            StudentDataProcessor studentDataProcessor = new StudentDataProcessor(fileReader.Object, mapper.Object, fileWriter.Object, validator.Object);

            var finalStudents = new List<StudentModel>()
            {
                new StudentModel (), new StudentModel(), new StudentModel()
            };

            mapper.Setup(x => x.MapToStudent(It.IsAny<IEnumerable<StudentRawModel>>())).Returns(finalStudents);
            validator.Setup(v => v.ValidateStudentRecord(finalStudents[0])).Returns(false);
            validator.Setup(v => v.ValidateStudentRecord(finalStudents[1])).Returns(false);
            validator.Setup(v => v.ValidateStudentRecord(finalStudents[2])).Returns(false);

            studentDataProcessor.LoadData(path);

            fileWriter.Verify(f => f.CreateDiplomas(It.IsAny<StudentModel>()), Times.Exactly(0));
        }

        [Fact]
        public void LoadData_CallsSaveDiplomas_WithStudentFromImportData_IfValidateStudentRecordReturnsTrue()
        {
            string path = "path";

            Mock<IFileReader> fileReader = new Mock<IFileReader>();
            Mock<ICustomMapper> mapper = new Mock<ICustomMapper>();
            Mock<IFileWriter> fileWriter = new Mock<IFileWriter>();
            Mock<IValidator> validator = new Mock<IValidator>();

            StudentDataProcessor studentDataProcessor = new StudentDataProcessor(fileReader.Object, mapper.Object, fileWriter.Object, validator.Object);

            var finalStudents = new List<StudentModel>()
            {
                new StudentModel (), new StudentModel(), new StudentModel()
            };

            mapper.Setup(x => x.MapToStudent(It.IsAny<IEnumerable<StudentRawModel>>())).Returns(finalStudents);
            validator.Setup(v => v.ValidateStudentRecord(finalStudents[0])).Returns(true);
            validator.Setup(v => v.ValidateStudentRecord(finalStudents[1])).Returns(true);
            validator.Setup(v => v.ValidateStudentRecord(finalStudents[2])).Returns(false);

            studentDataProcessor.LoadData(path);

            fileWriter.Verify(f => f.SaveDiploma(It.IsAny<DocumentModel>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void LoadData_DoesntCallSaveDiploma_IfValidateStudentRecordReturnsFalse()
        {
            string path = "path";

            Mock<IFileReader> fileReader = new Mock<IFileReader>();
            Mock<ICustomMapper> mapper = new Mock<ICustomMapper>();
            Mock<IFileWriter> fileWriter = new Mock<IFileWriter>();
            Mock<IValidator> validator = new Mock<IValidator>();

            StudentDataProcessor studentDataProcessor = new StudentDataProcessor(fileReader.Object, mapper.Object, fileWriter.Object, validator.Object);

            var finalStudents = new List<StudentModel>()
            {
                new StudentModel (), new StudentModel(), new StudentModel()
            };

            mapper.Setup(x => x.MapToStudent(It.IsAny<IEnumerable<StudentRawModel>>())).Returns(finalStudents);
            validator.Setup(v => v.ValidateStudentRecord(finalStudents[0])).Returns(false);
            validator.Setup(v => v.ValidateStudentRecord(finalStudents[1])).Returns(false);
            validator.Setup(v => v.ValidateStudentRecord(finalStudents[2])).Returns(false);

            studentDataProcessor.LoadData(path);

            fileWriter.Verify(f => f.SaveDiploma(It.IsAny<DocumentModel>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(0));
        }

        //[Fact]
        //public void LoadData_CallsSaveDiplomaOneTime_IfCreateDiplomaIsCalled()
        //{
        //    string path = "path";

        //    Mock<IFileReader> fileReader = new Mock<IFileReader>();
        //    Mock<ICustomMapper> mapper = new Mock<ICustomMapper>();
        //    Mock<IFileWriter> fileWriter = new Mock<IFileWriter>();
        //    Mock<IValidator> validator = new Mock<IValidator>();

        //    StudentDataProcessor studentDataProcessor = new StudentDataProcessor(fileReader.Object, mapper.Object, fileWriter.Object, validator.Object);

        //    var finalStudents = new List<StudentModel>()
        //    {
        //        new StudentModel (), new StudentModel(), new StudentModel()
        //    };

        //    mapper.Setup(x => x.MapToStudent(It.IsAny<IEnumerable<StudentRawModel>>())).Returns(finalStudents);
        //    validator.Setup(v => v.ValidateStudentRecord(finalStudents[0])).Returns(false);
        //    validator.Setup(v => v.ValidateStudentRecord(finalStudents[1])).Returns(false);
        //    validator.Setup(v => v.ValidateStudentRecord(finalStudents[2])).Returns(false);
        //    fileWriter.Setup(f => f.CreateDiplomas(It.IsAny<StudentModel>()));

        //    studentDataProcessor.LoadData(path);

        //    fileWriter.Verify(f => f.SaveDiploma(It.IsAny<DocumentModel>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
        //}
       
    }
}