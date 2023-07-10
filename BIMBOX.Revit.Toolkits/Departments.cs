using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Toolkit.Extension
{
    public class Departments
    {
        //功能用地-扩展
        public enum FunctionalArea
        {
            GeneralMedicalArea,
            InfectionMedicalArea,
            CleaningServiceArea,
            ContaminationServiceArea,
            HospitalLivingArea
        }

        public void AssignFunctionalArea(FunctionalArea area)
        {
            switch (area)
            {
                case FunctionalArea.GeneralMedicalArea:
                    Console.WriteLine("General Medical Area: Includes outpatient department, inpatient department, and medical technology functions. The outpatient department and inpatient department are located around the medical technology department.");
                    break;
                case FunctionalArea.InfectionMedicalArea:
                    Console.WriteLine("Infection Medical Area: Includes fever clinic, specialized infectious disease treatment areas, etc. It should be located downwind in the hospital area and have a certain distance from other areas.");
                    break;
                case FunctionalArea.CleaningServiceArea:
                    Console.WriteLine("Cleaning Service Area: Includes administrative offices, general services and logistics, research and training, nutrition kitchen, etc. It is generally located between the medical and living areas, facilitating two-way service. The nutrition kitchen should be close to the inpatient department or directly located within the inpatient department.");
                    break;
                case FunctionalArea.ContaminationServiceArea:
                    Console.WriteLine("Contamination Service Area: Includes laundry, animal laboratory, mortuary, sewage treatment station, etc. It should be located at the edge of the site.");
                    break;
                case FunctionalArea.HospitalLivingArea:
                    Console.WriteLine("Hospital Living Area: Includes shift staff dormitories, staff canteen, etc. It is adjacent to the cleaning service area and has separate entrances and exits for external access.");
                    break;
                default:
                    Console.WriteLine("Invalid functional area.");
                    break;
            }
        }

        public static void Main(string[] args)
        {
            Departments hospital = new Departments();

            hospital.AssignFunctionalArea(FunctionalArea.GeneralMedicalArea);
            hospital.AssignFunctionalArea(FunctionalArea.InfectionMedicalArea);
            hospital.AssignFunctionalArea(FunctionalArea.CleaningServiceArea);
            hospital.AssignFunctionalArea(FunctionalArea.ContaminationServiceArea);
            hospital.AssignFunctionalArea(FunctionalArea.HospitalLivingArea);
        }
    }
}

