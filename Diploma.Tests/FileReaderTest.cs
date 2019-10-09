using Diploma.Files;
using Diploma.Validators;
using Diploma.Wrappers;
using System;
using Moq;
using Xunit;
using System.IO;
using Diploma.Models;
using System.Collections.Generic;

namespace Diploma.Tests
{
    public class FileReaderTest
    {
        [Fact]
        public void ImportData_ReturnsValidCollectionOfStudentRawModel()
        {
            var students = new List<StudentRawModel>()
            {
                new StudentRawModel{Nume = "Albu", Prenume = "Maria", LimbaRomana = 9.8, LimbaEngleza = 9.2, Matematica = 9.1, Fizica = 9.5, Chimia = 10,Informatica = 10, Geografia = 10, Istoria = 10, Biologia = 9.8},
                new StudentRawModel{Nume = "Badulescu" , Prenume = "Mihai", LimbaRomana = 10, LimbaEngleza = 8, Matematica = 8.6, Fizica = 9.8, Chimia = 9, Informatica = 9.7, Geografia = 9.8, Istoria = 8.7, Biologia =  8.9}
            };

            var iFileWrapper = new Mock<IFileWrapper>();

            string path = @"C:\Users\vpascal\.NET Projects\Unit Testing HW\DiplomaTestingKata\StudentCatalog.csv";

            iFileWrapper.Setup(a => a.OpenText(path)).Returns(File.OpenText(path));

            var fileReader = new FileReader(iFileWrapper.Object);

            var act = fileReader.ImportData(path);

            Assert.Equal(students.ToString(), act.ToString());
        }
    }
}
