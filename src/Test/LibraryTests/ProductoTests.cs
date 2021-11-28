using PII_E13.ClassLibrary;
using NUnit.Framework;
using System.Collections.Generic;

/// <summary>
/// Test para la clase Producto, creo instancia de Producto y verifico que sus campos sean correctos
/// </summary>
[TestFixture]
public class ProductoTests
{
    /// <summary>
    /// Instancia de Ubicacion usada en los tests.
    /// </summary>
    static IUbicacion ubicacionTest = new UbicacionBase("Ciudad Test", "333 Test");

    /// <summary>
    /// Instancia de Material que le paso a producto para testear.
    /// </summary>
    static Material materialTest = new Material("cobre", "Kilogramos", new List<string>() { "cat01", "cat02", "cat03" });

    /// <summary>
    /// Instancia de Producto que testeo.
    /// </summary>
    static Producto productoTest = new Producto(materialTest, "Ciudad Test", "333 Test", 30, 4396, 100);

    /// <summary>
    /// Testeo las características de producto.
    /// </summary>
    [Test]
    public void TestProducto()
    {
        Assert.AreSame(materialTest, productoTest.Material);
        Assert.AreEqual(ubicacionTest.Direccion, productoTest.Ubicacion.Direccion);
        Assert.AreEqual(ubicacionTest.Ciudad, productoTest.Ubicacion.Ciudad);
        Assert.AreEqual(30, productoTest.CantidadEnUnidad);
        Assert.AreEqual(4396, productoTest.ValorUYU);
        Assert.AreEqual(100, productoTest.ValorUSD);
    }
}