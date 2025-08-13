document.addEventListener('DOMContentLoaded', function () {
    console.log("Tài liệu đã được tải xong");

    Promise.all([
        loadScript('https://cdnjs.cloudflare.com/ajax/libs/chart.js/3.9.1/chart.min.js'),
        loadScript('https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/leaflet.js'),
        loadCSS('https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/leaflet.css')
    ]).then(() => {
        console.log("Tất cả thư viện đã được tải");
        initCharts();
        initVietnamMap(); 
    }).catch(error => {
        console.error("Không thể tải thư viện:", error);
        showErrorMessage("Không thể tải thư viện cần thiết. Vui lòng làm mới trang hoặc kiểm tra kết nối internet.");
    });
});

function loadScript(url) {
    return new Promise((resolve, reject) => {
        if (document.querySelector(`script[src="${url}"]`)) {
            resolve(); // Script đã được tải
            return;
        }

        const script = document.createElement('script');
        script.src = url;
        script.onload = resolve;
        script.onerror = reject;
        document.head.appendChild(script);
    });
}

function loadCSS(url) {
    return new Promise((resolve, reject) => {
        if (document.querySelector(`link[href="${url}"]`)) {
            resolve(); 
            return;
        }

        const link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = url;
        link.onload = resolve;
        link.onerror = reject;
        document.head.appendChild(link);
    });
}

// Hiển thị thông báo lỗi
function showErrorMessage(message) {
    const containers = document.querySelectorAll('.chart-container, #vietnam-map');
    containers.forEach(container => {
        container.innerHTML = `<div class="alert alert-danger">${message}</div>`;
    });
}

// Khởi tạo tất cả các biểu đồ
function initCharts() {
    try {
        const months = ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6',
            'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'];

        // Khởi tạo từng biểu đồ
        initPieChart(appointmentStatusData);
        initMonthlyPaymentChart(months);
        initMonthlyAppointmentChart(months);
        initPaymentByServiceChart();
    } catch (error) {
        console.error("Lỗi khi khởi tạo biểu đồ:", error);
        showErrorMessage("Không thể khởi tạo biểu đồ. Vui lòng làm mới trang.");
    }
}

//PIE CHART
function initPieChart(statusData) {
    const pieCtx = document.getElementById('pieChart');
    if (!pieCtx) return;

    const labels = statusData.map(s => s.label);
    const data = statusData.map(s => s.count);

    new Chart(pieCtx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: ['#99d255', '#7da73d', '#8bc249', '#f1c40f', '#e67e22', '#e74c3c', '#b5e285', '#d2eab7'],
                borderColor: '#fff',
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'right',
                }
            }
        }
    });
}


// Biểu đồ cột - Thanh toán hàng tháng
function initMonthlyPaymentChart(months) {
    const paymentCtx = document.getElementById('monthlyPaymentChart');
    if (!paymentCtx) {
        console.error("Không tìm thấy phần tử #monthlyPaymentChart");
        return;
    }

    const monthlyTotals = Array(12).fill(0);

    if (Array.isArray(monthlyPaymentData)) {
        monthlyPaymentData.forEach(item => {
            const index = item.month - 1;
            if (index >= 0 && index < 12) {
                monthlyTotals[index] = item.total;
            }
        });
    }

    new Chart(paymentCtx, {
        type: 'bar',
        data: {
            labels: months,
            datasets: [{
                label: 'Tổng Thanh Toán (VND)',
                data: monthlyTotals,
                backgroundColor: '#99d255',
                borderColor: '#7da73d',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });

    console.log("Biểu đồ thanh toán hàng tháng đã được khởi tạo");
}

// Biểu đồ đường - Lịch hẹn hàng tháng
function initMonthlyAppointmentChart(months) {
    const appointmentCtx = document.getElementById('monthlyAppointmentChart');
    if (!appointmentCtx) {
        console.error("Không tìm thấy phần tử #monthlyAppointmentChart");
        return;
    }

    const monthlyCounts = Array(12).fill(0);

    if (Array.isArray(monthlyAppointmentData)) {
        monthlyAppointmentData.forEach(item => {
            const index = item.month - 1;
            if (index >= 0 && index < 12) {
                monthlyCounts[index] = item.count;
            }
        });
    }

    new Chart(appointmentCtx, {
        type: 'line',
        data: {
            labels: months,
            datasets: [{
                label: 'Số Lượng Cuộc Hẹn',
                data: monthlyCounts,
                backgroundColor: 'rgba(153, 210, 85, 0.2)',
                borderColor: '#99d255',
                borderWidth: 2,
                tension: 0.3,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1
                    }
                }
            }
        }
    });

    console.log("Biểu đồ lịch hẹn hàng tháng đã được khởi tạo");
}


function initPaymentByServiceChart() {
    const serviceCtx = document.getElementById('paymentByServiceChart');
    if (!serviceCtx) {
        console.error("Không tìm thấy phần tử #paymentByServiceChart");
        return;
    }

    const labels = revenueByProviderData.map(item => item.label);
    const data = revenueByProviderData.map(item => item.total);

    new Chart(serviceCtx, {
        type: 'doughnut',
        data: {
            labels,
            datasets: [{
                data: data,
                backgroundColor: ['#99d255', '#7da73d'],
                borderColor: '#fff',
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom',
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return `${context.label}: ${context.parsed.toLocaleString('vi-VN')} VND`;
                        }
                    }
                }
            }
        }
    });

    console.log("Biểu đồ doanh thu theo loại nhà cung cấp đã được khởi tạo");
}


const vietnamProvinces = {
    
    "type": "FeatureCollection",
        "features": [
            {
                "type": "Feature",
                "properties": {
                    "name": "Hà Nội",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.8542,
                        21.0285
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Hồ Chí Minh",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.6602,
                        10.7626
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Đà Nẵng",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        108.2062,
                        16.0471
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Hải Phòng",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.6881,
                        20.8449
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Cần Thơ",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.722,
                        10.0341
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "An Giang",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.1259,
                        10.5216
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Bà Rịa - Vũng Tàu",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        107.2428,
                        10.5417
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Bắc Giang",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.1975,
                        21.281
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Bắc Kạn",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.8348,
                        22.146
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Bạc Liêu",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.7278,
                        9.2941
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Bắc Ninh",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.0763,
                        21.1861
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Bến Tre",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.3759,
                        10.2415
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Bình Định",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        109.2193,
                        13.782
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Bình Dương",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.6578,
                        11.3254
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Bình Phước",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.9046,
                        11.754
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Bình Thuận",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        108.0721,
                        11.0904
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Cà Mau",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.15,
                        9.1765
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Cao Bằng",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.257,
                        22.6657
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Đắk Lắk",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        108.2378,
                        12.71
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Đắk Nông",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        107.6098,
                        12.2646
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Điện Biên",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        103.023,
                        21.3974
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Đồng Nai",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.8245,
                        10.9453
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Đồng Tháp",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.6219,
                        10.457
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Gia Lai",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        108.1098,
                        13.8079
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Hà Giang",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        104.9836,
                        22.8233
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Hà Nam",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.9229,
                        20.5411
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Hà Tĩnh",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.8875,
                        18.3559
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Hậu Giang",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.6419,
                        9.7579
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Hoà Bình",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.3376,
                        20.8526
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Hưng Yên",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.0511,
                        20.646
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Khánh Hòa",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        109.0526,
                        12.2585
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Kiên Giang",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.08,
                        10.0125
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Kon Tum",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        107.9883,
                        14.3498
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Lai Châu",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        103.4702,
                        22.3862
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Lâm Đồng",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        108.4583,
                        11.9404
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Lạng Sơn",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.7615,
                        21.8537
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Lào Cai",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        103.9706,
                        22.4851
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Long An",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.4137,
                        10.5354
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Nam Định",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.1621,
                        20.4388
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Nghệ An",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        104.92,
                        19.2342
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Ninh Bình",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.9745,
                        20.2505
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Ninh Thuận",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        108.9916,
                        11.6739
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Phú Thọ",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.4012,
                        21.3227
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Phú Yên",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        109.0929,
                        13.0882
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Quảng Bình",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.6223,
                        17.4689
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Quảng Nam",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        108.0191,
                        15.5394
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Quảng Ngãi",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        108.8044,
                        15.1214
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Quảng Ninh",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        107.2925,
                        21.0064
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Quảng Trị",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        107.189,
                        16.7503
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Sóc Trăng",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.9739,
                        9.602
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Sơn La",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        103.9188,
                        21.325
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Tây Ninh",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.098,
                        11.3114
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Thái Bình",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.3366,
                        20.4463
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Thái Nguyên",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.8482,
                        21.5942
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Thanh Hóa",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.7852,
                        19.8067
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Thừa Thiên Huế",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        107.5907,
                        16.4637
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Tiền Giang",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.3421,
                        10.4494
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Trà Vinh",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        106.2993,
                        9.8127
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Tuyên Quang",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.214,
                        21.8231
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Vĩnh Long",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.956,
                        10.25
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Vĩnh Phúc",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        105.6049,
                        21.3089
                    ]
                }
            },
            {
                "type": "Feature",
                "properties": {
                    "name": "Yên Bái",
                    "facilities": 0,
                    "doctors": 0
                },
                "geometry": {
                    "type": "Point",
                    "coordinates": [
                        104.8702,
                        21.7051
                    ]
                }
            }
        ]
}

function normalizeProvince(name) {
    return name
        .replace(/^tỉnh|thành phố/gi, "")
        .trim()
        .toLowerCase();
}

function initVietnamMap() {
    const mapElement = document.getElementById('vietnam-map');
    if (!mapElement) {
        console.error("Không tìm thấy phần tử #vietnam-map");
        return;
    }

    console.log("Bắt đầu khởi tạo bản đồ");

    if (!document.getElementById('map-styles')) {
        const style = document.createElement('style');
        style.id = 'map-styles';
        style.textContent = `
            #vietnam-map {
                width: 100%;
                height: 500px;
                background-color: white;
                border-radius: 12px;
                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
            }
            .info {
                padding: 6px 8px;
                font: 14px/16px 'Lexend Deca', Arial, Helvetica, sans-serif;
                background: rgba(255, 255, 255, 0.8);
                box-shadow: 0 0 15px rgba(0, 0, 0, 0.2);
                border-radius: 5px;
            }
            .legend {
                line-height: 18px;
                color: #555;
            }
            .legend i {
                width: 18px;
                height: 18px;
                float: left;
                margin-right: 8px;
                opacity: 0.7;
            }
        `;
        document.head.appendChild(style);
    }

    mapElement.style.height = '500px';

    const vietnamMap = L.map('vietnam-map').setView([16.0544, 108.2022], 6);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(vietnamMap);

    if (typeof provinceData !== 'undefined') {
        vietnamProvinces.features.forEach(feature => {
            const geoProvinceKey = normalizeProvince(feature.properties.name);
            const matched = provinceData.find(p => normalizeProvince(p.province) === geoProvinceKey);

            feature.properties.facilities = matched?.facilityCount || 0;
            feature.properties.doctors = matched?.professionalCount || 0;
        });
    }

    function getColor(d) {
        return d > 50 ? '#004000' : 
            d > 30 ? '#006400' :     
                d > 20 ? '#008000' :   
                    d > 10 ? '#228B22' :   
                        d > 5 ? '#3CB371' :    
                            '#2E8B57';       
    }

    function style(feature) {
        return {
            radius: Math.sqrt(feature.properties.facilities) * 5,
            fillColor: getColor(feature.properties.facilities),
            color: "#fff",
            weight: 1,
            opacity: 1,
            fillOpacity: 0.8
        };
    }

    function onEachFeature(feature, layer) {
        const { name, facilities, doctors } = feature.properties;
        layer.bindPopup(
            `<b>${name}</b><br>` +
            `Cơ sở y tế: ${facilities}<br>` +
            `Chuyên gia y tế: ${doctors}`
        );
    }

    L.geoJSON(vietnamProvinces, {
        pointToLayer: (feature, latlng) => L.circleMarker(latlng, style(feature)),
        onEachFeature
    }).addTo(vietnamMap);

    const legend = L.control({ position: 'bottomright' });
    legend.onAdd = function () {
        const div = L.DomUtil.create('div', 'info legend');
        const grades = [0, 5, 10, 20, 30, 50];
        const labels = [];

        for (let i = 0; i < grades.length; i++) {
            const from = grades[i];
            const to = grades[i + 1];

            labels.push(
                `<i style="background:${getColor(from + 1)}"></i> ` +
                from + (to ? `&ndash;${to}` : '+') + ' cơ sở/chuyên gia y tế'
            );
        }

        div.innerHTML = '<h4>Số lượng cơ sở/chuyên gia y tế</h4>' + labels.join('<br>');
        return div;
    };
    legend.addTo(vietnamMap);

    setTimeout(() => {
        vietnamMap.invalidateSize();
    }, 200);

    console.log("Bản đồ đã được khởi tạo thành công");
}

