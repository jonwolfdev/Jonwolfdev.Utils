﻿using Jonwolfdev.Utils.Common.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jonwolfdev.Utils.Common.UnitTests.Validation
{
    public class ObjectValidatorTests
    {
        [Fact]
        public void ObjectValidator_ShouldValidateListItems_ThrowsException()
        {
            //Arrange
            var list = new List<ObjectToValidate>()
            {
                new ObjectToValidate()
            };
            //Act

            void act() { ObjectValidator.ValidateObject(list, nameof(list)); }

            //Assert
            var exception = Assert.Throws<ArgumentException>(act);
            Assert.Contains(nameof(ObjectToValidate.String), exception.Message);
            Assert.Contains(nameof(ObjectToValidate.ObjectsToValidate), exception.Message);
        }

        [Fact]
        public void ObjectValidator_NullObjectParam_ThrowsException()
        {
            //Arrange
            List<ObjectToValidate> list = null;
            //Act

            void act() { ObjectValidator.ValidateObject(list, nameof(list)); }

            //Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ObjectValidator_ShouldValidateListItems_NoErrors()
        {
            //Arrange
            var list = new List<ObjectToValidate>()
            {
                new ObjectToValidate()
                {
                    String = "str",
                    ObjectsToValidate = new List<ObjectToValidate>()
                }
            };
            //Act

            ObjectValidator.ValidateObject(list, nameof(list));

            //Assert
        }

        [Fact]
        public void ObjectValidator_ShouldValidateListItemsMinLength_ThrowsException()
        {
            //Arrange
            var list = new List<ObjectToValidate>()
            {
                new ObjectToValidate()
                {
                    String = "str",
                    ObjectsToValidate = new List<ObjectToValidate>(),
                    ObjectsToValidateRange = new List<ObjectToValidate>()
                }
            };
            //Act

            void act() => ObjectValidator.ValidateObject(list, nameof(list));

            //Assert
            var exception = Assert.Throws<ArgumentException>(act);
            Assert.Contains(nameof(ObjectToValidate.ObjectsToValidateRange), exception.Message);
            Assert.Contains("minimum", exception.Message);
        }

        [Fact]
        public void ObjectValidator_ShouldValidateMaxLength_ThrowsException()
        {
            //Arrange
            var list = new List<ObjectToValidate>()
            {
                new ObjectToValidate()
                {
                    String = "str_longer_than_allowed",
                    ObjectsToValidate = new List<ObjectToValidate>()
                }
            };
            //Act

            void act() => ObjectValidator.ValidateObject(list, nameof(list));

            //Assert
            var exception = Assert.Throws<ArgumentException>(act);
            Assert.Contains(nameof(ObjectToValidate.String), exception.Message);
            Assert.Contains("maximum", exception.Message);
        }
    }
}
