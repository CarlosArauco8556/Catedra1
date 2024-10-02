using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Catedra1.src.Data;
using Catedra1.src.Models;
namespace Catedra1.src.Data
{
    public class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDBContext>();
                
                var existingRuts = new HashSet<string>();
                if(!context.Users.Any())
                {
                    var userFaker = new Faker<User>()
                        .RuleFor(u => u.Rut, f => GenerateUniqueRandomRut(existingRuts))
                        .RuleFor(u => u.Name, f => f.Person.FullName)
                        .RuleFor(u => u.CorreoElectronico, f => f.Person.Email)
                        .RuleFor(u => u.Genero, f => f.PickRandom(new[] { "masculino", "femenino", "otro", "prefiero no decirlo" }))
                        .RuleFor(u => u.FechaNacimiento, f => f.Date.Past(f.Random.Number(18, 65)));
                    var users = userFaker.Generate(10);
                    context.Users.AddRange(users);
                    context.SaveChanges();
                }
                context.SaveChanges();
            }
        }
        private static string GenerateUniqueRandomRut(HashSet<string> existingRuts)
        {
            string rut;
            do
            {
                rut = GenerateRandomRut();
            } while (existingRuts.Contains(rut));
            existingRuts.Add(rut);
            return rut;
        }
        private static string GenerateRandomRut()
        {
            Random random = new();
            int rutNumber = random.Next(1, 99999999); // Número aleatorio de 7 o 8 dígitos
            int verificator = CalculateRutVerification(rutNumber);
            string verificatorStr = verificator.ToString();
            if(verificator == 10){
                verificatorStr = "k";
            }
            return $"{rutNumber}-{verificatorStr}";
        }
        private static int CalculateRutVerification(int rutNumber)
        {
            int[] coefficients = { 2, 3, 4, 5, 6, 7 };
            int sum = 0;
            int index = 0;
            while (rutNumber != 0)
            {
                sum += rutNumber % 10 * coefficients[index];
                rutNumber /= 10;
                index = (index + 1) % 6;
            }
            int verification = 11 - (sum % 11);
            return verification == 11 ? 0 : verification;
        }
    }
}