using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Enums
{
    public enum AppointmentStatus
    {
        AwaitingPayment, //waiting for payment
        Pending, //after paying successfully
        Expired, //after 30 mins no payment
        Confirmed, //professional confirms
        Rescheduled, //patient rescheduled before 2 days of appointment 
        Cancelled, //patient cancelled
        Rejected, //professional rejects
        Completed, //professional confirm after appointment
        None,
    }
}
