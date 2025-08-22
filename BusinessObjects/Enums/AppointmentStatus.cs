using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Enums
{
    public enum AppointmentStatus
    {
        Scheduled, //slot booked
        CheckedIn, //patient arrived at front desk
        InExam, //doctor started the encouter
        Completed, //confirmed by doctor
        CancelledByPatient, //patient cancels before the exam starts (at least 2 hours)
        CancelledByDoctor,
        NoShow, //auto-marked if not checked in (after expected time 15min)
    }
}
