using ClassLibrary;
using NUnit.Framework;
using System.Collections.Generic;

 [TestFixture]
public class ProductoTest
{
    /// <summary>
    /// Test para la clase Producto, creo instancia de Producto y verifico que sus campos sean correctos
    /// </summary>
    /// <param name="Test""></param>
    /// <param name="Test""></param>
    /// <returns></returns>
    static Ubicacion ubicacionTest = new Ubicacion("Ciudad Test", "333 Test");
    /// <summary>
    /// instancia de ubicacion que le paso a producto para testear
    /// </summary>
   
    static Material materialTest = new Material("cobre",new List<string>(){"cat01","cat02","cat03"},"Kilogramos");
    /// <summary>
    /// instancia de material que le paso a producto para testear
    /// </summary>
   static Producto productoTest = new Producto("productoTest",materialTest,ubicacionTest,30,4396,100);
    /// <summary>
    /// clase de producto que testeo
    /// </summary>
    
    [Test]
    public void TestProducto()
    {
        ///<summary>
        /// las las clases de producto
        /// </summary>
        Assert.AreEqual("productoTest",productoTest.Nombre);
        Assert.AreEqual(materialTest,productoTest.Material);
        Assert.AreEqual(ubicacionTest,productoTest.Ubicacion);
        Assert.AreEqual(30,productoTest.CantidadEnUnidad);
        Assert.AreEqual(4396,productoTest.ValorUYU);
        Assert.AreEqual(100,productoTest.ValorUSD);
    }
}