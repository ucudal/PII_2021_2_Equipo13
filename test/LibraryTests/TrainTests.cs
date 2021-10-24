//--------------------------------------------------------------------------------
// <copyright file="TrainTests.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//--------------------------------------------------------------------------------

using ClassLibrary;
using NUnit.Framework;

namespace Tests
{
    /// <summary>
    /// Prueba de la clase <see cref="Train"/>.
    /// </summary>
    [TestFixture]
    public class TrainTests
    {
        /// <summary>
        /// El tren para probar.
        /// </summary>
        private Train train;

        /// <summary>
        /// Crea un tren para probar.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.train = new Train();
        }

        /// <summary>
        /// Prueba que el tren arranque.
        /// </summary>
        [Test]
        public void StartTrainTest()
        {
            Assert.NotNull(this.train);
            this.train.StartEngines();
            Assert.True(this.train.IsEngineStarted);
        }

        /// <summary>
        /// Prueba que el tren se detenga.
        /// </summary>
        [Test]
        public void StopTrainTest()
        {
            Assert.NotNull(this.train);
            this.train.StartEngines();
            this.train.StopEngines();
            Assert.False(this.train.IsEngineStarted);
        }
    }
}