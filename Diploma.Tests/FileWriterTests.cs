using Diploma.Files;
using Diploma.Models;
using Diploma.Utils;
using Diploma.Wrappers;
using GemBox.Document;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Diploma.Tests
{
    public class FileWriterTests
    {
        private StudentModel GetSampleStudentModel()
        {
            var sampleStudentModel = new StudentModel
            {
                FirstName = "Vasily", LastName = "Pascal", Grades = new List<Subject>
                { new Subject { SubjectName = "Math", Grade = 7 }, new Subject { SubjectName = "Chemistry", Grade = 9 } }
            };
            return sampleStudentModel;
            }

        [Fact]
        public void CreateDiplomas_GetFirstParargaphCalledOnce()
        {
            Mock<IDocWrapper> pdfWrapper = new Mock<IDocWrapper>();
            Mock<IStringCreator> stringCreator = new Mock<IStringCreator>();

            FileWriter sut = new FileWriter(pdfWrapper.Object, stringCreator.Object);

            var student = GetSampleStudentModel();

            sut.CreateDiplomas(student);

            stringCreator.Verify(s => s.GetFirstParagraph(student.LastName, student.FirstName), Times.Once);
        }

        [Fact]
        public void CreateDiplomas_GetSecondParargaphTextCalledOnce()
        {
            Mock<IDocWrapper> pdfWrapper = new Mock<IDocWrapper>();
            Mock<IStringCreator> stringCreator = new Mock<IStringCreator>();

            FileWriter sut = new FileWriter(pdfWrapper.Object, stringCreator.Object);

            var student = GetSampleStudentModel();

            sut.CreateDiplomas(student);

            stringCreator.Verify(t => t.GetSecondParagraphText(student.Grades), Times.Once);
        }

        //[Fact]
        //public void CreateDiplomas_ReturnsDocumentModel()
        //{
        //    ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        //    Mock<IDocWrapper> pdfWrapper = new Mock<IDocWrapper>();
        //    Mock<IStringCreator> stringCreator = new Mock<IStringCreator>();

        //    FileWriter sut = new FileWriter(pdfWrapper.Object, stringCreator.Object);

        //    var student = GetSampleStudentModel();
        //    var expectedDocument = new DocumentModel();

        //    string firstParagraphText = "Hello";
        //    var secondParagraphText = "World";

        //    stringCreator.Setup(s => s.GetFirstParagraph(It.IsAny<string>(), It.IsAny<string>())).Returns(firstParagraphText);
        //    stringCreator.Setup(s => s.GetSecondParagraphText(It.IsAny<IEnumerable<Subject>>())).Returns(secondParagraphText);



        //    expectedDocument.Sections.Add(new Section(expectedDocument,
        //        new List<Paragraph>() { new Paragraph(expectedDocument, firstParagraphText), new Paragraph(expectedDocument, secondParagraphText) }));

        //    var actual = sut.CreateDiplomas(student);

        //    Assert.Equal(expectedDocument, actual);
        //}

        [Fact]
        public void SaveDiploma_CreathPath_CallOnce()
        {
            Mock<IDocWrapper> pdfWrapper = new Mock<IDocWrapper>();
            Mock<IStringCreator> stringCreator = new Mock<IStringCreator>();

            FileWriter sut = new FileWriter(pdfWrapper.Object, stringCreator.Object);

            sut.SaveDiploma(It.IsAny<DocumentModel>(), It.IsAny<string>(), It.IsAny<string>());

            stringCreator.Verify(s => s.CreatePath(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SaveDiploma_SaveDocument_CallOnce()
        {
            Mock<IDocWrapper> pdfWrapper = new Mock<IDocWrapper>();
            Mock<IStringCreator> stringCreator = new Mock<IStringCreator>();

            FileWriter sut = new FileWriter(pdfWrapper.Object, stringCreator.Object);

            sut.SaveDiploma(It.IsAny<DocumentModel>(), It.IsAny<string>(), It.IsAny<string>());
            pdfWrapper.Verify(d => d.SaveDocument(It.IsAny<DocumentModel>(), It.IsAny<string>()), Times.Once());
        }


    }
}
