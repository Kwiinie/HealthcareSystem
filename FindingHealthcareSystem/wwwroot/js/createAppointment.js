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

let selectedDate = new Date(); // default today

const months = [
    "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
    "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
];

// ---------- Helpers: read inputs ----------
function readScheduleBounds() {
    const startVal = document.getElementById("ScheduleStartDate")?.value?.trim();
    const endVal = document.getElementById("ScheduleEndDate")?.value?.trim();
    const start = startVal ? new Date(startVal + "T00:00:00") : null;
    const end = endVal ? new Date(endVal + "T00:00:00") : null;
    if (start) start.setHours(0, 0, 0, 0);
    if (end) end.setHours(0, 0, 0, 0);
    return { start, end };
}

// DB weekday: 1..7 (CN..T7)  -> JS getDay(): 0..6 (CN..T7)
function mapDbWeekdayToJs(dbW) {
    if (dbW === 1) return 0;       // CN
    if (dbW >= 2 && dbW <= 7) return dbW - 1; // T2..T7
    return null;
}

function readWorkingWeekdays() {
    const raw = document.getElementById("WorkingWeekdays")?.value?.trim();
    if (!raw) return new Set(); // không có -> khóa hết
    let arr;
    try { arr = JSON.parse(raw); }
    catch { arr = raw.split(",").map(s => parseInt(s.trim(), 10)); }
    const jsDays = arr
        .map(n => mapDbWeekdayToJs(Number(n)))
        .filter(v => v !== null && !Number.isNaN(v));
    return new Set(jsDays); // values: 0..6
}

function readClosedExceptionDates() {
    const raw = document.getElementById("ClosedExceptionDates")?.value?.trim();
    if (!raw) return new Set();
    let arr;
    try { arr = JSON.parse(raw); }
    catch { arr = raw.split(",").map(s => s.trim()); }
    // lưu chuỗi 'yyyy-MM-dd' để so trùng nhanh
    return new Set(arr.filter(Boolean));
}

function dateToYmd(d) {
    const y = d.getFullYear();
    const m = String(d.getMonth() + 1).padStart(2, "0");
    const day = String(d.getDate()).padStart(2, "0");
    return `${y}-${m}-${day}`;
}

let { start: scheduleStart, end: scheduleEnd } = readScheduleBounds();
let workingDows = readWorkingWeekdays();            // Set(0..6)
let closedExceptionDates = readClosedExceptionDates(); // Set('yyyy-MM-dd')

// ---------- Render calendar ----------
const renderCalendar = () => {
    const firstDayofMonth = new Date(currYear, currMonth, 1).getDay();
    const lastDateofMonth = new Date(currYear, currMonth + 1, 0).getDate();
    const lastDayofMonth = new Date(currYear, currMonth, lastDateofMonth).getDay();
    const lastDateofLastMon = new Date(currYear, currMonth, 0).getDate();

    let liTag = "";

    // leading inactive (prev month)
    for (let i = firstDayofMonth; i > 0; i--) {
        liTag += `<li class="inactive">${lastDateofLastMon - i + 1}</li>`;
    }

    const today = new Date(); today.setHours(0, 0, 0, 0);
    const hasBounds = !!scheduleStart && !!scheduleEnd;

    for (let i = 1; i <= lastDateofMonth; i++) {
        const d = new Date(currYear, currMonth, i);
        d.setHours(0, 0, 0, 0);

        const ymd = dateToYmd(d);

        const isPastDate = d < today;

        const inBounds = hasBounds ? (d >= scheduleStart && d <= scheduleEnd) : false;
        const allowedWeekday = workingDows.size > 0 ? workingDows.has(d.getDay()) : false;
        const isClosedException = closedExceptionDates.has(ymd);

        const isSelected =
            i === selectedDate.getDate() &&
            currMonth === selectedDate.getMonth() &&
            currYear === selectedDate.getFullYear();

        // inactive nếu: quá khứ OR ngoài khoảng OR không thuộc weekday làm việc OR bị close bởi exception
        const mustBeInactive = isPastDate || !inBounds || !allowedWeekday || isClosedException;

        if (mustBeInactive) {
            liTag += `<li class="inactive" title="${isClosedException ? "Nghỉ/đóng lịch" : ""}" style="cursor: not-allowed;">${i}</li>`;
        } else {
            const className = isSelected ? "active" : "";
            liTag += `<li class="${className}" data-date="${ymd}" onclick="selectDate(${currYear}, ${currMonth}, ${i})">${i}</li>`;
        }
    }

    // trailing inactive (next month)
    for (let i = lastDayofMonth; i < 6; i++) {
        liTag += `<li class="inactive">${i - lastDayofMonth + 1}</li>`;
    }

    currentDate.innerText = `${months[currMonth]} ${currYear}`;
    daysTag.innerHTML = liTag;
};

// ---------- Select date ----------
function selectDate(year, month, day) {
    const newSelectedDate = new Date(year, month, day);
    newSelectedDate.setHours(0, 0, 0, 0);

    const ymd = dateToYmd(newSelectedDate);
    const hasBounds = !!scheduleStart && !!scheduleEnd;

    if (!hasBounds || newSelectedDate < scheduleStart || newSelectedDate > scheduleEnd) {
        alert("Ngày đã chọn nằm ngoài thời gian làm việc của bác sĩ.");
        return;
    }
    if (!workingDows.has(newSelectedDate.getDay())) {
        alert("Ngày này không thuộc lịch làm việc của bác sĩ.");
        return;
    }
    if (closedExceptionDates.has(ymd)) {
        alert("Bác sĩ tạm đóng lịch ngày này.");
        return;
    }

    selectedDate = newSelectedDate;
    renderCalendar();

    document.getElementById("SelectedDateInput").value = ymd;
    document.getElementById("dateSelectionForm").submit();
}

// init
renderCalendar();

prevNextIcon.forEach(icon => {
    icon.addEventListener("click", () => {
        currMonth = icon.id === "prev" ? currMonth - 1 : currMonth + 1;
        if (currMonth < 0 || currMonth > 11) {
            const tmp = new Date(currYear, currMonth, 1);
            currYear = tmp.getFullYear();
            currMonth = tmp.getMonth();
        }
        renderCalendar();
    });
});

// re-read after DOM ready (phòng trường hợp input load chậm)
document.addEventListener("DOMContentLoaded", () => {
    ({ start: scheduleStart, end: scheduleEnd } = readScheduleBounds());
    workingDows = readWorkingWeekdays();
    closedExceptionDates = readClosedExceptionDates();
    renderCalendar();
});
