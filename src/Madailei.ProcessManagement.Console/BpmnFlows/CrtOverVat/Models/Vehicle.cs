using System;
using System.Collections.Generic;
using System.Text;

namespace Madailei.ProcessManagement.Console.BpmnFlows.CrtOverVat.Models
{
    public class Vehicle
    {
        public string Version { get; set; }

        public string Vin { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string BodyType { get; set; }

        public int PowerKw { get; set; }

        public int PowerHp { get; set; }

        public int? PowerTorque { get; set; }

        public string Emission { get; set; }

        public string EmissionClass { get; set; }

        public string FuelType { get; set; }

        public string Transmission { get; set; }

        public int NumberOfGears { get; set; }

        public int? NumberOfDoors { get; set; }

        public int? NumberOfSeats { get; set; }

        public int EngineCapacity { get; set; }

        public int RegistrationYear { get; set; }

        public int RegistrationMonth { get; set; }

        public int ManufacturedYear { get; set; }

        public int ManufacturedMonth { get; set; }

        public string Drivetrain { get; set; }

        public string ExteriorColor { get; set; }
    }

}
