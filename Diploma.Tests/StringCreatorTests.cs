using Diploma.Models;
using Diploma.Utils;
using Diploma.Wrappers;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Diploma.Tests
{
    public class StringCreatorTests
    {

        [Theory]
        [InlineData("Vasily","Pascal")]
        [InlineData("Vasily", "")]
        [InlineData("", "")]    
        public void CreatePath_ReturnsValidPath_WithValidInputName(string name, string lastName)
        {
            string path = "path";
            string extension = ".docx";

            Mock<IConstants> constants = new Mock<IConstants>();
            Mock<IDomainWrapper> domainWrapper = new Mock<IDomainWrapper>();
          
            StringCreator sut = new StringCreator(constants.Object, domainWrapper.Object);

            domainWrapper.Setup(d => d.GetRoothDirectory()).Returns(path);
            constants.Setup(c => c.GetExtension).Returns(extension);

            var expected = path + name + lastName + extension;
            var actual = sut.CreatePath(name, lastName);

            domainWrapper.Verify(d => d.GetRoothDirectory(), Times.Once);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Vasily", "Pascal")]
        [InlineData("Vasily", "")]
        [InlineData("", "")]
        public void GetFirstParagraph_ReturnsValidParagraph_WithValidInputName(string name, string lastName)
        {
            string diplomaNameText = "Diploma \n\n Studii Medii";

            Mock<IConstants> constants = new Mock<IConstants>();
            Mock<IDomainWrapper> domainWrapper = new Mock<IDomainWrapper>();

            StringCreator sut = new StringCreator(constants.Object, domainWrapper.Object);

            constants.Setup(c => c.GetDiplomNameText).Returns(diplomaNameText);

            var expected = diplomaNameText + "\n" + name + " " + lastName;
            var actual = sut.GetFirstParagraph(name, lastName);

            constants.Verify(c => c.GetDiplomNameText, Times.Once);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSecondParagraphText_ReturnsValidText_WithValidInputCollectionOfGrades()
        {
            var grades = new List<Subject>()
            {
                new Subject() { SubjectName = "Math", Grade = 13 }, new Subject() { SubjectName = "Chemistry", Grade = 9 }
            };

            Mock<IConstants> constants = new Mock<IConstants>();
            Mock<IDomainWrapper> domainWrapper = new Mock<IDomainWrapper>();

            StringCreator sut = new StringCreator(constants.Object, domainWrapper.Object);

            var expected = grades[0].SubjectName + " " + grades[0].Grade + "\n" + grades[1].SubjectName + " " + grades[1].Grade + "\n";
            var actual = sut.GetSecondParagraphText(grades);

            Assert.Equal(expected, actual);
        }
    }
}
