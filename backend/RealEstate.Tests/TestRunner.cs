using NUnit.Framework;

namespace RealEstate.Tests
{
    [SetUpFixture]
    public class TestRunner
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            Console.WriteLine("========================================****");
            Console.WriteLine("INICIANDO SUITE DE PRUEBAS UNITARIAS");
            Console.WriteLine("Real Estate API - Tests");
            Console.WriteLine("========================================");
            
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            Console.WriteLine("========================================****");
            Console.WriteLine("FINALIZANDO SUITE DE PRUEBAS");
            Console.WriteLine("========================================");            
        }
    }
}