using RealEstate.Domain.Entities;
using MongoDB.Driver;

namespace RealEstate.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(MongoDbContext context)
        {
            // Verificar si ya hay datos AAB (20 08 2025)
            var ownerCount = await context.Owners.CountDocumentsAsync(_ => true);
            if (ownerCount > 0) return;

            // ejemplo AAB
            Console.WriteLine("Creando datos de ejemplo AAB...");
            var owners = new List<Owner>
            {
                new Owner
                {
                    Name = "Juan Pérez",
                    Address = "Calle algo 123 #45-67, Bogotá",
                    Phone = "+57 300 123 4567",
                    Birthday = new DateTime(1980, 5, 15)
                },
                new Owner
                {
                    Name = "María García",
                    Address = "Carrera no se 50 #30-20, Medellin",
                    Phone = "+57 301 987 6543",
                    Birthday = new DateTime(1975, 8, 22)
                },
                new Owner
                {
                    Name = "Carlos Rodríguez",
                    Address = "Avenida 80 #25-40, Cali",
                    Phone = "+57 302 456 7890",
                    Birthday = new DateTime(1985, 12, 3)
                }
            };

            await context.Owners.InsertManyAsync(owners);

            //  ejemplo AAB (20 08 2025)
            var properties = new List<Property>
            {
                new Property
                {
                    Name = "Apartamento Moderno Chapinero",
                    Address = "Carrera 13 #85-40, Chapinero, Bogotá",
                    Price = 450000000m,
                    IdOwner = owners[0].Id,
                    Image = "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=500",
                    Year = 2020,
                    CodeInternal = 100001
                },
                new Property
                {
                    Name = "Casa Campestre Envigado",
                    Address = "Calle 25 Sur #48-30, Envigado",
                    Price = 800000000m,
                    IdOwner = owners[1].Id,
                    Image = "https://images.unsplash.com/photo-1570129477492-45c003edd2be?w=500",
                    Year = 2018,
                    CodeInternal = 100002
                },
                new Property
                {
                    Name = "Oficina Centro Empresarial",
                    Address = "Avenida El Poblado #10-32, Medellin",
                    Price = 300000000m,
                    IdOwner = owners[1].Id,
                    Image = "https://images.unsplash.com/photo-1497366216548-37526070297c?w=500",
                    Year = 2019,
                    CodeInternal = 100003
                },
                new Property
                {
                    Name = "Penthouse Vista al Mar",
                    Address = "Bocagrande, Cartagena",
                    Price = 1200000000m,
                    IdOwner = owners[2].Id,
                    Image = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=500",
                    Year = 2021,
                    CodeInternal = 100004
                },
                new Property
                {
                    Name = "Casa Familiar Zona Norte",
                    Address = "Calle 170 #45-20, Bogotá",
                    Price = 620000000m,
                    IdOwner = owners[0].Id,
                    Image = "https://images.unsplash.com/photo-1558618047-3c8c76ca7d13?w=500",
                    Year = 2017,
                    CodeInternal = 100005
                }
            };

            await context.Properties.InsertManyAsync(properties);

            Console.WriteLine("Datos de ejemplo creados exitosamente AAB (20 08 2025)");
        }
    }
}