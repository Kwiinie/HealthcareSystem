////////////////////////////////////////////////////////////////////////////
///                         CALENDAR SCRIPT                             ///
//////////////////////////////////////////////////////////////////////////

const daysTag = document.querySelector(".days"),
    currentDate = document.querySelector(".current-date"),
    prevNextIcon = document.querySelectorAll(".icons span");

// getting new date, current year and month
let date = new Date(),
    currYear = date.getFullYear(),
    currMonth = date.getMonth();

let selectedDate = new Date(); // Store the selected date, default to today

const months = ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7",
    "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"];

const renderCalendar = () => {
    let firstDayofMonth = new Date(currYear, currMonth, 1).getDay(), 
        lastDateofMonth = new Date(currYear, currMonth + 1, 0).getDate(), 
        lastDayofMonth = new Date(currYear, currMonth, lastDateofMonth).getDay(),
        lastDateofLastMonth = new Date(currYear, currMonth, 0).getDate(); 

    let liTag = "";

    for (let i = firstDayofMonth; i > 0; i--) {
        liTag += `<li class="inactive">${lastDateofLastMonth - i + 1}</li>`;
    }

    const today = new Date();
    today.setHours(0, 0, 0, 0); 

    for (let i = 1; i <= lastDateofMonth; i++) {
        const currentDate = new Date(currYear, currMonth, i);
        currentDate.setHours(0, 0, 0, 0);

        const isPastDate = currentDate < today;

        const isSelectedDate = i === selectedDate.getDate() &&
            currMonth === selectedDate.getMonth() &&
            currYear === selectedDate.getFullYear();

        let className = "";
        if (isSelectedDate) {
            className = "active";
        }

        if (isPastDate) {
            liTag += `<li class="inactive" style="cursor: not-allowed;">${i}</li>`;
        } else {
            liTag += `<li class="${className}" data-date="${currYear}-${currMonth + 1}-${i}" onclick="selectDate(${currYear}, ${currMonth}, ${i})">${i}</li>`;
        }
    }

    for (let i = lastDayofMonth; i < 6; i++) {
        liTag += `<li class="inactive">${i - lastDayofMonth + 1}</li>`;
    }

    currentDate.innerText = `${months[currMonth]} ${currYear}`;
    daysTag.innerHTML = liTag;
}

function selectDate(year, month, day) {
    const newSelectedDate = new Date(year, month, day);

    if (newSelectedDate.getDay() === 0) {
        alert("Không có lịch vào Chủ nhật");
        return;
    }

    selectedDate = newSelectedDate;

    renderCalendar();

    const formattedDate = `${year}-${month + 1}-${day}`;
    document.getElementById('SelectedDateInput').value = formattedDate;
    document.getElementById('dateSelectionForm').submit();
}

renderCalendar();

prevNextIcon.forEach(icon => {
    icon.addEventListener("click", () => { 
        currMonth = icon.id === "prev" ? currMonth - 1 : currMonth + 1;

        if (currMonth < 0 || currMonth > 11) { 
            date = new Date(currYear, currMonth, new Date().getDate());
            currYear = date.getFullYear();
            currMonth = date.getMonth();
        }

        renderCalendar(); 
    });
});