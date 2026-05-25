
using UnityEngine;

namespace _project.Scripts.Services.Input
{
    using UnityEngine;

    public class AccelerometerInput
    {
        public AccelerometerInput()
        {
            Input.gyro.enabled = true;
        }

        public Quaternion Rotation
        {
            get
            {
                var q = Input.gyro.attitude;

                return new Quaternion(
                    q.x,
                    q.y,
                    -q.z,
                    -q.w);
            }
        }
    }
}