const connection = new signalR.HubConnectionBuilder()
    .withUrl("/updateHub")
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("UpdateProfessionalAppointmentInfo", function (data) {
    $('#totalCompleteAppointment').text(data.totalCompleteAppointment);
    $('#totalMyAppointment').text(data.totalMyAppointment);
    $('#totalWaitAppointment').text(data.totalWaitAppointment);
    $('#totalPatient').text(data.totalPatient);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});
