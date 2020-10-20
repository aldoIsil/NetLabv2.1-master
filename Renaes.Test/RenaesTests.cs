using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Renaes.Test
{
    [TestClass]
    public class RenaesTests
    {
        [TestMethod()]
        public void getEstablecimiento_Found()
        {
            var proxy = new RenaesProxy();

            var establecimiento = proxy.getEstablecimiento("6623");

            Assert.IsNotNull(establecimiento);
        }

        [TestMethod()]
        public void getEstablecimiento_NotFound()
        {
            var proxy = new RenaesProxy();

            var establecimiento = proxy.getEstablecimiento("190999");

            Assert.IsNull(establecimiento);
        }
    }
}
