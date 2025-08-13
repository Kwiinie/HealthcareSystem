document.addEventListener('DOMContentLoaded', function () {
    AddEventsPatientRecord();
})

function AddEventsPatientRecord() {
    const patientRecord = $('#patient-record');
    const patientRecordModal = $('#patientRecordModal');

    $('.custom-dropdown-toggle').on('click', function (e) {
        e.preventDefault();

        var $dropdown = $(this).closest('.custom-dropdown');
        var $menu = $dropdown.find('.custom-dropdown-menu');

        $('.custom-dropdown-menu.show').not($menu).removeClass('show');

        $menu.toggleClass('show');
    });
    $(document).on('click', function (e) {
        if (!$(e.target).closest('.custom-dropdown').length) {
            $('.custom-dropdown-menu.show').removeClass('show');
        }
    });

    patientRecord.on('click', function () {
        fetchAppointment($(this).data('id'), $(this).data('servicetype'));
    })

    async function fetchAppointment(id, type) {
        try {
            const params = new URLSearchParams({
                handler: 'Appointment',
                id: id,
                type: type
            });
            const response = await fetch(`?${params.toString()}`, {
                method: 'GET'
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            entity = data.appointment;
            setAppointmentDetail(entity);
            patientRecordModal.modal('show');
        } catch (err) {
            console.log(err);
        }
    }

    function setAppointmentDetail(entity) {
        entity = entity;
        idAppointment = entity.id;
        $('.patient-name').text(entity.patient.user.fullname);
        $('.patient-genderAge').text(entity.age + " tuổi" + " | " + entity.patient?.user?.gender);
        $('.patient-email').text(entity.patient?.user?.email);
        $('.patient-phone').text(entity.patient?.user?.phoneNumber);
        $('.patient-note').text(entity.patient?.note);
    }

}