using Diploma.Models;
using Diploma.Validators;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;

namespace Diploma.Tests
{
    public class ValidatorTests
    {
        [Fact]
        public void ValidateStudentRecord_EmptyName_ReturnsFalse()
        {
            var validator = new Validator();

            var studentModel = new StudentModel();

            var actual = validator.ValidateStudentRecord(studentModel);

            actual.Should().Be(false);
        }

        [Fact]
        public void ValidateStudentRecord_EmptySubject_ReturnsFalse()
        {
            var validator = new Validator();

            var studentModel = new StudentModel { FirstName = "Albu", LastName = "Maria", Grades = new List<Subject>() { new Subject() } };

            var actual = validator.ValidateStudentRecord(studentModel);

            actual.Should().Be(false);
        }

        [Fact]
        public void ValidateStudentRecord_InvalidSubjectNameValidGrades_ReturnsFalse()
        {
            var validator = new Validator();

            var studentModel = new StudentModel { FirstName = "Albu", LastName = "Maria", Grades = new List<Subject>() { new Subject() { SubjectName = "", Grade = 9 } } };

            var actual = validator.ValidateStudentRecord(studentModel);

            actual.Should().Be(false);
        }

        [Fact]
        public void ValidateStudentRecord_ValidSubjectNameInvalidGrades_ReturnsFalse()
        {
            var validator = new Validator();

            var studentModel = new StudentModel { FirstName = "Albu", LastName = "Maria", Grades = new List<Subject>() { new Subject() { SubjectName = "Math", Grade = 13 } } };

            var actual = validator.ValidateStudentRecord(studentModel);

            actual.Should().Be(false);
        }

        [Fact]
        public void ValidateStudentRecord_ValidStudentData_ReturnsTrue()
        {
            var validator = new Validator();

            var studentModel = new StudentModel { FirstName = "Albu", LastName = "Maria", Grades = new List<Subject>() { new Subject() { SubjectName = "Math", Grade = 9 }, new Subject() { SubjectName = "Chemistry", Grade = 7 } } };

            var actual = validator.ValidateStudentRecord(studentModel);

            actual.Should().Be(true);
        }

    }
}