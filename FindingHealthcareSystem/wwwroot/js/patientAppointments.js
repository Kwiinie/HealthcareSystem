
document.addEventListener('DOMContentLoaded', function () {
    AddEvents();
})

function AddEvents() {
    const appointmentStatuses = {
        0: "AwaitingPayment",
        1: "Pending",
        2: "Expired",
        3: "Confirmed",
        4: "Rescheduled",
        5: "Cancelled",
        6: "Rejected",
        7: "Completed"
    };

    let appointmentModal = $('#appointmentModal').modal();
    let idAppointment = null;
    let divSlots = [];
    let slotExisted = [];
    let entity = null;
    let updateBtn = $('#updateAppointmentBtn');
    let selectedSlot = null;

    const dropdownStatus = $('#appointmentSelectStatus');
    const newDate = $('#new-date');
    const timeSlots = $('#time-slots');
    const rescheduleSection = $('#reschedule-section');
    const rescheduleRow = $('#reschedule-row');
    const nextPageBtn = $('#nextPage');
    const previousPageBtn = $('#previousPage');
    const diagnoseSection = $('#diagnose-section');
    const diagnoseText = $('#diagnoseText');

    nextPageBtn.on('click', async function () {
        await loadPage($(this).data("page"));
    })
    previousPageBtn.on('click', async function () {
        await loadPage($(this).data("page"));
    })


    newDate.on('change', async function () {
        await fetchSlotsByDate(new Date($(this).val()).toISOString().split('T')[0]);
    })

    $('.nextWeekBtn').on('click', async function () {
        await fetchNextWeek($('#inputDate').val(), $(this).data('next'));
    })

    $('#inputDate').on('change', async function () {
        await fetchNextWeek($('#inputDate').val(), 0);
    })

    $('.appointment-card').on('click', async function () {
        await fetchAppointment($(this).data('id'), $(this).data('servicetype'));
    })


    $(dropdownStatus).on('change', async function () {
        const value = $(dropdownStatus).val();
        if (value === '4') {
            await showRescheduleSection();
            hideInputDiagnose();
        }
        else if (value == '7') {
            hideRescheduleSection();
            showInputDiagnose();
        } else {
            hideRescheduleSection();
            hideInputDiagnose();
        }
        handleButtonAppointment(appointmentStatuses[value]);
    });
    appointmentModal.on('show.bs.modal', async function () {
        if ($(dropdownStatus).val() === '4') {
            await showRescheduleSection();
        }
    });

    appointmentModal.on('hidden.bs.modal', function () {
        hideRescheduleSection();
    });


    async function showRescheduleSection() {
        newDate.val(new Date().toISOString().split('T')[0]);
        await fetchLastDateNoSlots(newDate.val());
        rescheduleSection.addClass('active');
    }

    function showInputDiagnose() {
        diagnoseSection.addClass('active');
        diagnoseText.prop('readonly', false);
    }

    function hideInputDiagnose() {
        diagnoseSection.removeClass('active');
    }

    function setEventSlots() {
        divSlots.forEach(div => {
            div.on('click', function () {
                if (!div.hasClass('disable')) {
                    divSlots.forEach(x => x.removeClass('selected'));
                    $(this).addClass('selected');
                    selectedSlot = $(this).text();
                    console.log(selectedSlot);
                }
            })
        })
    }

    function hideRescheduleSection() {
        timeSlots.empty();
        divSlots = [];
        rescheduleSection.removeClass('active');
    }

    function setAppointmentDetail(entity) {
        console.log(entity);
        entity = entity;
        idAppointment = entity.id;
        $('.patient-name').text(entity.patient.user.fullname);
        $('.patient-genderAge').text(entity.age + " tuổi" + " | " + entity.patient?.user?.gender);
        $('.patient-email').text(entity.patient?.user?.email);
        $('.patient-phone').text(entity.patient?.user?.phoneNumber);
        $('.modal-edit-status').text(getStatusText(appointmentStatuses[entity.status]));
        $('.modal-edit-date').text(new Date(entity.date).toLocaleDateString('vi-VN'));
        $('.modal-edit-hour').text(new Date(entity.date).toLocaleTimeString('vi-VN'));
        $('.notes-content').text(entity.description);
        $('.modal-edit-totalPrice').text(`${entity.payment.price} VNĐ`);
        if (entity.privateService !== null) {
            $('.service-name').text(entity.privateService.name);
            $('.modal-edit-servicePrice').text(`${entity.privateService.price} VNĐ`);
            $('.service-description').text(entity.privateService.description);
        } else {
            $('.service-name').text(entity.publicService.name);
            $('.modal-edit-servicePrice').text(`${entity.publicService.price} VNĐ`);
            $('.service-description').text(entity.publicService.description);
        }
        if (entity.diagnose) {
            diagnoseText.prop('readonly', true);
        }
        handleButtonAppointment(appointmentStatuses[entity.status]);
        getStatusBgColor(appointmentStatuses[entity.status]);
    }


    async function fetchSlotsByDate(date) {
        try {
            const dateInput = date;
            const slots = generateTimeSlots(7, 16, 60);
            const params = new URLSearchParams({
                handler: "SlotsInDay",
                date: dateInput,
                slots: slots
            })
            const response = await fetch(`?${params.toString()}`, {
                method: 'GET'
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            slotExisted = data.existedSlots;
            timeSlots.empty();
            generateSlots();
            setEventSlots();

        } catch (err) {
            console.log(err);
        }
    }

    async function fetchLastDateNoSlots(date) {
        try {
            const dateInput = date;
            const slots = generateTimeSlots(7, 16, 60);
            const params = new URLSearchParams({
                handler: "SlotsInDay",
                date: dateInput,
                slots: slots
            })
            const response = await fetch(`?${params.toString()}`, {
                method: 'GET'
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            slotExisted = data.existedSlots;
            const currentDate = new Date(date);
            if (slotExisted.length === generateTimeSlots(7, 16, 60).length) {
                await fetchLastDateNoSlots(currentDate.setDate(currentDate.getDate() + 1));
            } else {
                newDate.val(currentDate.toISOString().split('T')[0]);
                timeSlots.empty();
                generateSlots();
                setEventSlots();
            }

        } catch (err) {
            console.log(err);
        }
    }


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

            dropdownStatus.empty();
            data.availableStatuses.forEach(x => {
                dropdownStatus.append($('<option>', {
                    value: x,
                    text: appointmentStatuses[x]
                }));
            });
            dropdownStatus.val(data.appointment.status);

            appointmentModal.modal('show');
        } catch (err) {
            console.log(err);
        }
    }

    function generateSlots() {
        generateTimeSlots(7, 16, 60).forEach(slot => {
            const existed = slotExisted.find(value => value === slot);
            const div = $('<div>', {
                class: `time-slot ${existed ? "disable" : ""}`,
                'data-time': slot,
                text: slot
            }).appendTo(timeSlots);
            divSlots.push(div);
        });
    }

    function generateTimeSlots(startHour, endHour, interval) {
        const slots = [];
        const start = startHour * 60;
        const end = endHour * 60;
        for (let time = start; time <= end; time += interval) {
            const hours = Math.floor(time / 60);
            const minutes = time % 60;
            const formatDate = `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
            slots.push(formatDate);
        }
        return slots;
    }

    function handleButtonAppointment(status) {
        const wrapper = $('#btnModalWrapper');
        switch (status) {
            default:
                if (status === appointmentStatuses[entity.status]) {
                    updateBtn.off('click');
                    wrapper.empty();
                } else {
                    wrapper.html(updateBtn.prop('outerHTML'));
                    updateBtn = $('#updateAppointmentBtn');
                }
                break;
        }
        updateBtn?.on('click', async function () {
            await changeAppointmentStatus(dropdownStatus.val());
        })
    }
    async function fetchNextWeek(date, dayNext) {
        try {
            const params = new URLSearchParams({
                handler: 'NextWeek',
                monday: date,
                next: dayNext
            });
            const response = await fetch(`?${params.toString()}`, {
                method: 'GET'
            })
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.text();
            $('#patientAppointments').empty();
            $('#patientAppointments').append(data);
            AddEvents();
        } catch (err) {
            console.log(err);
        }
    }
    async function changeAppointmentStatus(status) {
        try {
            const params = new URLSearchParams({
                handler: 'ChangeAppointmentStatus',
                id: idAppointment,
                status: status,
                date: newDate.val()
            });
            if (appointmentStatuses[status] === "Rescheduled" && selectedSlot && entity) {
                params.append('slot', selectedSlot);
                params.append('entity', JSON.stringify(entity));
            } else if (appointmentStatuses[status] === "Completed") {
                if (diagnoseText.val().length == 0) {
                    alert("Vui lòng nhập chẩn đoán!");
                    return;
                }
                params.append('diagnose', diagnoseText.val());
            }
            const response = await fetch(`?${params.toString()}`, {
                method: "GET"
            })
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.text();
            $('#patientAppointments').empty();
            $('#patientAppointments').append(data);
            await loadPage(1);
            AddEvents();
            appointmentModal.modal("hide");
        } catch (err) {
            console.log(err);
        } finally {

        }
    }

    async function loadPage(numPage) {
        try {
            const params = new URLSearchParams({
                handler: 'LoadPage',
                pagee: numPage
            });
            const response = await fetch(`?${params.toString()}`, {
                method: 'GET'
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.text();
            $('#patientRecords').empty();
            $('#patientRecords').append(data);
            AddEvents();
        } catch (err) {
            console.log(err);
        }
    }

    function getStatusText(status) {
        switch (status) {
            case "Pending":
                return "Đang chờ xác nhận";
            case "Cancelled":
                return "Đã từ chối"
            case "Confirmed":
                return "Đã xác nhận";
            case "Completed":
                return "Đã khám";
            case "Rejected":
                return "Đã từ chối";
            case "Rescheduled":
                return "Cuộc hẹn đặt lại";
            default:
                return "Không xác định";
        }
    }

    function getStatusBgColor(status) {
        var statusdiv = document.querySelector('.modal-edit-status');
        switch (status) {
            case "Cancelled":
            case "Rescheduled":
                statusdiv.style.backgroundColor = 'rgb(255, 87, 34,20%)';
                statusdiv.style.color = 'rgb(255, 87, 34)';
                break;
            case "Confirmed":
                statusdiv.style.backgroundColor = 'rgb(0, 100, 0,20%)';
                statusdiv.style.color = 'rgb(0, 100, 0)';
                break;
            case "Completed":
            case "Pending":
                statusdiv.style.backgroundColor = 'rgb(0, 128, 0 / 20%)';
                statusdiv.style.color = 'rgb(0, 128, 0)';
                break;
            case "Rejected":
                statusdiv.style.backgroundColor = 'rgb(204, 0, 0/20%)';
                statusdiv.style.color = 'rgb(204, 0, 0)';
                break;
            default:
                return "Không xác định";
        }
    }
}
