using SharpBrick.PoweredUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XboxLegoController
{
    class TopGearRallyCar
    {
        private TechnicMediumHub Hub;
        private TechnicLargeLinearMotor SteeringMotor;
        private TechnicXLargeLinearMotor DriveMotor;

        public TopGearRallyCar(TechnicMediumHub hub)
        {
            Hub = hub;
        }

        public async Task InitializeAsync()
        {
            await Hub.RgbLight.SetRgbColorsAsync(0xff, 0x00, 0x00);
            await Hub.VerifyDeploymentModelAsync(modelBuilder => modelBuilder
                .AddHub<TechnicMediumHub>(hubBuilder => hubBuilder
                    .AddDevice<TechnicLargeLinearMotor>(Hub.B)
                    .AddDevice<TechnicXLargeLinearMotor>(Hub.D)
                )
            );
            SteeringMotor = Hub.B.GetDevice<TechnicLargeLinearMotor>();
            DriveMotor = Hub.D.GetDevice<TechnicXLargeLinearMotor>();
            await Hub.RgbLight.SetRgbColorsAsync(0x00, 0xff, 0x00);
        }

        public async Task SetSteeringDegrees(int degrees)
        {
            await SteeringMotor.GotoPositionAsync(degrees, 10, 100, SpecialSpeed.Brake);
        }

        public async Task SetDriveSpeed(int speed)
        {
            await DriveMotor.StartSpeedAsync((sbyte)speed, 100, SpeedProfiles.None);
        }
    }
}
