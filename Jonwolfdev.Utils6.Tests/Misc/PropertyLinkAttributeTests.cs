using Jonwolfdev.Utils6.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jonwolfdev.Utils6.Tests.Misc
{
    public class PropertyLinkAttributeTests
    {
        [Fact]
        public void Should_Create_Class()
        {
            // act & assert
            new PropertyLinkAttribute(1);
        }

        [Fact]
        public void Should_Throw_ArgumentException_Invalid_Id()
        {
            // act & assert
            Assert.Throws<ArgumentException>(() => new PropertyLinkAttribute(-1));
        }
    }
}
