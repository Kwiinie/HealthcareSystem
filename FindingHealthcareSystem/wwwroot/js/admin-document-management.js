// Admin Document Management JavaScript

document.addEventListener('DOMContentLoaded', function () {
    initializeDocumentManagement();
    loadCharts();
});

function initializeDocumentManagement() {
    console.log('Document management page initialized');
}

function loadCharts() {
    loadDocumentTypeChart();
    loadVerificationStatusChart();
}

function loadDocumentTypeChart() {
    fetch('?handler=DocumentTypeStats')
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                const ctx = document.getElementById('documentTypeChart').getContext('2d');
                new Chart(ctx, {
                    type: 'doughnut',
                    data: {
                        labels: data.data.map(item => item.typeName),
                        datasets: [{
                            data: data.data.map(item => item.count),
                            backgroundColor: [
                                '#007bff',
                                '#28a745',
                                '#ffc107',
                                '#dc3545',
                                '#6f42c1',
                                '#fd7e14',
                                '#20c997'
                            ],
                            borderWidth: 2,
                            borderColor: '#fff'
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: {
                                position: 'bottom',
                                labels: {
                                    padding: 20,
                                    usePointStyle: true
                                }
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (context) {
                                        const total = context.dataset.data.reduce((a, b) => a + b, 0);
                                        const percentage = ((context.parsed / total) * 100).toFixed(1);
                                        return `${context.label}: ${context.parsed} (${percentage}%)`;
                                    }
                                }
                            }
                        }
                    }
                });
            }
        })
        .catch(error => {
            console.error('Error loading document type chart:', error);
        });
}

function loadVerificationStatusChart() {
    fetch('?handler=VerificationStatusStats')
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                const ctx = document.getElementById('verificationStatusChart').getContext('2d');
                new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: data.data.map(item => item.statusName),
                        datasets: [{
                            label: 'Số lượng',
                            data: data.data.map(item => item.count),
                            backgroundColor: data.data.map(item => {
                                switch (item.status.toLowerCase()) {
                                    case 'pendingverification': return '#ffc107';
                                    case 'underreview': return '#17a2b8';
                                    case 'verified': return '#28a745';
                                    case 'rejected': return '#dc3545';
                                    default: return '#6c757d';
                                }
                            }),
                            borderColor: data.data.map(item => {
                                switch (item.status.toLowerCase()) {
                                    case 'pendingverification': return '#e0a800';
                                    case 'underreview': return '#138496';
                                    case 'verified': return '#1e7e34';
                                    case 'rejected': return '#bd2130';
                                    default: return '#495057';
                                }
                            }),
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: {
                                display: false
                            }
                        },
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
            }
        })
        .catch(error => {
            console.error('Error loading verification status chart:', error);
        });
}

function viewDocumentDetails(documentId) {
    // Redirect to document verification page with specific document
    window.location.href = `/admin/document-verification?documentId=${documentId}`;
}

function refreshData() {
    showAlert('Đang làm mới dữ liệu...', 'info');
    setTimeout(() => {
        window.location.reload();
    }, 500);
}

function exportDocuments() {
    // Implement export functionality
    showAlert('Chức năng xuất Excel đang được phát triển', 'info');
}

function sendRenewalReminders() {
    // Implement renewal reminder functionality
    if (confirm('Bạn có chắc chắn muốn gửi nhắc nhở gia hạn cho tất cả chứng chỉ sắp hết hạn?')) {
        showAlert('Đang gửi nhắc nhở...', 'info');
        // Implement the actual sending logic here
        setTimeout(() => {
            showAlert('Đã gửi nhắc nhở gia hạn thành công', 'success');
        }, 2000);
    }
}

function generateReport() {
    // Implement report generation
    showAlert('Chức năng tạo báo cáo đang được phát triển', 'info');
}

function showAlert(message, type = 'info') {
    // Remove existing alerts
    const existingAlerts = document.querySelectorAll('.alert-auto-dismiss');
    existingAlerts.forEach(alert => alert.remove());

    // Create alert element
    const alertClasses = {
        'success': 'alert-success',
        'error': 'alert-danger',
        'warning': 'alert-warning',
        'info': 'alert-info'
    };

    const alertIcons = {
        'success': 'fas fa-check-circle',
        'error': 'fas fa-exclamation-circle',
        'warning': 'fas fa-exclamation-triangle',
        'info': 'fas fa-info-circle'
    };

    const alert = document.createElement('div');
    alert.className = `alert ${alertClasses[type] || 'alert-info'} alert-dismissible fade show alert-auto-dismiss`;
    alert.style.position = 'fixed';
    alert.style.top = '20px';
    alert.style.right = '20px';
    alert.style.zIndex = '9999';
    alert.style.minWidth = '300px';

    alert.innerHTML = `
        <i class="${alertIcons[type] || 'fas fa-info-circle'} me-2"></i>
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;

    document.body.appendChild(alert);

    // Auto dismiss after 5 seconds
    setTimeout(() => {
        if (alert.parentNode) {
            alert.remove();
        }
    }, 5000);
}

// Make functions globally available
window.viewDocumentDetails = viewDocumentDetails;
window.refreshData = refreshData;
window.exportDocuments = exportDocuments;
window.sendRenewalReminders = sendRenewalReminders;
window.generateReport = generateReport;